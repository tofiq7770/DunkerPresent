using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Helpers.Extentions;
using Service.Services.Interfaces;
using Service.ViewModels.Brand;
using System.Linq.Expressions;

namespace Service.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repository;
        public readonly IMapper _mapper;

        public BrandService(IBrandRepository BrandRepository, IMapper mapper)
        {
            _repository = BrandRepository;
            _mapper = mapper;
        }

        public async Task<bool> AnyAsync(string name)
        {
            return await _repository.AnyAsync(name.ToLower().Trim());
        }

        public async Task<bool> CreateAsync(BrandCreateVM model, string imagePath)
        {
            //if (!ModelState.IsValid)
            //    return false;

            //if (!model.Image.CheckImage())
            //{
            //    ModelState.AddModelError("Image", "Please Enter valid format");
            //    return false;
            //}
            string filename = await model.Image.CreateImageAsync(imagePath);

            Brand brand = new()
            {
                Name = model.Name,
                Image = filename
            };

            await _repository.CreateAsync(brand);
            await _repository.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, string imagePath)
        {
            var Brand = await _repository.GetSingleAsync(x => x.Id == id);

            if (Brand is null)
                return false;

            _repository.Delete(Brand);
            Brand.Image.DeleteImage(imagePath);
            await _repository.SaveAsync();


            return true;
        }

        public async Task<IEnumerable<BrandListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<BrandListVM>>(await _repository.GetAllAsync());
        }

        public async Task<BrandUpdateVM> GetByIdAsync(int id)
        {
            return _mapper.Map<BrandUpdateVM>(await _repository.GetByIdAsync(id));
        }
        public async Task<BrandUpdateVM?> GetUpdatedBrandAsync(int id)
        {
            var brand = await _repository.GetSingleAsync(x => x.Id == id);

            if (brand is null)
                return null;

            BrandUpdateVM BrandUpdateVM = new()
            {
                Image = brand.Image,
                Name = brand.Name,
            };

            return BrandUpdateVM;
        }

        public async Task<bool> IsExistAsync(Expression<Func<Brand, bool>> expression)
        {
            return await _repository.IsExistAsync(expression);
        }


        public async Task<bool?> UpdateAsync(BrandUpdateVM vm, string imagePath)
        {
            //if (!ModelState.IsValid)
            //    return false;

            var existBrand = await _repository.GetSingleAsync(x => x.Id == vm.Id);
            var oldImage = existBrand?.Image;
            if (existBrand is null)
                return null;

            //if (vm.Photo is not null && !vm.Photo.CheckImage())
            //{
            //    ModelState.AddModelError("Photo", "Please Enter valid input");
            //    return false;
            //}

            existBrand.Name = vm.Name;
            if (vm.Photo is not null)
            {
                existBrand.Image.DeleteImage(imagePath);
                existBrand.Image = await vm.Photo.CreateImageAsync(imagePath);
            }
            else
            {
                existBrand.Image = oldImage;
            }

            _repository.Update(existBrand);
            await _repository.SaveAsync();

            return true;

        }

        public async Task<SelectList> GetAllSelectListAsync()
        {
            return new SelectList(await _repository.GetAllAsync(), "Id", "Name");
        }
    }

}
