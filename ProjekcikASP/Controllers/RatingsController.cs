using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjekcikASP.Models;
using WebApplication2.Models;
using Microsoft.AspNet.Identity;
namespace ProjekcikASP.Controllers
{
    public class RatingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ratings
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var rating = db.Rating.Include(r => r.Movie).Include(r => r.User);
            return View(rating.ToList());
        }

        // GET: Ratings/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Rating.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: Ratings/Create
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            ViewBag.MovieId = new SelectList(db.Movies, "Id", "MovieName");
            ViewBag.UserId = new SelectList(db.User, "Id", "UserId");
            return View();
        }

        // POST: Ratings/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Admin")]
        public ActionResult Create([Bind(Include = "Id,RatingValue,MovieId,UserId")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                
                db.Rating.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MovieId = new SelectList(db.Movies, "Id", "MovieName", rating.MovieId);
            ViewBag.UserId = new SelectList(db.User, "Id", "UserId", rating.UserId);
            return View(rating);
        }

        // GET: Ratings/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Rating.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieId = new SelectList(db.Movies, "Id", "MovieName", rating.MovieId);
            ViewBag.UserId = new SelectList(db.User, "Id", "UserId", rating.UserId);
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,RatingValue,MovieId,UserId")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MovieId = new SelectList(db.Movies, "Id", "MovieName", rating.MovieId);
            ViewBag.UserId = new SelectList(db.User, "Id", "UserId", rating.UserId);
            return View(rating);
        }

        // GET: Ratings/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Rating.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Rating.Find(id);
            db.Rating.Remove(rating);
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

        [Authorize(Roles = "Admin,User")]
        public ActionResult AddRatingMovie(int RatingValue, int MovieId, string UserId)
        {
            Rating rating = new Rating();
            rating.RatingValue = RatingValue;
            rating.MovieId = MovieId;
            rating.UserId = UserId;
            db.Rating.Add(rating);
            db.SaveChanges();
            return Json(rating);
        }
    }
}
