using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Service.ViewModels.Slider;
using System.Linq.Expressions;

namespace Service.Services.Interfaces
{
    public interface ISliderService
    {
        Task<IEnumerable<SliderListVM>> GetAllAsync();
        Task<SliderUpdateVM> GetByIdAsync(int id);
        Task<bool> CreateAsync(SliderCreateVM model, ModelStateDictionary modelState, string imagePath);
        public Task<bool> DeleteAsync(int id, string imagePath);
        public Task<SliderUpdateVM?> GetUpdatedSliderAsync(int id);
        public Task<bool?> UpdateAsync(SliderUpdateVM model, ModelStateDictionary modelState, string imagePath);
        Task<bool> AnyAsync(string title);
        public Task<bool> IsExistAsync(Expression<Func<Slider, bool>> expression);
    }
}
