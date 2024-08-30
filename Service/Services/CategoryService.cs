using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Category;
using System.Linq.Expressions;

namespace Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public readonly IMapper _mapper;

        public async Task<bool> AnyAsync(string name)
        {
            return await _repository.AnyAsync(name.ToLower().Trim());
        }
        public CategoryService(ICategoryRepository CategoryRepository, IMapper mapper)
        {
            _repository = CategoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CategoryListVM>>(await _repository.GetAllAsync());
        }

        public async Task<CategoryUpdateVM> GetByIdAsync(int id)
        {
            return _mapper.Map<CategoryUpdateVM>(await _repository.GetByIdAsync(id));
        }

        public async Task CreateAsync(CategoryCreateVM model)
        {
            var mapCategory = _mapper.Map<Category>(model);
            await _repository.CreateAsync(mapCategory);
        }

        public async Task UpdateAsync(int id, CategoryUpdateVM model)
        {
            var dbCategory = await _repository.GetByIdAsync(id);

            var mapCategory = _mapper.Map(model, dbCategory);

            await _repository.UpdateAsync(mapCategory);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));
        }

        public async Task<bool> IsExistAsync(Expression<Func<Category, bool>> expression)
        {
            return await _repository.IsExistAsync(expression, "Products");
        }

        public async Task<SelectList> GetAllSelectListAsync()
        {
            return new SelectList(await _repository.GetAllAsync(), "Id", "Name");
        }
    }
}