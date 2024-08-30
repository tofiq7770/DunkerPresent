using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class ColorRepository : BaseRepository<Color>, IColorRepository
    {
        public ColorRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<bool> AnyAsync(string name)
        {
            return await _context.Colors.AnyAsync(m => m.Name == name);
        }
    }
}
