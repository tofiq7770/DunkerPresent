using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Tag;
using System.Linq.Expressions;
namespace Service.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;
        public readonly IMapper _mapper;

        public async Task<bool> AnyAsync(string name)
        {
            return await _repository.AnyAsync(name.ToLower().Trim());
        }
        public TagService(ITagRepository TagRepository, IMapper mapper)
        {
            _repository = TagRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<TagListVM>>(await _repository.GetAllAsync());
        }

        public async Task<TagUpdateVM> GetByIdAsync(int id)
        {
            return _mapper.Map<TagUpdateVM>(await _repository.GetByIdAsync(id));
        }

        public async Task CreateAsync(TagCreateVM model)
        {
            var mapTag = _mapper.Map<Tag>(model);
            await _repository.CreateAsync(mapTag);
        }

        public async Task UpdateAsync(int id, TagUpdateVM model)
        {
            var dbTag = await _repository.GetByIdAsync(id);

            var mapTag = _mapper.Map(model, dbTag);

            await _repository.UpdateAsync(mapTag);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));
        }

        public async Task<bool> IsExistAsync(Expression<Func<Tag, bool>> expression)
        {
            return await _repository.IsExistAsync(expression, "Products");
        }

        public async Task<SelectList> GetAllSelectListAsync()
        {
            return new SelectList(await _repository.GetAllAsync(), "Id", "Name");
        }
    }
}
