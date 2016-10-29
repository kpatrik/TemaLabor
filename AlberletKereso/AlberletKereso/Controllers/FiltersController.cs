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
            return View(unitOfWork.FilterRepository.Get(filter => filter.feliratkozo == unitOfWork.UserManager.FindById(User.Identity.GetUserId())));
        }

        // GET: Filters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Filter filter = unitOfWork.FilterRepository.GetByID(id);
            if (filter == null)
            {
                return HttpNotFound();
            }
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
            if (ModelState.IsValid)
            {
                var user = unitOfWork.UserManager.FindById(User.Identity.GetUserId());
                var ujszuro = new Models.Filter(filter.Cim, filter.Szobak_szama, filter.Emelet, filter.Mosdok_szama, filter.Alapterulet, filter.MinAr, filter.MaxAr, filter.Berendezett, user);
                user.Filters.Add(ujszuro);
                unitOfWork.UserManager.Update(user);
                unitOfWork.Save();
            
                return RedirectToAction("Index");
            }

            return View(filter);
        }

        // GET: Filters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Filter filter = unitOfWork.FilterRepository.GetByID(id);
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
            if (ModelState.IsValid)
            {

                // db.Entry(filter).State = EntityState.Modified;
                unitOfWork.FilterRepository.Update(filter);
                unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(filter);
        }

        // GET: Filters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Filter filter = unitOfWork.FilterRepository.GetByID(id);
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
            Models.Filter filter = unitOfWork.FilterRepository.GetByID(id);
            unitOfWork.FilterRepository.Delete(filter);
            unitOfWork.Save();
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
