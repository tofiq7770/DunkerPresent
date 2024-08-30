using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IBrandRepository : IBaseRepository<Brand>
    {
        Task<bool> AnyAsync(string name);

    }
}
