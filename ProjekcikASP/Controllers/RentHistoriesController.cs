using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjekcikASP.Models;

namespace ProjekcikASP.Controllers
{
    public class RentHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RentHistories
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var rentHistory = db.RentHistory.Include(r => r.Movie).Include(r => r.User);
            return View(rentHistory.ToList());
        }

        // GET: RentHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentHistory rentHistory = db.RentHistory.Find(id);
            if (rentHistory == null)
            {
                return HttpNotFound();
            }
            return View(rentHistory);
        }

        // GET: RentHistories/Create
        public ActionResult Create()
        {
            ViewBag.MovieId = new SelectList(db.Movies, "Id", "UserId");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: RentHistories/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MovieId,UserId,RentDate,RentTime")] RentHistory rentHistory)
        {
            if (ModelState.IsValid)
            {
                DateTime localDate = DateTime.Now;
                rentHistory.RentDate = localDate;
                db.RentHistory.Add(rentHistory);
                db.SaveChanges();
                return RedirectToAction("RentSuccessful", "Movies", new { id = rentHistory.Id });
            }

            ViewBag.MovieId = new SelectList(db.Movies, "Id", "UserId", rentHistory.MovieId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", rentHistory.UserId);
            return View(rentHistory);
        }

        // GET: RentHistories/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentHistory rentHistory = db.RentHistory.Find(id);
            if (rentHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieId = new SelectList(db.Movies, "Id", "UserId", rentHistory.MovieId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", rentHistory.UserId);
            return View(rentHistory);
        }

        // POST: RentHistories/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,MovieId,UserId,RentDate,RentTime")] RentHistory rentHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rentHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MovieId = new SelectList(db.Movies, "Id", "UserId", rentHistory.MovieId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", rentHistory.UserId);
            return View(rentHistory);
        }

        // GET: RentHistories/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentHistory rentHistory = db.RentHistory.Find(id);
            if (rentHistory == null)
            {
                return HttpNotFound();
            }
            return View(rentHistory);
        }

        // POST: RentHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            RentHistory rentHistory = db.RentHistory.Find(id);
            db.RentHistory.Remove(rentHistory);
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
