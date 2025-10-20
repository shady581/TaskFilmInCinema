using Microsoft.AspNetCore.Mvc;
using TaskFilmInCinema;
using TaskFilmInCinema.Models;

[Area("Admin")]
public class ActorController : Controller
{
    private readonly ApplicationDbContext _context;

    public ActorController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var actors = _context.Actors.ToList();
        return View(actors);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Actor actor, IFormFile ImgFile)
    {
        if (ImgFile is not null && ImgFile.Length > 0)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ActorsImg");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                ImgFile.CopyTo(stream);
            }

            actor.Img = "/ActorsImg/" + fileName;
        }

        _context.Actors.Add(actor);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var actor = _context.Actors.Find(id);
        if (actor == null) return NotFound();
        return View(actor);
    }

    // Edit POST
    [HttpPost]
    public IActionResult Edit(Actor actor, IFormFile ImgFile)
    {
        if (ImgFile is not null && ImgFile.Length > 0)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ActorsImg");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                ImgFile.CopyTo(stream);
            }

            actor.Img = "/ActorsImg/" + fileName;
        }

        _context.Actors.Update(actor);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var actor = _context.Actors.FirstOrDefault(a => a.Act_Id == id);

        if (actor is null)
            return RedirectToAction("NotFoundPage", "Home"); // لو عندك صفحة NotFound

        // Optional: delete the image file from wwwroot
        if (!string.IsNullOrEmpty(actor.Img))
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ActorsImg", Path.GetFileName(actor.Img));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        _context.Actors.Remove(actor);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}