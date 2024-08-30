using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.ProductImageVMs;

namespace Service.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _repository;
        private readonly IMapper _mapper;

        public ProductImageService(IProductImageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(ProductImageCreateVM model)
        {
            await _repository.CreateAsync(_mapper.Map<ProductImage>(model));
        }

        public async Task Delete(ProductImage model)
        {
            await _repository.DeleteAsync(model);
        }

        public async Task<IEnumerable<ProductImage>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ProductImage> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(ProductImage model)
        {
            await _repository.UpdateAsync(model);
        }
    }
}
