using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class BannerRepository : BaseRepository<Banner>, IBannerRepository
    {
        public BannerRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<bool> AnyAsync(string title)
        {
            return await _context.Banners.AnyAsync(m => m.Title == title);
        }
    }
}
