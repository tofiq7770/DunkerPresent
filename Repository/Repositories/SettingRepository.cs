using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class SettingRepository : BaseRepository<Setting>, ISettingRepository
    {
        public SettingRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<bool> AnyAsync(string key)
        {
            return await _context.Settings.AnyAsync(m => m.Key == key);
        }
        public async Task<Dictionary<string, string>> GetAll()
        {
            return await _context.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);
        }
    }
}
