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
        private ApplicationDbContext db = new ApplicationDbContext();
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected Microsoft.AspNet.Identity.UserManager<ApplicationUser> UserManager { get; set; }
        public FiltersController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET: Filters
        public ActionResult Index()
        {
            var userID = UserManager.FindById(User.Identity.GetUserId()).Id;
            var filters = from f in db.Filters
                          where f.feliratkozo.Id == userID
                          select f;
            return View(filters.ToList());
        }

        // GET: Filters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Filter filter = db.Filters.Find(id);
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
                var user = UserManager.FindById(User.Identity.GetUserId());
                var ujszuro = new Models.Filter(filter.Cim, filter.Szobak_szama, filter.Emelet, filter.Mosdok_szama, filter.Alapterulet, filter.MinAr, filter.MaxAr, filter.Berendezett, user);
                user.Filters.Add(ujszuro);
                UserManager.Update(user);
                using (var context = new ApplicationDbContext())
                {
                    context.SaveChanges();
                }
               
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
            Models.Filter filter = db.Filters.Find(id);
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
                db.Entry(filter).State = EntityState.Modified;
                db.SaveChanges();
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
            Models.Filter filter = db.Filters.Find(id);
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
            Models.Filter filter = db.Filters.Find(id);
            db.Filters.Remove(filter);
            db.SaveChanges();
            return RedirectToAction("Index");
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
