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

namespace AlberletKereso.Controllers
{
    public class SajatAlberletekController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public SajatAlberletekController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        protected UserManager<ApplicationUser> UserManager { get; set; }


        protected ApplicationDbContext ApplicationDbContext { get; set; }
    

        // GET: SajatAlberletek
        public ActionResult Index()
        {

            var userID = UserManager.FindById(User.Identity.GetUserId()).Id;
            var alberletek = from a in db.Alberletek
                             where a.Hirdeto.Id == userID
                             select a;

            return View(alberletek.ToList());
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
        public ActionResult Create([Bind(Include = "AlberletId,Cim,Szobak_szama,Emelet,Mosdok_szama,Alapterulet,Ar,Berendezett")] Alberlet alberlet)
        {
            if (ModelState.IsValid)
            {
                db.Alberletek.Add(alberlet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alberlet);
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
                db.Entry(alberlet).State = EntityState.Modified;
                db.SaveChanges();
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
