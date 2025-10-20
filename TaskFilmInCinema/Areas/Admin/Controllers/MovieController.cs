using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFilmInCinema.Models;

namespace TaskFilmInCinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index
        public IActionResult Index()
        {
            var movies = _context.Movies
                .Include(m => m.Cinema)
                .Include(m => m.Category)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .ToList();

            return View(movies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Cinemas = _context.Cinemas.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Actors = _context.Actors.ToList();
            return View();
        }

        // Create POST
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile MainImgFile, IFormFile SubImgFile, int[] SelectedActors)
        {
            // رفع الصور (Main & Sub)
            if (MainImgFile != null && MainImgFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\MovieImages");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(MainImgFile.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = System.IO.File.Create(filePath);
                MainImgFile.CopyTo(stream);
                movie.MainImg = fileName;
            }

            if (SubImgFile != null && SubImgFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\MovieImages");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(SubImgFile.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = System.IO.File.Create(filePath);
                SubImgFile.CopyTo(stream);
                movie.SubImg = fileName;
            }

            // تأكد من CinemaId و CategoryId موجودين
            if (!_context.Cinemas.Any(c => c.Cin_Id == movie.CinemaId) ||
                !_context.Categories.Any(c => c.Cat_Id == movie.CategoryId))
            {
                ModelState.AddModelError("", "Invalid Cinema or Category.");
                ViewBag.Cinemas = _context.Cinemas.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Actors = _context.Actors.ToList();
                return View(movie);
            }

            // أضف الفيلم أولاً
            _context.Movies.Add(movie);
            _context.SaveChanges();  // لازم بعد SaveChanges عشان MovieId يتكون

            // أضف MovieActors
            if (SelectedActors != null)
            {
                foreach (var actId in SelectedActors)
                {
                    _context.MovieActors.Add(new MovieActor
                    {
                        Mov_Id = movie.Mov_Id,
                        Act_Id = actId
                    });
                }
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }


      [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies
                .Include(m => m.MovieActors)
                .FirstOrDefault(m => m.Mov_Id == id);

            if (movie == null) return RedirectToAction("Index");

            ViewBag.Cinemas = _context.Cinemas.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Actors = _context.Actors.ToList();

            return View(movie);
        }

        // Edit POST
        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile MainImgFile, IFormFile SubImgFile, int[] SelectedActors)
        {
            var existingMovie = _context.Movies
                .Include(m => m.MovieActors)
                .FirstOrDefault(m => m.Mov_Id == movie.Mov_Id);

            if (existingMovie == null) return RedirectToAction("Index");

            existingMovie.Name = movie.Name;
            existingMovie.Description = movie.Description;
            existingMovie.Price = movie.Price;
            existingMovie.Date = movie.Date;
            existingMovie.Status = movie.Status;
            existingMovie.CinemaId = movie.CinemaId;
            existingMovie.CategoryId = movie.CategoryId;

            // Update images
            if (MainImgFile != null && MainImgFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\MovieImages");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(MainImgFile.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = System.IO.File.Create(filePath);
                MainImgFile.CopyTo(stream);
                existingMovie.MainImg = fileName;
            }

            if (SubImgFile != null && SubImgFile.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\MovieImages");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(SubImgFile.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = System.IO.File.Create(filePath);
                SubImgFile.CopyTo(stream);
                existingMovie.SubImg = fileName;
            }

            // Update actors
            existingMovie.MovieActors.Clear();
            if (SelectedActors != null)
            {
                foreach (var actId in SelectedActors)
                {
                    existingMovie.MovieActors.Add(new MovieActor { Act_Id = actId, Mov_Id = existingMovie.Mov_Id });
                }
            }

            _context.Movies.Update(existingMovie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Delete
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Mov_Id == id);
            if (movie != null)
            {
                // Delete images
                if (!string.IsNullOrEmpty(movie.MainImg))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\MovieImages", movie.MainImg);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }
                if (!string.IsNullOrEmpty(movie.SubImg))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\MovieImages", movie.SubImg);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }

                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
