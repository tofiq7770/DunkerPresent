using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class SliderRepository : BaseRepository<Slider>, ISliderRepository
    {
        public SliderRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<bool> AnyAsync(string title)
        {
            return await _context.Sliders.AnyAsync(m => m.Title == title);
        }
    }
}
