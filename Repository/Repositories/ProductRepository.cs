using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<bool> AnyAsync(string name)
        {
            return await _context.Products.AnyAsync(m => m.Name == name);
        }
        public async Task<List<Product>> GetAllWithDatas()
        {
            return await _entities.Include(e => e.ProductColors).Include(e => e.Brand).Include(m => m.Category)
                                                                .Include(c => c.ProductTags).ThenInclude(m => m.Tag).ToListAsync();
        }
    }
}
