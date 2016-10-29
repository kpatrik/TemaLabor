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
        public ActionResult Index(String cim, int? minSzoba, int? maxSzoba, int? minTerulet, int? maxTerulet, int? minAr, int? maxAr)
        {
            if (maxSzoba ==null) maxSzoba = int.MaxValue;
            if (maxTerulet == null) maxTerulet = int.MaxValue;
            if (maxAr == null) maxAr = int.MaxValue;
            if (minSzoba == null) minSzoba = 0;
            if (minTerulet == null) minTerulet = 0;
            if (minAr == null) minAr = 0;

            var alberletek = unitOfWork.AlberletRepository.Get();

            if (!String.IsNullOrEmpty(cim))
            {
                alberletek = unitOfWork.AlberletRepository.Get(filter: f => f.Alapterulet >= minTerulet & f.Alapterulet <= maxTerulet &
                                                                            f.Szobak_szama >= minSzoba & f.Szobak_szama <= maxSzoba &
                                                                            f.Alapterulet >= minTerulet & f.Alapterulet <= maxTerulet & f.Cim.Contains("cim")
                                                                            );
            }
            else
            {
                alberletek = unitOfWork.AlberletRepository.Get(filter: f => f.Alapterulet >= minTerulet & f.Alapterulet <= maxTerulet &
                                                                            f.Szobak_szama >= minSzoba & f.Szobak_szama <= maxSzoba &
                                                                            f.Alapterulet >= minTerulet & f.Alapterulet <= maxTerulet);
            } 
            return View(alberletek.ToList());
        }


        public ActionResult KepNezegeto(int? id)
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

            var Kepek = unitOfWork.KepRepository.Get(filter: f => f.Alberlet.AlberletId == id);

            return View(Kepek.ToList());
        }
        

        // GET: Alberlets/Details/5
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



        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
