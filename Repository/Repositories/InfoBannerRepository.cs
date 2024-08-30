using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class InfoBannerRepository : BaseRepository<InfoBanner>, IInfoBannerRepository
    {
        public InfoBannerRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<bool> AnyAsync(string title)
        {
            return await _context.InfoBanners.AnyAsync(m => m.Title == title);
        }
    }
}