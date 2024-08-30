using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {

        private readonly AppDbContext _context;
        public ContactController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {
            List<Contact> contacts = await _context.Contacts.ToListAsync();
            return View(contacts);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id <= 0) return BadRequest();
            Contact contacts = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);
            return View(contacts);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Contact contacts = await _context.Contacts.FirstOrDefaultAsync(s => s.Id == id);

            if (contacts is null) return NotFound();


            _context.Contacts.Remove(contacts);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
