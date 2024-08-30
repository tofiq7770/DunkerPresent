using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Setting;

namespace Service.Services
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _repository;
        public readonly IMapper _mapper;

        public SettingService(ISettingRepository settingRepository, IMapper mapper)
        {
            _repository = settingRepository;
            _mapper = mapper;
        }

        public async Task<Dictionary<string, string>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<SettingListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SettingListVM>>(await _repository.GetAllAsync());
        }

        public async Task<SettingUpdateVM> GetByIdAsync(int id)
        {
            return _mapper.Map<SettingUpdateVM>(await _repository.GetByIdAsync(id));
        }

        public async Task CreateAsync(SettingCreateVM model)
        {
            var mapSetting = _mapper.Map<Setting>(model);
            await _repository.CreateAsync(mapSetting);
        }

        public async Task UpdateAsync(int id, SettingUpdateVM model)
        {
            var dbSetting = await _repository.GetByIdAsync(id);

            var mapSetting = _mapper.Map(model, dbSetting);

            await _repository.UpdateAsync(mapSetting);
        }

        public async Task<bool> AnyAsync(string key)
        {
            return await _repository.AnyAsync(key.ToLower().Trim());
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));
        }
    }
}