using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlberletKereso.Models;

namespace AlberletKereso.Controllers
{
    public class AlberletsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Alberlets
        public ActionResult Index(String search, int? minSzoba, int? maxSzoba, int? minTerulet, int? maxTerulet, int? minAr, int? maxAr)
        {
            Services.AlberletsService alberletsService = new Services.AlberletsService(unitOfWork);

            minSzoba = alberletsService.DefineMinSzoba(minSzoba);
            maxSzoba = alberletsService.DefineMaxSzoba(maxSzoba);
            minTerulet = alberletsService.DefineMinTerulet(minTerulet);
            maxTerulet = alberletsService.DefineMaxTerulet(maxTerulet);
            minAr = alberletsService.DefineMinAr(minAr);
            maxAr = alberletsService.DefineMaxAr(maxAr);

            var alberletek = alberletsService.GetAlberlets(search, minSzoba, maxSzoba, minTerulet, maxTerulet, minAr, maxAr);

            return View(alberletek.ToList());
        }



        // GET: Alberlets/Details/5
        public ActionResult Details(int? id)
        {
            Services.AlberletsService alberletsService = new Services.AlberletsService(unitOfWork);

            if (alberletsService.CheckID(id)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Alberlet alberlet = alberletsService.GetAlberlet(id);

            if (alberletsService.CheckAlberlet(alberlet)) return HttpNotFound();

            alberlet.Kepek = alberletsService.GetKepek((int)id).ToList();

            return View(alberlet);
        }



        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}