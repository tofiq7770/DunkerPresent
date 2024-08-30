using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Color;
using System.Linq.Expressions;

namespace Service.Services
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _repository;
        private readonly IMapper _mapper;

        public ColorService(IColorRepository ColorRepository, IMapper mapper)
        {
            _repository = ColorRepository;
            _mapper = mapper;
        }

        public async Task<bool> AnyAsync(string name)
        {
            return await _repository.AnyAsync(name.ToLower().Trim());
        }

        public async Task<IEnumerable<ColorListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<ColorListVM>>(await _repository.GetAllAsync());
        }

        public async Task<ColorUpdateVM> GetByIdAsync(int id)
        {
            return _mapper.Map<ColorUpdateVM>(await _repository.GetByIdAsync(id));
        }

        public async Task CreateAsync(ColorCreateVM model)
        {
            var mapColor = _mapper.Map<Color>(model);
            await _repository.CreateAsync(mapColor);
        }

        public async Task UpdateAsync(int id, ColorUpdateVM model)
        {
            var dbColor = await _repository.GetByIdAsync(id);

            var mapColor = _mapper.Map(model, dbColor);

            await _repository.UpdateAsync(mapColor);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));
        }

        public Task<bool> IsExistAsync(Expression<Func<Color, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<SelectList> GetAllSelectListAsync()
        {
            return new SelectList(await _repository.GetAllAsync(), "Id", "Name");
        }

        public async Task<SelectList> GetAllSelectListAsync(IEnumerable<int> colorIds)
        {
            var colors = await _repository.GetAllAsync();
            colors = colors.Where(m => !colorIds.Contains(m.Id)).ToList();

            return new SelectList(colors, "Id", "Name");
        }
    }
}
