using MoviesCollection.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MoviesCollection.Controllers
{
    public class HomeController : Controller
    {
        private MVCMoviesEntities db = new MVCMoviesEntities(); 
        // GET: Home
        public ActionResult Index(string movieGenre, string searchString)

        {
            //genre search making list for drop down

            var GenreList = new List<string>();
            // LINQ QUERIES TO DATABASE
            var GenreQuery = from d in db.Movies
                             orderby d.Genre
                             select d.Genre;

            // ADD UNIQUE VALUES TO GENRE LIST
            GenreList.AddRange(GenreQuery.Distinct());

            //put into viewbag foer use in VIEW
            ViewBag.movieGenre = new SelectList(GenreList);

            // define key as new select list

            //get all movies from genre search
            var movies = from m in db.Movies
                         select m;

            //if a genre serach has been done, use LINQ to get a reduced list of Movies
            //to only the selected genre.

            if (!String.IsNullOrEmpty(movieGenre))
            {

                movies = movies.Where(x => x.Genre == movieGenre);
            }

            //if a title is searched is done use LINQ to reduce list of movies
            // matching searchString

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));

            }

            return View(movies);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(movie);
        }

        public ActionResult Details(int? id)

        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
           

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        public ActionResult Edit(int? id)


        {

            if(id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = db.Movies.Find(id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);


        }
        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                //updates local copy of record and then save changes in simple terms...
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");

            }

            return View(movie);
        }

        public ActionResult Delete(int? id)
        {

            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = db.Movies.Find(id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);

        }

        [HttpPost ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}