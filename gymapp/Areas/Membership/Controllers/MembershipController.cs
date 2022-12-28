using App.Data;
using App.Models;
using MembershipModel = App.Models.Memberships.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Memebership.Controllers
{
    [Area("Membership")]
    [Route("admin/membership/{action=Index}/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class MembershipController : Controller
    {
        private readonly GymAppDbContext _context;

        public MembershipController(GymAppDbContext context)
        {
            _context = context;
        }

        [TempData] public string? StatusMessage { get; set; }

        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize)
        {
            var memberships = _context.Memberships;

            int totalMemberships = await memberships.CountAsync();
            if (pagesize <= 0) pagesize = 10;
            int countPages = (int)Math.Ceiling((double)totalMemberships / pagesize);

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
            ViewBag.totalMemberships = totalMemberships;

            ViewBag.membershipIndex = (currentPage - 1) * pagesize;

            var membershipsInPage = await memberships.Skip((currentPage - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return View(membershipsInPage);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Fee,Level,Duration,Hours,Bonus")] MembershipModel membership)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membership);

                await _context.SaveChangesAsync();
                StatusMessage = "Vừa tạo gói tập mới";
                return RedirectToAction(nameof(Index));
            }

            return View(membership);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null)
            {
                return NotFound();
            }

            var membershipEdit = new MembershipModel()
            {
                Fee = membership.Fee,
                Level = membership.Level,
                Duration = membership.Duration,
                Hours = membership.Hours,
                Bonus = membership.Bonus
            };

            return View(membershipEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("MembershipId,Fee,Level,Duration,Hours,Bonus")] MembershipModel membership)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var membershipUpdate = await _context.Memberships.FindAsync(membership.MembershipId);
                    if (membershipUpdate == null)
                    {
                        return NotFound();
                    }

                    membershipUpdate.Fee = membership.Fee;
                    membershipUpdate.Level = membership.Level;
                    membershipUpdate.Duration = membership.Duration;
                    membershipUpdate.Hours = membership.Hours;
                    membershipUpdate.Bonus = membership.Bonus;

                    _context.Update(membershipUpdate);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipExists(membership.MembershipId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                StatusMessage = "Vừa cập nhật gói tập";
                return RedirectToAction(nameof(Index));
            }
            return View(membership);
        }

        // GET: Blog/Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships.FirstOrDefaultAsync(m => m.MembershipId == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // POST: Blog/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var membership = await _context.Memberships.FindAsync(id);

            if (membership == null)
            {
                return NotFound();
            }

            _context.Memberships.Remove(membership);
            await _context.SaveChangesAsync();

            StatusMessage = "Bạn vừa xóa gói tập: " + membership.Level;

            return RedirectToAction(nameof(Index));
        }

        private bool MembershipExists(int id)
        {
            return _context.Memberships.Any(e => e.MembershipId == id);
        }
    }
}