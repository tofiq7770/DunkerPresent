
using AutoMapper;
using Domain.Entities;
using DunkerFinal.Areas.Admin.Class;
using DunkerFinal.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.Helpers.Extentions;
using Service.Services.Interfaces;
using Service.ViewModels.Product;
using Service.ViewModels.ProductColorVms;
using Service.ViewModels.ProductImageVMs;
using Service.ViewModels.ProductTagVMs;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductService _service;
        private readonly IProductImageService _imageService;
        private readonly IColorService _colorService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IBrandService _brandService;
        private readonly IProductColorService _productColorService;
        private readonly IProductTagService _productTagService;
        private readonly IWebHostEnvironment _environment;
        private readonly string _imagePath;

        public ProductController(IProductService service,
            IWebHostEnvironment environment,
            IProductColorService productColorService,
            IColorService colorService,
            ICategoryService categoryService,
            IBrandService brandService,
            ITagService tagService,
            AppDbContext context,
            IProductTagService productTagService, IProductImageService imageService, IMapper mapper)
        {
            _productTagService = productTagService;
            _service = service;
            _environment = environment;
            _imagePath = Path.Combine(_environment.WebRootPath, "assets", "images");
            _productColorService = productColorService;
            _colorService = colorService;
            _categoryService = categoryService;
            _brandService = brandService;
            _context = context;
            _tagService = tagService;
            _imageService = imageService;
            _mapper = mapper;
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 3)
        {
            var products = _service.GetAllAsync();

            var paginatedProducts = PagedList<Product>.Create(await products, pageNumber, pageSize);

            var viewModel = new ProductsListVM
            {
                Products = paginatedProducts,
                PageNumber = pageNumber,
                TotalPages = paginatedProducts.TotalPages
            };

            return View(viewModel);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.Colors = await _colorService.GetAllSelectListAsync();
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            ViewBag.Colors = await _colorService.GetAllSelectListAsync();
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            if (!ModelState.IsValid) return View(request);

            if (await _service.AnyAsync(request.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", $"{request.Name} already exist!");
                return View(request);
            }

            foreach (var item in request.Images)
            {
                if (!item.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Images", "File must be Image Format");
                    return View(request);
                }

                if (!item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Images", "Max File capacity must be 200KB");
                    return View(request);
                }
            }

            int productId = await _service.CreateAsync(request);

            for (int i = 0; i < request.Images.Length; i++)
            {
                bool isMain = false;

                if (i == 0) isMain = true;

                string fileName = Guid.NewGuid().ToString() + "-" + request.Images[i].FileName;
                string path = Path.Combine(_environment.WebRootPath, "assets/img", fileName);
                await request.Images[i].SaveFileToLocalAsync(path);

                await _imageService.CreateAsync(new ProductImageCreateVM() { Image = fileName, ProductId = productId, IsMain = isMain });
            }

            foreach (var colorId in request.ColorIds)
            {
                await _productColorService.CreateAsync(new ProductColorCreateVM() { ProductId = productId, ColorId = colorId });
            }

            foreach (var tagId in request.TagIds)
            {
                await _productTagService.CreateAsync(new ProductTagCreateVM() { ProductId = productId, TagId = tagId });
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.Colors = await _colorService.GetAllSelectListAsync();
            ViewBag.Colors = await _colorService.GetAllSelectListAsync(await _productColorService.GetAllColorIdsByProductId(id));
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            var result = await _service.GetByIdAsync(id);

            var model = _mapper.Map<ProductUpdateVM>(result);
            model.Colors = await _productColorService.GetAllByProductIdAsync(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductUpdateVM request)
        {
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.Colors = await _colorService.GetAllSelectListAsync(await _productColorService.GetAllColorIdsByProductId(id));
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            if (!ModelState.IsValid) return View(request);


            var productCount = await _context.Products.Where(p => p.Name.ToLower() == request.Name.Trim().ToLower() && p.Id != id).CountAsync();

            if (productCount > 0)
            {
                ModelState.AddModelError("Name", $"{request.Name} already exists!");
                return View(request);
            }


            if (request.Images != null)
            {
                foreach (var item in request.Images)
                {
                    if (!item.CheckFileFormat("image/"))
                    {
                        ModelState.AddModelError("Images", "File must be Image Format");
                        return View(request);
                    }

                    if (!item.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Images", "Max File capacity must be 200KB");
                        return View(request);
                    }
                }

                for (int i = 0; i < request.Images.Length; i++)
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + request.Images[i].FileName;
                    string path = Path.Combine(_environment.WebRootPath, "assets/img", fileName);
                    await request.Images[i].SaveFileToLocalAsync(path);

                    await _imageService.CreateAsync(new ProductImageCreateVM() { Image = fileName, ProductId = id, IsMain = false });
                }
            }

            await _service.UpdateAsync(request);

            if (request.ColorIds != null)
            {
                foreach (var colorId in request.ColorIds)
                {
                    await _productColorService.CreateAsync(new ProductColorCreateVM() { ProductId = id, ColorId = colorId });
                }
            }
            if (request.TagIds != null)
            {
                foreach (var tagId in request.TagIds)
                {
                    await _productTagService.CreateAsync(new ProductTagCreateVM() { ProductId = id, TagId = tagId });
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var existColor = await _productColorService.GetByIdAsync(id);
            await _productColorService.Delete(existColor);
            return PartialView("_ColorOptionPartial", existColor);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteImgByProductId(int imgId, int productId)
        {
            var existImg = await _imageService.GetByIdAsync(imgId);
            var product = await _service.GetByIdAsync(productId);

            if (existImg.IsMain) return Content("IsMainError");

            if (product?.ProductImages.Count() == 1) return Content("CountError");

            await _imageService.Delete(existImg);

            return Content("Success");
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> MakeMain(int imgId, int productId)
        {
            var product = await _service.GetByIdAsync(productId);

            foreach (var item in product.ProductImages)
            {
                item.IsMain = false;
                await _imageService.UpdateAsync(item);
            }
            var existImg = await _imageService.GetByIdAsync(imgId);

            existImg.IsMain = true;
            await _imageService.UpdateAsync(existImg);


            return Ok();
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Product product = await _context.Products
                                            .Include(p => p.ProductImages)
                                            .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return NotFound();

            foreach (ProductImage image in product.ProductImages)
            {
                image.Image.DeleteFile(_environment.WebRootPath, "assets/img");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Detail(int? id)
        {
            ViewBag.Colors = await _colorService.GetByIdAsync((int)id);
            ViewBag.Categories = await _categoryService.GetAllSelectListAsync();
            ViewBag.Brands = await _brandService.GetAllSelectListAsync();
            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            if (id == null) return BadRequest();
            Product product = await _context.Products.Include(m => m.ProductColors).ThenInclude(m => m.Color).Include(m => m.ProductTags).ThenInclude(m => m.Tag).Include(m => m.ProductImages).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            List<ProductImageVM> productImages = new();
            foreach (var item in product.ProductImages)
            {
                productImages.Add(new ProductImageVM
                {
                    Image = item.Image,
                    IsMain = item.IsMain
                });

            }
            Product model = new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                Category = product.Category,
                ProductImages = product.ProductImages.ToList(),
                Brand = product.Brand,
                ProductColors = product.ProductColors,
                ProductTags = product.ProductTags.ToList(),
                Weight = product.Weight,
                Quantity = product.Quantity

            };
            return View(model);

        }


    }
}
