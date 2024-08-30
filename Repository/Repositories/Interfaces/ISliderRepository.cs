using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface ISliderRepository : IBaseRepository<Slider>
    {
        Task<bool> AnyAsync(string title);

    }
}
