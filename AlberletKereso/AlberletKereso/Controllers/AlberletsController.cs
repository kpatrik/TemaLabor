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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Alberlets
        public ActionResult Index(String search, int? minSzoba, int? maxSzoba, int? minTerulet, int? maxTerulet, int? minAr, int? maxAr)
        {
            if (maxSzoba ==null) maxSzoba = int.MaxValue;
            if (maxTerulet == null) maxTerulet = int.MaxValue;
            if (maxAr == null) maxAr = int.MaxValue;
            if (minSzoba == null) minSzoba = 0;
            if (minTerulet == null) minTerulet = 0;
            if (minAr == null) minAr = 0;

            var alberletek = from a in db.Alberletek
                             where a.Alapterulet >= minTerulet
                             where a.Alapterulet <= maxTerulet
                             where a.Szobak_szama >= minSzoba
                             where a.Szobak_szama <= maxSzoba
                             where a.Ar >= minAr
                             where a.Ar <= maxAr
                             select a;
            if (!String.IsNullOrEmpty(search))
            {
                alberletek = alberletek.Where(c => c.Cim.Contains(search));
            }

            return View(alberletek.ToList());
        }

        // GET: Alberlets/Details/5
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



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
