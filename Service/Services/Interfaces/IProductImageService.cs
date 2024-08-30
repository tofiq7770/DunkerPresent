using Domain.Entities;
using Service.ViewModels.ProductImageVMs;

namespace Service.Services.Interfaces
{
    public interface IProductImageService
    {
        Task CreateAsync(ProductImageCreateVM model);
        Task UpdateAsync(ProductImage model);
        Task Delete(ProductImage model);
        Task<ProductImage> GetByIdAsync(int id);
        Task<IEnumerable<ProductImage>> GetAllAsync();
    }
}
