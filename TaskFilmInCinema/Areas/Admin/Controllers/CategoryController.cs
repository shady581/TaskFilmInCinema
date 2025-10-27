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
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // Create GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create POST
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Edit GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Cat_Id == id);
            if (category == null) return RedirectToAction("NotFoundPage", "Home");
            return View(category);
        }

        // Edit POST
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Delete
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Cat_Id == id);
            if (category == null) return RedirectToAction("NotFoundPage", "Home");

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}