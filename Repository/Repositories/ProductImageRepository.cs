using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class ProductImageRepository : BaseRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<bool> AnyAsync(string name)
        {
            return await _context.ProductImages.AnyAsync(m => m.Product.Name == name);
        }
    }
}
