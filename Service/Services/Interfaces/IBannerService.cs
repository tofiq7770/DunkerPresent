using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Service.ViewModels.Banner;
using System.Linq.Expressions;

namespace Service.Services.Interfaces
{
    public interface IBannerService
    {
        Task<IEnumerable<BannerListVM>> GetAllAsync();
        Task<BannerUpdateVM> GetByIdAsync(int id);
        Task<bool> CreateAsync(BannerCreateVM model, ModelStateDictionary modelState, string imagePath);
        public Task<bool> DeleteAsync(int id, string imagePath);
        public Task<BannerUpdateVM?> GetUpdatedBannerAsync(int id);
        public Task<bool?> UpdateAsync(BannerUpdateVM model, ModelStateDictionary modelState, string imagePath);
        Task<bool> AnyAsync(string title);
        public Task<bool> IsExistAsync(Expression<Func<Banner, bool>> expression);
    }
}
