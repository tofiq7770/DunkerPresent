using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Product;

namespace Service.Services
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IColorService _colorService;
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, ICategoryService categoryService, ITagService tagService, IColorService colorService, IBrandService brandService, IMapper mapper)
        {
            _repository = repository;
            _categoryService = categoryService;
            _tagService = tagService;
            _colorService = colorService;
            _brandService = brandService;
            _mapper = mapper;
        }

        public async Task<bool> AnyAsync(string name)
        {
            return await _repository.AnyAsync(name.ToLower().Trim());
        }

        public async Task<int> CreateAsync(ProductCreateVM model)
        {
            var productId = await _repository.CreateAsync(_mapper.Map<Product>(model));

            return productId;
        }


        public Task<bool> DeleteAsync(int id, string ImagePath)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetAllAsync(int? categoryId = null)
        {
            var products = await _repository.GetAll("ProductImages", "Category", "ProductColors").ToListAsync();

            if (categoryId is not null)
                products = products.Where(x => x.CategoryId == categoryId).ToList();

            return products;
        }

        public async Task<List<Product>> GetBestProducts()
        {
            var products = await _repository.GetAll("ProductImages").ToListAsync();

            products = products.OrderByDescending(x => x.Quantity).Take(2).ToList();

            return products;
        }

        public Task<List<Product>> GetBestSellerProducts()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDetailVM?> GetByIdAsync(int id)
        {
            return _mapper.Map<ProductDetailVM>(await _repository.GetByIdAsync(id, "ProductImages"));
        }

        public async Task<List<Product>> GetNewProducts()
        {
            var products = await _repository.GetAll("ProductImages").OrderByDescending(x => x.Id).Take(12).ToListAsync();

            return products;
        }

        public async Task<List<Product>> GetProductsByCategoryId(int categoryId)
        {
            var products = await _repository.GetFiltered(x => x.CategoryId == categoryId, "ProductImages").ToListAsync();

            return products;
        }

        public Task<ProductUpdateVM?> GetUpdatedProductAsync(int id, dynamic ViewBag)
        {
            throw new NotImplementedException();
        }

        public async Task SendViewBagElements(dynamic ViewBag)
        {
            var categories = (await _categoryService.GetAllAsync()).ToList();
            var brands = await _brandService.GetAllAsync();
            var tags = await _tagService.GetAllAsync();

            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            ViewBag.Tags = tags;
        }

        public async Task UpdateAsync(ProductUpdateVM model)
        {
            await _repository.UpdateAsync(_mapper.Map<Product>(model));
        }
    }
}
