using App.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using Microsoft.EntityFrameworkCore;
using ContactModel = App.Models.Contacts.Contact;
using App.Models.Classes;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing.Printing;

namespace App.Areas.Contact.Controllers
{
    [Area("Contact")]
    [Authorize(Roles = RoleName.Administrator)]
    public class ContactController : Controller
    {
        private readonly GymAppDbContext _context;

        public ContactController(GymAppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/admin/contact")]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize, string keyword)
        {
            ViewBag.Keyword = keyword;

            IOrderedQueryable<ContactModel> contacts;
            if (keyword != null)
            {
                contacts = _context.Contacts.Where(p => p.FullName.Contains(keyword) || p.Email.Contains(keyword) || p.Phone.Contains(keyword)).OrderBy(p => p.FullName);
            }
            else
            {
                contacts = _context.Contacts.OrderBy(p => p.FullName);
            }

            int totalContacts = await contacts.CountAsync();
            if (pagesize <= 0) pagesize = 5;
            int countPages = (int)Math.Ceiling((double)totalContacts / pagesize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    p = pageNumber,
                    pagesize = pagesize
                }) ?? string.Empty
            };

            ViewBag.pagingModel = pagingModel;
            ViewBag.totalContacts = totalContacts;

            ViewBag.contactIndex = (currentPage - 1) * pagesize;

            var contactsInPage = await contacts.Skip((currentPage - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return View(contactsInPage);
            //var contacts = await _context.Contacts.ToListAsync();
            //return View(contacts);
        }

        [HttpGet("/admin/contact/detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [TempData] public string StatusMessage { set; get; }

        [HttpGet("/contact/")]
        [AllowAnonymous]
        public IActionResult SendContact()
        {
            return View();
        }

        [HttpPost("/contact/")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendContact([Bind("FullName,Email,Message,Phone")] ContactModel contact)
        {
            if (ModelState.IsValid)
            {
                contact.DateSent = DateTime.Now;

                _context.Add(contact);
                await _context.SaveChangesAsync();

                //TempData["SuccessMessage"] = "Gửi liên hệ thành công.";
                StatusMessage = "Gửi liên hệ thành công.";

                return RedirectToAction("SendContact");
                //return RedirectToAction("Index", "Contact");
            }

            return View(contact);
        }

        [HttpGet("/admin/contact/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost("/admin/contact/delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            StatusMessage = "Xóa liên hệ thành công.";

            return RedirectToAction(nameof(Index));
        }
    }
}