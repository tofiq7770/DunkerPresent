using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.Helpers.Extentions;
using Service.ViewModels.Elementor;

namespace DunkerFinal.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ElementorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ElementorController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {
            List<Elementor> Elementors = await _context.Elementors.ToListAsync();
            return View(Elementors);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ElementorCreateVM create)
        {
            if (!ModelState.IsValid) return View();

            if (!create.Image.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Image", "File must be Image Format");
                return View();
            }
            if (!create.Image.CheckFileSize(200))
            {

                ModelState.AddModelError("Image", "Max File Capacity mut be 300KB");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "-" + create.Image.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets", "img", fileName);
            await create.Image.SaveFileToLocalAsync(path);
            await _context.Elementors.AddAsync(new Elementor
            {
                Image = fileName
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Elementor Elementor = await _context.Elementors.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (Elementor == null) return NotFound();
            ElementorDetailVM model = new()
            {
                Id = Elementor.Id,
                Image = Elementor.Image

            };
            return View(model);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Elementor slide = await _context.Elementors.FirstOrDefaultAsync(s => s.Id == id);

            if (slide is null) return NotFound();

            slide.Image.DeleteFile(_env.WebRootPath, "assets", "img");

            _context.Elementors.Remove(slide);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();
            Elementor Elementor = await _context.Elementors.FirstOrDefaultAsync(m => m.Id == id);
            if (Elementor == null) return NotFound();

            return View(new ElementorUpdateVM
            {
                Image = Elementor.Image
            });

        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id, ElementorUpdateVM request)
        {
            Elementor existElementor = await _context.Elementors.FirstOrDefaultAsync(m => m.Id == id);
            if (!ModelState.IsValid)
            {
                request.Image = existElementor.Image;
                return View(request);
            }

            if (request.Photo != null)
            {
                if (!request.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be 200kb");
                    return View(request.Photo);
                }

                if (!request.Photo.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Photo", "Image format is wrong");
                    return View(request.Photo);
                }

                FileExtentions.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), existElementor.Image);

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "img", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                existElementor.Image = fileName;
            }

            if (existElementor == null) { return NotFound(); }


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
