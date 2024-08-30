using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Service.ViewModels.UserVMS;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserVM>();

            foreach (var user in users)
            {
                var userRoleNames = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserVM
                {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = string.Join(", ", userRoleNames),
                    UserId = user.Id,
                    UserRoles = userRoleNames.ToList()
                });
            }

            return View(userRoles);
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddRole()
        {

            ViewBag.Users = new SelectList(await _userManager.Users.ToListAsync(), "Id", "UserName");
            ViewBag.Roles = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddRole(AddRoleVM request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            var role = await _roleManager.FindByIdAsync(request.RoleId);

            if (user != null && role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RemoveRole(RemoveRoleVM request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Count <= 1)
            {
                TempData["Error"] = "Cannot remove the last remaining role.";
                return RedirectToAction("Index");
            }

            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role != null)
            {
                await _userManager.RemoveFromRoleAsync(user, request.RoleName);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetRoleCount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Json(new { roleCount = 0 });
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Json(new { roleCount = roles.Count });
        }
    }
}
