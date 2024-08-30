using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Task<bool> AnyAsync(string name);
    }
}
