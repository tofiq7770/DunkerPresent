using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IInfoBannerRepository : IBaseRepository<InfoBanner>
    {
        Task<bool> AnyAsync(string key);
    }
}
