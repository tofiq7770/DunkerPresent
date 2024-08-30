using Domain.Entities;
using DunkerFinal.ViewModels.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;

namespace DunkerFinal.Controllers
{

    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ContactCreateVM Contact)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            await _context.Contacts.AddAsync(new Contact { Name = Contact.Name, Message = Contact.Message, Email = Contact.Email });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            Contact Contact = await _context.Contacts.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (Contact == null) return NotFound();

            _context.Contacts.Remove(Contact);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
