using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.Category;
using System.Linq.Expressions;

namespace Service.Services.Interfaces
{
    public interface ICategoryService
    {

        public Task<bool> IsExistAsync(Expression<Func<Category, bool>> expression);
        Task<IEnumerable<CategoryListVM>> GetAllAsync();
        Task<CategoryUpdateVM> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateVM model);
        Task<bool> AnyAsync(string name);
        Task UpdateAsync(int id, CategoryUpdateVM model);
        Task DeleteAsync(int id);
        Task<SelectList> GetAllSelectListAsync();
    }
}
