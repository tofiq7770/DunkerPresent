using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IColorRepository : IBaseRepository<Color>
    {
        Task<bool> AnyAsync(string name);
    }
}
