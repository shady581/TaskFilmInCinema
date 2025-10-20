using Microsoft.AspNetCore.Mvc;
using TaskFilmInCinema.Models;

namespace TaskFilmInCinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category/Index
        public IActionResult Index()
        {
            var cinemas = _context.Cinemas.ToList();
            return View(cinemas);
        }

        // Create GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create POST
        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile ImgFile)
        {
            if (ImgFile != null && ImgFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CinemaImage", fileName);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CinemaImage")))
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CinemaImage"));

                using (var stream = System.IO.File.Create(filePath))
                {
                    ImgFile.CopyTo(stream);
                }

                cinema.Img = fileName;
            }

            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Edit GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cinema = _context.Cinemas.FirstOrDefault(c => c.Cin_Id == id);
            if (cinema == null) return RedirectToAction("NotFoundPage", "Home");
            return View(cinema);
        }

        // Edit POST
        [HttpPost]
        public IActionResult Edit(Cinema cinema)
        {
            _context.Cinemas.Update(cinema);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Delete
        public IActionResult Delete(int id)
        {
            var cinema = _context.Cinemas.FirstOrDefault(c => c.Cin_Id == id);
            if (cinema == null) return RedirectToAction("NotFoundPage", "Home");

            _context.Cinemas.Remove(cinema);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}