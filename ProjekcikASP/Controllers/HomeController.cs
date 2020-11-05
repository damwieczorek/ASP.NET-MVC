using ProjekcikASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjekcikASP.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //public ActionResult Index()
        //{
          
      
        //    ViewData["movies"] = db.Movies.ToList();
         
        //    return View(db.Movies.ToList());
        //}
        public ActionResult Index(string sortOrder, string categoryName, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(categoryName) ? "Category" : "";
            var movie = from s in db.Movies
                           select s;
            if (!String.IsNullOrEmpty(categoryName))
            {
                movie = movie.Where(s => s.Category.Category.Contains(categoryName));
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                movie = movie.Where(s => s.MovieName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Name":
                    movie = movie.OrderByDescending(s => s.MovieName);
                    break;
                case "Category":
                   
                    movie = movie.OrderByDescending(s => s.Category);
                    break;
                case "Date":
                    movie = movie.OrderBy(s => s.MovieRealiseDate);
                    break;
                case "Date_desc":
                    movie = movie.OrderByDescending(s => s.MovieRealiseDate);
                    break;
                case "Price":
                    movie = movie.OrderBy(s => s.MovieRentPrice);
                    break;
                case "Price_desc":
                    movie = movie.OrderByDescending(s => s.MovieRentPrice);
                    break;
                case "Rating":
                    movie = movie.OrderByDescending(s => s.Rating);
                    break;
                default:
                    movie = movie.OrderBy(s => s.MovieName);
                    break;
            }
            ViewBag.Caty = db.Category.ToList();
            return View(movie.ToList());
        }
        public ActionResult About()
        {
            ViewBag.Message = "Wykonał Damian Wieczorek";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakt";

            return View();
        }
    }
}