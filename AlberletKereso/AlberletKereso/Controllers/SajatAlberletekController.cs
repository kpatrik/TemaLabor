using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlberletKereso.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Drawing;

namespace AlberletKereso.Controllers
{

    public class SajatAlberletekController : Controller
    {
        //private ApplicationDbContext db;


        private UnitOfWork unitOfWork = new UnitOfWork();

        //protected ApplicationUserManager UserManager { get; set; }

        // GET: SajatAlberletek
        public ActionResult Index()
        {
            var userId = unitOfWork.UserManager.FindById(User.Identity.GetUserId()).Id;

            var alberletek = unitOfWork.AlberletRepository.Get(filter: f => f.Hirdeto.Id == userId);

            return View(alberletek);
        }

       
        public ActionResult KepHozzaadas()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KepHozzaadas(int? id)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
               
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Alberlet alberlet = unitOfWork.AlberletRepository.GetByID(id);

               
                if (alberlet == null)
                {
                    return HttpNotFound();
                }
                string extension = System.IO.Path.GetExtension(file.FileName);
                if(extension != ".jpeg" && extension != ".png"&& extension != ".jpg")
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                
                if (file != null && file.ContentLength > 0 )
                {
                    
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(HttpContext.Server.MapPath("~/Uploads/"), fileName);
                    file.SaveAs(path);
                    Image img = alberlet.resizeImage(640, 480, path);
                    img.Save(path);
                    Kep kep = new Kep(path, fileName, alberlet);
                    alberlet.Kepek.Add(kep);
                    unitOfWork.Save();
                }

            }

            return RedirectToAction("Index");
        }

        // GET: SajatAlberletek/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alberlet alberlet = unitOfWork.AlberletRepository.GetByID(id);
            if (alberlet == null)
            {
                return HttpNotFound();
            }
            return View(alberlet);
        }

        // GET: SajatAlberletek/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SajatAlberletek/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Alberlet model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = unitOfWork.UserManager.FindById(User.Identity.GetUserId());
            var ujalberlet = new Alberlet(model.Cim, model.Szobak_szama, model.Emelet, model.Mosdok_szama, model.Alapterulet, model.Ar, model.Berendezett, user);
            user.Hirdetesek.Add(ujalberlet);
            unitOfWork.UserManager.Update(user);

            iterateUsers(ujalberlet);

            unitOfWork.Save();
            return RedirectToAction("Index", new { Message = "Hirdetés feladva!" });
        }

        // GET: SajatAlberletek/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alberlet alberlet = unitOfWork.AlberletRepository.GetByID(id);
            if (alberlet == null)
            {
                return HttpNotFound();
            }
            return View(alberlet);
        }



        // POST: SajatAlberletek/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlberletId,Cim,Szobak_szama,Emelet,Mosdok_szama,Alapterulet,Ar,Berendezett")] Alberlet alberlet)
        {
            if (ModelState.IsValid)
            {

                alberlet.Hirdeto = unitOfWork.UserManager.FindById(User.Identity.GetUserId());
                iterateUsers(alberlet);
                return RedirectToAction("Index");
            }
            return View(alberlet);
        }

        // GET: SajatAlberletek/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Alberlet alberlet = unitOfWork.AlberletRepository.GetByID(id);

            if (alberlet == null)
            {
                return HttpNotFound();
            }
            return View(alberlet);
        }

        // POST: SajatAlberletek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alberlet alberlet = unitOfWork.AlberletRepository.GetByID(id);
            unitOfWork.AlberletRepository.Delete(alberlet);
            unitOfWork.Save();

            return RedirectToAction("Index");
        }

        private void iterateUsers(Alberlet albi)
        {

            foreach (var ite2 in unitOfWork.UserManager.Users)
            {
                var filters = unitOfWork.FilterRepository.Get(filter: f => f.feliratkozo.Id == ite2.Id);

                ite2.Filters = filters.ToList();
                foreach (Models.Filter item in ite2.Filters)
                {
                    if (
                        (item.Alapterulet == null || item.Alapterulet <= albi.Alapterulet)
                            && (item.Berendezett == null || item.Berendezett.Equals(albi.Berendezett) || !item.Berendezett.HasValue)
                            && (item.Cim == null || item.Cim.Equals(albi.Cim))
                            && (item.Emelet == null || item.Emelet <= albi.Emelet)
                            && (item.MaxAr == null || item.MaxAr >= albi.Ar)
                            && (item.MinAr == null || albi.Ar <= item.MinAr)
                            && (item.Mosdok_szama == null || item.Mosdok_szama <= albi.Mosdok_szama)
                            && (item.Szobak_szama == null || item.Szobak_szama <= albi.Szobak_szama)
                            && ite2.Id != albi.Hirdeto.Id) { }
                    sendMail(ite2, albi);

                }

            }


        }

        private void sendMail(ApplicationUser user, Alberlet albi)
        {
            MailMessage o = new MailMessage("temalabor@hotmail.com", user.Email, "Találat",
                "Megtaláltuk a neked megfelelő albérletet! \n " +
                "Az albérlet adatai: \n" +
                "Cím: " + albi.Cim + "\n" +
                "Szobák száma: " + albi.Szobak_szama + "\n" +
                "Emelet: " + albi.Emelet + "\n" +
                "Mosdok száma: " + albi.Mosdok_szama + "\n" +
                "Alapterület: " + albi.Alapterulet + "\n" +
                "Ár: " + albi.Ar);
            NetworkCredential netCred = new NetworkCredential("temalabor@hotmail.com", "valami123");
            SmtpClient smtpobj = new SmtpClient("smtp.live.com", 587);
            smtpobj.EnableSsl = true;
            smtpobj.Credentials = netCred;
            smtpobj.Send(o);
        }
    }

   
}
