using Domain.Entities;
using DunkerFinal.ViewModels.Explores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.Helpers.Extentions;

namespace DunkerFinal.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ExploreController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ExploreController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {
            List<Explore> Explores = await _context.Explores.ToListAsync();
            return View(Explores);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExploreCreateVM create)
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
            await _context.Explores.AddAsync(new Explore
            {
                Image = fileName,
                Name = create.Name,
                Brand = create.Brand
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Explore Explore = await _context.Explores.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (Explore == null) return NotFound();
            ExploreDetailVM model = new()
            {
                Id = Explore.Id,
                Image = Explore.Image,
                Name = Explore.Name,
                Brand = Explore.Brand

            };
            return View(model);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Explore slide = await _context.Explores.FirstOrDefaultAsync(s => s.Id == id);

            if (slide is null) return NotFound();

            slide.Image.DeleteFile(_env.WebRootPath, "assets", "img");

            _context.Explores.Remove(slide);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();
            Explore Explore = await _context.Explores.FirstOrDefaultAsync(m => m.Id == id);
            if (Explore == null) return NotFound();

            return View(new ExploreUpdateVM
            {
                Image = Explore.Image,
                Name = Explore.Name,
                Brand = Explore.Brand
            });


        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id, ExploreUpdateVM request)
        {
            Explore existExplore = await _context.Explores.FirstOrDefaultAsync(m => m.Id == id);
            if (!ModelState.IsValid)
            {
                request.Image = existExplore.Image;
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

                FileExtentions.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), existExplore.Image);

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "img", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                existExplore.Image = fileName;
            }

            if (existExplore == null) { return NotFound(); }


            existExplore.Name = request.Name;
            existExplore.Brand = request.Brand;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
