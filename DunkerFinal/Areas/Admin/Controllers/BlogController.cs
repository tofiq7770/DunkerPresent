using Domain.Entities;
using DunkerFinal.ViewModels.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.Helpers.Extentions;

namespace DunkerFinal.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {
            List<Blog> Blogs = await _context.Blogs.ToListAsync();
            return View(Blogs);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM create)
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
            await _context.Blogs.AddAsync(new Blog
            {
                Image = fileName,
                Title = create.Title,
                Description = create.Description
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Blog blog = await _context.Blogs.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (blog == null) return NotFound();
            BlogDetailVM model = new()
            {
                Id = blog.Id,
                Image = blog.Image,
                Title = blog.Title,
                Description = blog.Description,

            };
            return View(model);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Blog slide = await _context.Blogs.FirstOrDefaultAsync(s => s.Id == id);

            if (slide is null) return NotFound();

            slide.Image.DeleteFile(_env.WebRootPath, "assets", "img");

            _context.Blogs.Remove(slide);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null) return NotFound();

            return View(new BlogUpdateVM
            {
                Image = blog.Image,
                Title = blog.Title,
                Description = blog.Description
            });


        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id, BlogUpdateVM request)
        {
            Blog existBlog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
            if (!ModelState.IsValid)
            {
                request.Image = existBlog.Image;
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

                FileExtentions.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), existBlog.Image);

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "img", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                existBlog.Image = fileName;
            }

            if (existBlog == null) { return NotFound(); }


            existBlog.Title = request.Title;
            existBlog.Description = request.Description;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
