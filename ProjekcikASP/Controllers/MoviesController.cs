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
    public class MoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Movies
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var movies = db.Movies.Include(m => m.Category).Include(m => m.User);
            return View(movies.ToList());
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movies movies = db.Movies.Find(id);
            if (movies == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            var ra = from s in db.Rating
                        select s;
            var rating = ra.Where(s => (s.MovieId == id) && (s.UserId == userId));
            var ratings = ra.Where(s => s.MovieId == id);
            var sum = 0;
            foreach(var rat in ratings.ToList())
            {
                sum += rat.RatingValue;
            }
            ViewBag.avg = (sum == 0)? 0 : (1.0*sum / ratings.Count());
            ViewBag.allRatings= ratings.Count(); 
            ViewBag.rating = rating.ToList();
            return View(movies);
        }
        //CreateComment
        [Authorize(Roles = "User, Admin")]
        
        public ActionResult CreateComment([Bind(Include = "Id,MovieId,UserId,Comment")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comments);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = comments.MovieId});
            }

            ViewBag.MovieId = new SelectList(db.Movies, "Id", "MovieName", comments.MovieId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", comments.UserId);
            return View(comments);
        }
        [Authorize(Roles = "User, Admin")]
        public ActionResult RentSuccessful(int id)
        {
            RentHistory rent = db.RentHistory.Find(id);
            ViewBag.Movie = db.Movies.Find(rent.MovieId);
            return View(rent);
        }
        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Category");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Movies/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryId,UserId,MovieName,MovieDescription,MovieDirector,MovieRealiseDate,MovieRentPrice,MovieOriginCountry,MoviePhoto,MovieTrailer")] Movies movies)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movies);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Category", movies.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", movies.UserId);
            return View(movies);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movies movies = db.Movies.Find(id);
            if (movies == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Category", movies.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", movies.UserId);
            return View(movies);
        }

        // POST: Movies/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,UserId,MovieName,MovieDescription,MovieDirector,MovieRealiseDate,MovieRentPrice,MovieOriginCountry,MoviePhoto,MovieTrailer")] Movies movies)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Category, "Id", "Category", movies.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", movies.UserId);
            return View(movies);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movies movies = db.Movies.Find(id);
            if (movies == null)
            {
                return HttpNotFound();
            }
            return View(movies);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Movies movies = db.Movies.Find(id);
            db.Movies.Remove(movies);
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
