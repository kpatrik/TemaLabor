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

namespace AlberletKereso.Controllers
{
   
    public class SajatAlberletekController : Controller
    {
        //private ApplicationDbContext db;
       

        public SajatAlberletekController()
        {
           
        }

        //protected ApplicationUserManager UserManager { get; set; }

        // GET: SajatAlberletek
        public ActionResult Index()
        {
            var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>();

            var userID = UserManager.FindById(User.Identity.GetUserId()).Id;
            var alberletek = from a in db.Alberletek
                             where a.Hirdeto.Id == userID
                             select a;

            return View(alberletek.ToList());
        }

        public ActionResult KepNezegeto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alberlet alberlet = db.Alberletek.Find(id);
            if (alberlet == null)
            {
                return HttpNotFound();
            }
            var Kepek = from k in db.Keps
                        where k.Alberlet.AlberletId == id
                        select k;

            return View(Kepek.ToList());

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
                Alberlet alberlet = db.Alberletek.Find(id);

                
                if (alberlet == null)
                {
                    return HttpNotFound();
                }

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(HttpContext.Server.MapPath("~/Uploads/"), fileName);
                    file.SaveAs(path);
                    Kep kep = new Kep(path, fileName, alberlet);
                    alberlet.Kepek.Add(kep);
                    db.SaveChanges();
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
            Alberlet alberlet = db.Alberletek.Find(id);
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
            var user = UserManager.FindById(User.Identity.GetUserId());
            var ujalberlet = new Alberlet(model.Cim, model.Szobak_szama, model.Emelet, model.Mosdok_szama, model.Alapterulet, model.Ar, model.Berendezett, user);
            user.Hirdetesek.Add(ujalberlet);
            UserManager.Update(user);
            
            iterateUsers(ujalberlet);
           
            db.SaveChanges();
            return RedirectToAction("Index", new { Message = "Hirdetés feladva!" });
        }

        // GET: SajatAlberletek/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alberlet alberlet = db.Alberletek.Find(id);
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
                
                alberlet.Hirdeto = UserManager.FindById(User.Identity.GetUserId());
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
            Alberlet alberlet = db.Alberletek.Find(id);
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
            Alberlet alberlet = db.Alberletek.Find(id);
            db.Alberletek.Remove(alberlet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void iterateUsers(Alberlet albi)
        {
           
            foreach (var ite2 in UserManager.Users)
            {
                var filters = from f in db.Filters
                              where f.feliratkozo.Id == ite2.Id
                              select f;
                ite2.Filters = filters.ToList();
                foreach (Models.Filter item in ite2.Filters)
                {
                    if (
                        (item.Alapterulet==null||item.Alapterulet <= albi.Alapterulet)
                            && (item.Berendezett==null ||item.Berendezett.Equals(albi.Berendezett) || !item.Berendezett.HasValue)
                            && (item.Cim==null||item.Cim.Equals(albi.Cim))
                            && (item.Emelet==null||item.Emelet <= albi.Emelet)
                            && (item.MaxAr==null||item.MaxAr >= albi.Ar)
                            && (item.MinAr==null||albi.Ar <= item.MinAr)
                            && (item.Mosdok_szama==null||item.Mosdok_szama <= albi.Mosdok_szama)
                            && (item.Szobak_szama==null||item.Szobak_szama <= albi.Szobak_szama)
                            && ite2.Id !=albi.Hirdeto.Id) { }
                    sendMail(ite2, albi);

                }

            }


         }

        private void sendMail(ApplicationUser user, Alberlet albi)
        {
            MailMessage o = new MailMessage("temalabor@hotmail.com", user.Email, "Találat",
                "Megtaláltuk a neked megfelelő albérletet! \n "+
                "Az albérlet adatai: \n"+
                "Cím: " + albi.Cim+"\n"+
                "Szobák száma: " +albi.Szobak_szama+"\n"+
                "Emelet: " + albi.Emelet+ "\n"+
                "Mosdok száma: " +albi.Mosdok_szama+"\n"+
                "Alapterület: "+ albi.Alapterulet+"\n"+
                "Ár: "+albi.Ar);
            NetworkCredential netCred = new NetworkCredential("temalabor@hotmail.com", "valami123");
            SmtpClient smtpobj = new SmtpClient("smtp.live.com", 587);
            smtpobj.EnableSsl = true;
            smtpobj.Credentials = netCred;
            smtpobj.Send(o);
        }
    }
}
