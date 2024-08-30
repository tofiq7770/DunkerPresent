using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.ProductTagVMs;

namespace Service.Services
{
    public class ProductTagService : IProductTagService
    {
        private readonly IProductTagRepository _repository;
        public readonly IMapper _mapper;

        public ProductTagService(IProductTagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(ProductTagCreateVM model)
        {
            await _repository.CreateAsync(_mapper.Map<ProductTag>(model));
        }
    }
}
