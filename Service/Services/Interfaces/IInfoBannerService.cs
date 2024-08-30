using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Service.ViewModels.InfoBanner;
using System.Linq.Expressions;

namespace Service.Services.Interfaces
{
    public interface IInfoBannerService
    {
        Task<IEnumerable<InfoBannerListVM>> GetAllAsync();
        Task<InfoBannerUpdateVM> GetByIdAsync(int id);
        Task<bool> CreateAsync(InfoBannerCreateVM model, ModelStateDictionary modelState, string imagePath);
        public Task<bool> DeleteAsync(int id, string imagePath);
        public Task<InfoBannerUpdateVM?> GetUpdatedInfoBannerAsync(int id);
        public Task<bool?> UpdateAsync(InfoBannerUpdateVM model, ModelStateDictionary modelState, string imagePath);
        Task<bool> AnyAsync(string title);
        public Task<bool> IsExistAsync(Expression<Func<InfoBanner, bool>> expression);
    }
}
