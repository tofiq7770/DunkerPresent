﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.ViewModels.Slider;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SliderController : Controller
    {
        private readonly ISliderService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagePath;
        public SliderController(ISliderService service, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img");
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {
            var sliders = await _service.GetAllAsync();

            return View(sliders);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Detail(int id)
        {

            return View(await _service.GetByIdAsync(id));
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {

            if (!ModelState.IsValid) return View(vm);
            if (await _service.AnyAsync(vm.Title))
            {
                ModelState.AddModelError("Title", $"{vm.Title} is already exist!");
                return View(vm);
            }
            var result = await _service.CreateAsync(vm, ModelState, _imagePath);

            if (!result)
                return View(vm);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _service.DeleteAsync(id, _imagePath);

            if (!result)
                return NotFound();

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _service.GetUpdatedSliderAsync(id);

            if (result is null)
                return NotFound();

            return View(result);

        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Update(SliderUpdateVM vm)
        {
            var result = await _service.UpdateAsync(vm, ModelState, _imagePath);

            if (result is null)
                return NotFound();
            else if (result is false)
                return View(vm);

            return RedirectToAction("Index");
        }
    }
}