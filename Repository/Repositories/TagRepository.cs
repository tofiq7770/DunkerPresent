using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<bool> AnyAsync(string name)
        {
            return await _context.Tags.AnyAsync(m => m.Name == name);
        }
    }
}
