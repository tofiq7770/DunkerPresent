using Service.ViewModels.ProductTagVMs;

namespace Service.Services.Interfaces
{
    public interface IProductTagService
    {

        Task CreateAsync(ProductTagCreateVM model);
    }
}
