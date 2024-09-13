using Domain.Entities;
using Service.ViewModels.Product;


namespace Service.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllAsync(int? categoryId = null);
        Task<bool> AnyAsync(string name);
        public Task SendViewBagElements(dynamic ViewBag);
        public Task<int> CreateAsync(ProductCreateVM model);
        public Task<bool> DeleteAsync(int id, string ImagePath);
        public Task<ProductUpdateVM?> GetUpdatedProductAsync(int id, dynamic ViewBag);
        public Task UpdateAsync(ProductUpdateVM model);
        public Task<ProductDetailVM?> GetByIdAsync(int id);

        public Task<List<Product>> GetBestProducts();
        public Task<List<Product>> GetProductsByCategoryId(int categoryId);
        public Task<List<Product>> GetNewProducts();
        public Task<List<Product>> GetBestSellerProducts();
    }
}
