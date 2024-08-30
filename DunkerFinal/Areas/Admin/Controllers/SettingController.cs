using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Setting;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {

            return View(await _settingService.GetAllAsync());
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _settingService.GetByIdAsync(id));
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (await _settingService.AnyAsync(request.Key))
            {
                ModelState.AddModelError("Key", $"{request.Key} is already exist!");
                return View(request);
            }
            await _settingService.CreateAsync(request);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();

            var setting = await _settingService.GetByIdAsync((int)id);

            if (setting == null) return NotFound();

            return View(setting);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SettingUpdateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (id == null) return BadRequest();

            var setting = await _settingService.GetByIdAsync((int)id);

            if (setting == null) return NotFound();

            await _settingService.UpdateAsync((int)id, request);

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid) return View();

            if (id == null) return BadRequest();
            var setting = await _settingService.GetByIdAsync((int)id);

            if (setting == null) return NotFound();

            await _settingService.DeleteAsync((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}
