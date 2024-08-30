using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        public BrandRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<bool> AnyAsync(string Name)
        {
            return await _context.Brands.AnyAsync(m => m.Name == Name);
        }
    }
}
