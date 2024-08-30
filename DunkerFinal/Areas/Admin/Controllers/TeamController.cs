using Domain.Entities;
using DunkerFinal.ViewModels.Teams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.Helpers.Extentions;

namespace DunkerFinal.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {
            List<Team> Teams = await _context.Teams.ToListAsync();
            return View(Teams);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamCreateVM create)
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
            await _context.Teams.AddAsync(new Team
            {
                Image = fileName,
                Name = create.Name,
                Position = create.Position
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Team Team = await _context.Teams.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (Team == null) return NotFound();
            TeamDetailVM model = new()
            {
                Id = Team.Id,
                Image = Team.Image,
                Name = Team.Name,
                Position = Team.Position

            };
            return View(model);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Team slide = await _context.Teams.FirstOrDefaultAsync(s => s.Id == id);

            if (slide is null) return NotFound();

            slide.Image.DeleteFile(_env.WebRootPath, "assets", "img");

            _context.Teams.Remove(slide);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();
            Team Team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == id);
            if (Team == null) return NotFound();

            return View(new TeamUpdateVM
            {
                Image = Team.Image,
                Name = Team.Name,
                Position = Team.Position
            });


        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id, TeamUpdateVM request)
        {
            Team existTeam = await _context.Teams.FirstOrDefaultAsync(m => m.Id == id);
            if (!ModelState.IsValid)
            {
                request.Image = existTeam.Image;
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

                FileExtentions.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), existTeam.Image);

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "img", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                existTeam.Image = fileName;
            }

            if (existTeam == null) { return NotFound(); }


            existTeam.Name = request.Name;
            existTeam.Position = request.Position;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
