using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repository.Repositories.Interfaces;
using Service.Helpers.Extentions;
using Service.Services.Interfaces;
using Service.ViewModels.Slider;
using System.Linq.Expressions;

namespace Service.Services
{
    public class SliderService : ISliderService
    {
        private readonly ISliderRepository _repository;
        public readonly IMapper _mapper;

        public SliderService(ISliderRepository sliderRepository, IMapper mapper)
        {
            _repository = sliderRepository;
            _mapper = mapper;
        }

        public async Task<bool> AnyAsync(string title)
        {
            return await _repository.AnyAsync(title.ToLower().Trim());
        }

        public async Task<bool> CreateAsync(SliderCreateVM model, ModelStateDictionary ModelState, string imagePath)
        {
            if (!ModelState.IsValid)
                return false;

            if (!model.Image.CheckImage())
            {
                ModelState.AddModelError("Image", "Please Enter valid format");
                return false;
            }
            string filename = await model.Image.CreateImageAsync(imagePath);

            Slider slider = new()
            {
                Title = model.Title,
                Description = model.Description,
                Image = filename
            };

            await _repository.CreateAsync(slider);
            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, string imagePath)
        {
            var slider = await _repository.GetSingleAsync(x => x.Id == id);

            if (slider is null)
                return false;

            _repository.Delete(slider);
            slider.Image.DeleteImage(imagePath);
            await _repository.SaveAsync();


            return true;
        }

        public async Task<IEnumerable<SliderListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SliderListVM>>(await _repository.GetAllAsync());
        }

        public async Task<SliderUpdateVM> GetByIdAsync(int id)
        {
            return _mapper.Map<SliderUpdateVM>(await _repository.GetByIdAsync(id));
        }
        public async Task<SliderUpdateVM?> GetUpdatedSliderAsync(int id)
        {
            var slider = await _repository.GetSingleAsync(x => x.Id == id);

            if (slider is null)
                return null;

            SliderUpdateVM SliderUpdateVM = new()
            {
                Image = slider.Image,
                Description = slider.Description,
                Title = slider.Title,
            };

            return SliderUpdateVM;
        }

        public async Task<bool> IsExistAsync(Expression<Func<Slider, bool>> expression)
        {
            return await _repository.IsExistAsync(expression);
        }


        public async Task<bool?> UpdateAsync(SliderUpdateVM vm, ModelStateDictionary ModelState, string imagePath)
        {
            if (!ModelState.IsValid)
                return false;

            var existSlider = await _repository.GetSingleAsync(x => x.Id == vm.Id);
            var oldImage = existSlider?.Image;
            if (existSlider is null)
                return null;

            if (vm.Photo is not null && !vm.Photo.CheckImage())
            {
                ModelState.AddModelError("Photo", "Please Enter valid input");
                return false;
            }

            existSlider.Title = vm.Title;
            existSlider.Description = vm.Description;
            if (vm.Photo is not null)
            {
                existSlider.Image.DeleteImage(imagePath);
                existSlider.Image = await vm.Photo.CreateImageAsync(imagePath);
            }
            else
            {
                existSlider.Image = oldImage;
            }

            _repository.Update(existSlider);
            await _repository.SaveAsync();

            return true;

        }
    }

}