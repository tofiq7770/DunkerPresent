using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.Tag;
using System.Linq.Expressions;
namespace Service.Services.Interfaces
{
    public interface ITagService
    {

        public Task<bool> IsExistAsync(Expression<Func<Tag, bool>> expression);
        Task<IEnumerable<TagListVM>> GetAllAsync();
        Task<bool> AnyAsync(string name);
        Task<TagUpdateVM> GetByIdAsync(int id);
        Task CreateAsync(TagCreateVM model);
        Task UpdateAsync(int id, TagUpdateVM model);
        Task DeleteAsync(int id);
        Task<SelectList> GetAllSelectListAsync();
    }
}
