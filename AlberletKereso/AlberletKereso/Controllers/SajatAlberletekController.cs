using System.Net;
using System.Web.Mvc;
using AlberletKereso.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace AlberletKereso.Controllers
{

    public class SajatAlberletekController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: SajatAlberletek
        public ActionResult Index()
        {
            Services.SajatAlberletekService sajatAlberletekService = new Services.SajatAlberletekService(unitOfWork);

            var alberletek = sajatAlberletekService.GetAlberletek(User.Identity.GetUserId());

            return View(alberletek);
        }

       
        public ActionResult KepHozzaadas()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KepHozzaadas(int? id)
        {
            Services.SajatAlberletekService sajatAlberletekService = new Services.SajatAlberletekService(unitOfWork);

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
               
                if (sajatAlberletekService.CheckID(id))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Alberlet alberlet = sajatAlberletekService.GetAlberlet((int)id);

               
                if (alberlet == null)
                {
                    return HttpNotFound();
                }

                if (!sajatAlberletekService.CheckExtension(file))
                    //Ide kéne valami szebb lekezelés, vagy értesítés
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                
                if (file != null && file.ContentLength > 0 )
                {
                    sajatAlberletekService.UploadImage(file, alberlet, HttpContext);
                }

            }

            return RedirectToAction("Index");
        }

        // GET: SajatAlberletek/Details/5
        public ActionResult Details(int? id)
        {
            Services.SajatAlberletekService sajatAlberletekService = new Services.SajatAlberletekService(unitOfWork);

            if (sajatAlberletekService.CheckID(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alberlet alberlet = sajatAlberletekService.GetAlberlet((int)id);

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
            Services.SajatAlberletekService sajatAlberletekService = new Services.SajatAlberletekService(unitOfWork);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            sajatAlberletekService.UploadAlberlet(model, User.Identity.GetUserId());

            return RedirectToAction("Index", new { Message = "Hirdetés feladva!" });
        }

        // GET: SajatAlberletek/Edit/5
        public ActionResult Edit(int? id)
        {
            Services.SajatAlberletekService sajatAlberletekService = new Services.SajatAlberletekService(unitOfWork);

            if (sajatAlberletekService.CheckID(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alberlet alberlet = sajatAlberletekService.GetAlberlet((int)id);

            if (alberlet == null)
            {
                return HttpNotFound();
            }

            return View(alberlet);
        }



        // POST: SajatAlberletek/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlberletId,Cim,Szobak_szama,Emelet,Mosdok_szama,Alapterulet,Ar,Berendezett")] Alberlet alberlet)
        {
            Services.SajatAlberletekService sajatAlberletekService = new Services.SajatAlberletekService(unitOfWork);

            if (ModelState.IsValid)
            {
                alberlet.Hirdeto = unitOfWork.UserManager.FindById(User.Identity.GetUserId());
                sajatAlberletekService.IterateUsers(alberlet);

                return RedirectToAction("Index");
            }
            return View(alberlet);
        }

        // GET: SajatAlberletek/Delete/5
        public ActionResult Delete(int? id)
        {
            Services.SajatAlberletekService sajatAlberletekService = new Services.SajatAlberletekService(unitOfWork);

            if (sajatAlberletekService.CheckID(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Alberlet alberlet = sajatAlberletekService.GetAlberlet((int)id);

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
            Services.SajatAlberletekService sajatAlberletekService = new Services.SajatAlberletekService(unitOfWork);
            sajatAlberletekService.DeleteAlberlet(id);

            return RedirectToAction("Index");
        }
    }
}
