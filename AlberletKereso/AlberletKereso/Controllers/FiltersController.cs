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

namespace AlberletKereso.Controllers
{
    public class FiltersController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public FiltersController()
        {

        }

        // GET: Filters
        public ActionResult Index()
        {
            var userId = unitOfWork.UserManager.FindById(User.Identity.GetUserId()).Id;
            var filterek = unitOfWork.FilterRepository.Get(f => f.feliratkozo.Id == userId);
            return View(filterek);
        }

        // GET: Filters/Details/5
        public ActionResult Details(int? id)
        {
            Services.FiltersService filtersService = new Services.FiltersService(unitOfWork);

            if (filtersService.CheckID(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Models.Filter filter = filtersService.GetFilter((int)id);

            if (filter == null) return HttpNotFound();

            return View(filter);
        }

        // GET: Filters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Filters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilterId,Cim,Szobak_szama,Emelet,Mosdok_szama,Alapterulet,MinAr,MaxAr,Berendezett")] Models.Filter filter)
        {
            Services.FiltersService filtersService = new Services.FiltersService(unitOfWork);

            if (ModelState.IsValid)
            {
                filtersService.CreateFilter(User.Identity.GetUserId(), filter);
            
                return RedirectToAction("Index");
            }

            return View(filter);
        }

        // GET: Filters/Edit/5
        public ActionResult Edit(int? id)
        {
            Services.FiltersService filtersService = new Services.FiltersService(unitOfWork);

            if (filtersService.CheckID(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Models.Filter filter = filtersService.GetFilter((int)id);

            if (filter == null)
            {
                return HttpNotFound();
            }
            return View(filter);
        }

        // POST: Filters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FilterId,Cim,Szobak_szama,Emelet,Mosdok_szama,Alapterulet,MinAr,MaxAr,Berendezett")] Models.Filter filter)
        {
            Services.FiltersService filtersService = new Services.FiltersService(unitOfWork);

            if (ModelState.IsValid)
            {
                filtersService.UpdateFilter(filter);

                return RedirectToAction("Index");
            }
            return View(filter);
        }

        // GET: Filters/Delete/5
        public ActionResult Delete(int? id)
        {
            Services.FiltersService filtersService = new Services.FiltersService(unitOfWork);

            if (filtersService.CheckID(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Filter filter = filtersService.GetFilter((int)id);

            if (filter == null)
            {
                return HttpNotFound();
            }
            return View(filter);
        }

        // POST: Filters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Services.FiltersService filtersService = new Services.FiltersService(unitOfWork);

            Models.Filter filter = filtersService.GetFilter(id);
            filtersService.DeleteFilter(filter);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
