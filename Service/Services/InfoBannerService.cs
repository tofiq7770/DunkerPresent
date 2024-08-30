using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repository.Repositories.Interfaces;
using Service.Helpers.Extentions;
using Service.Services.Interfaces;
using Service.ViewModels.InfoBanner;
using System.Linq.Expressions;

namespace Service.Services
{
    public class InfoBannerService : IInfoBannerService
    {
        private readonly IInfoBannerRepository _repository;
        public readonly IMapper _mapper;

        public InfoBannerService(IInfoBannerRepository infoBannerRepository, IMapper mapper)
        {
            _repository = infoBannerRepository;
            _mapper = mapper;
        }

        public async Task<bool> AnyAsync(string title)
        {
            return await _repository.AnyAsync(title.ToLower().Trim());
        }

        public async Task<bool> CreateAsync(InfoBannerCreateVM model, ModelStateDictionary ModelState, string imagePath)
        {
            if (!ModelState.IsValid)
                return false;

            if (!model.Image.CheckImage())
            {
                ModelState.AddModelError("Image", "Please Enter valid format");
                return false;
            }
            string filename = await model.Image.CreateImageAsync(imagePath);

            InfoBanner infoBanner = new()
            {
                Title = model.Title,
                Description = model.Description,
                SubTitle = model.Subtitle,
                Image = filename
            };

            await _repository.CreateAsync(infoBanner);
            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, string imagePath)
        {
            var infoBanner = await _repository.GetSingleAsync(x => x.Id == id);

            if (infoBanner is null)
                return false;

            _repository.Delete(infoBanner);
            infoBanner.Image.DeleteImage(imagePath);
            await _repository.SaveAsync();


            return true;
        }

        public async Task<IEnumerable<InfoBannerListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<InfoBannerListVM>>(await _repository.GetAllAsync());
        }

        public async Task<InfoBannerUpdateVM> GetByIdAsync(int id)
        {
            return _mapper.Map<InfoBannerUpdateVM>(await _repository.GetByIdAsync(id));
        }
        public async Task<InfoBannerUpdateVM?> GetUpdatedInfoBannerAsync(int id)
        {
            var infoBanner = await _repository.GetSingleAsync(x => x.Id == id);

            if (infoBanner is null)
                return null;

            InfoBannerUpdateVM infoBannerUpdateVM = new()
            {
                Image = infoBanner.Image,
                Description = infoBanner.Description,
                Subtitle = infoBanner.SubTitle,
                Title = infoBanner.Title,
            };

            return infoBannerUpdateVM;
        }

        public async Task<bool> IsExistAsync(Expression<Func<InfoBanner, bool>> expression)
        {
            return await _repository.IsExistAsync(expression);
        }


        public async Task<bool?> UpdateAsync(InfoBannerUpdateVM vm, ModelStateDictionary ModelState, string imagePath)
        {
            if (!ModelState.IsValid)
                return false;

            var existInfoBanner = await _repository.GetSingleAsync(x => x.Id == vm.Id);
            var oldImage = existInfoBanner?.Image;
            if (existInfoBanner is null)
                return null;

            if (vm.Photo is not null && !vm.Photo.CheckImage())
            {
                ModelState.AddModelError("Photo", "Please Enter valid input");
                return false;
            }

            existInfoBanner.Title = vm.Title;
            existInfoBanner.SubTitle = vm.Subtitle;
            existInfoBanner.Description = vm.Description;
            if (vm.Photo is not null)
            {
                existInfoBanner.Image.DeleteImage(imagePath);
                existInfoBanner.Image = await vm.Photo.CreateImageAsync(imagePath);
            }
            else
            {
                existInfoBanner.Image = oldImage;
            }

            _repository.Update(existInfoBanner);
            await _repository.SaveAsync();

            return true;

        }
    }

}