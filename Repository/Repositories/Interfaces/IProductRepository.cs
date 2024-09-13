using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<bool> AnyAsync(string name);
        Task<List<Product>> GetAllWithDatas();


    }
}
