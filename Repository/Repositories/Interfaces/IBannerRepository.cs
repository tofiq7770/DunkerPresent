using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IBannerRepository : IBaseRepository<Banner>
    {

        Task<bool> AnyAsync(string title);
    }
}
