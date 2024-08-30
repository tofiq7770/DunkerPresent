using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repository.Repositories.Interfaces;
using Service.Helpers.Extentions;
using Service.Services.Interfaces;
using Service.ViewModels.Banner;
using System.Linq.Expressions;

namespace Service.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _repository;
        public readonly IMapper _mapper;

        public BannerService(IBannerRepository BannerRepository, IMapper mapper)
        {
            _repository = BannerRepository;
            _mapper = mapper;
        }

        public async Task<bool> AnyAsync(string title)
        {
            return await _repository.AnyAsync(title.ToLower().Trim());
        }

        public async Task<bool> CreateAsync(BannerCreateVM model, ModelStateDictionary ModelState, string imagePath)
        {
            if (!ModelState.IsValid)
                return false;

            if (!model.Image.CheckImage())
            {
                ModelState.AddModelError("Image", "Please Enter valid format");
                return false;
            }
            string filename = await model.Image.CreateImageAsync(imagePath);

            Banner banner = new()
            {
                Title = model.Title,
                Description = model.Description,
                Image = filename
            };

            await _repository.CreateAsync(banner);
            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, string imagePath)
        {
            var Banner = await _repository.GetSingleAsync(x => x.Id == id);

            if (Banner is null)
                return false;

            _repository.Delete(Banner);
            Banner.Image.DeleteImage(imagePath);
            await _repository.SaveAsync();


            return true;
        }

        public async Task<IEnumerable<BannerListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<BannerListVM>>(await _repository.GetAllAsync());
        }

        public async Task<BannerUpdateVM> GetByIdAsync(int id)
        {
            return _mapper.Map<BannerUpdateVM>(await _repository.GetByIdAsync(id));
        }
        public async Task<BannerUpdateVM?> GetUpdatedBannerAsync(int id)
        {
            var banner = await _repository.GetSingleAsync(x => x.Id == id);

            if (banner is null)
                return null;

            BannerUpdateVM BannerUpdateVM = new()
            {
                Image = banner.Image,
                Description = banner.Description,
                Title = banner.Title,
            };

            return BannerUpdateVM;
        }

        public async Task<bool> IsExistAsync(Expression<Func<Banner, bool>> expression)
        {
            return await _repository.IsExistAsync(expression);
        }


        public async Task<bool?> UpdateAsync(BannerUpdateVM vm, ModelStateDictionary ModelState, string imagePath)
        {
            if (!ModelState.IsValid)
                return false;

            var existBanner = await _repository.GetSingleAsync(x => x.Id == vm.Id);
            var oldImage = existBanner?.Image;
            if (existBanner is null)
                return null;

            if (vm.Photo is not null && !vm.Photo.CheckImage())
            {
                ModelState.AddModelError("Photo", "Please Enter valid input");
                return false;
            }

            existBanner.Title = vm.Title;
            existBanner.Description = vm.Description;
            if (vm.Photo is not null)
            {
                existBanner.Image.DeleteImage(imagePath);
                existBanner.Image = await vm.Photo.CreateImageAsync(imagePath);
            }
            else
            {
                existBanner.Image = oldImage;
            }

            _repository.Update(existBanner);
            await _repository.SaveAsync();

            return true;

        }
    }

}
