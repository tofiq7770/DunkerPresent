using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.ProductColorVms;

namespace Service.Services.Interfaces
{
    public interface IProductColorService
    {
        Task CreateAsync(ProductColorCreateVM model);
        Task<IEnumerable<ProductColorListVM>> GetAllByProductIdAsync(int productId);
        Task<SelectList> GetAllSelectListByProductIdAsync(int productId);
        Task Delete(ProductColor model);
        Task<ProductColor> GetByIdAsync(int id);
        Task<IEnumerable<int>> GetAllColorIdsByProductId(int productId);
    }
}
