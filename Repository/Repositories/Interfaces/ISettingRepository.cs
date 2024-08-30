using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface ISettingRepository : IBaseRepository<Setting>
    {

        Task<Dictionary<string, string>> GetAll();
        Task<bool> AnyAsync(string key);
    }
}
