using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.ProductColorVms;

namespace Service.Services
{
    public class ProductColorService : IProductColorService
    {
        private readonly IProductColorRepository _repository;
        public readonly IMapper _mapper;

        public ProductColorService(IProductColorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(ProductColorCreateVM model)
        {
            await _repository.CreateAsync(_mapper.Map<ProductColor>(model));
        }

        public async Task<IEnumerable<ProductColorListVM>> GetAllByProductIdAsync(int productId)
        {
            IEnumerable<ProductColor> productColors = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<ProductColorListVM>>(productColors.Where(m => m.ProductId == productId));
        }

        public async Task Delete(ProductColor model)
        {
            await _repository.DeleteAsync(model);
        }

        public async Task<ProductColor> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id, "Color");
        }

        public async Task<SelectList> GetAllSelectListByProductIdAsync(int productId)
        {
            var productColors = await _repository.GetAllAsync();
            var mapProductColors = _mapper.Map<IEnumerable<ProductColorListVM>>(productColors.Where(m => m.ProductId != productId));

            return new SelectList(mapProductColors, "ColorId", "ColorName");
        }

        public async Task<IEnumerable<int>> GetAllColorIdsByProductId(int productId)
        {
            var productColors = await _repository.GetAllAsync();

            var assignedColorIds = productColors
                .Where(pc => pc.ProductId == productId)
                .Select(pc => pc.ColorId)
                .ToList();

            return assignedColorIds;
        }
    }
}
