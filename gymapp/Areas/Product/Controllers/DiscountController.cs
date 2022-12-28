using App.Data;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using App.Models.Payments;

namespace App.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("admin/product/discount/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class DiscountController : Controller
    {
        private readonly GymAppDbContext _context;

        public DiscountController(GymAppDbContext context)
        {
            _context = context;
        }

        [TempData] public string? StatusMessage { get; set; }

        // GET: Product/Product
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize, string keyword)
        {
            var discounts = _context.Discounts.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                discounts = discounts.Where(x => x.Code.Contains(keyword));
            }

            int totalDícounts = await discounts.CountAsync();
            if (pagesize <= 0) pagesize = 5;
            int countPages = (int)Math.Ceiling((double)totalDícounts / pagesize);

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
            ViewBag.totalDícounts = totalDícounts;

            ViewBag.discountIndex = (currentPage - 1) * pagesize;

            var discountsInPage = await discounts.Skip((currentPage - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return View(discountsInPage);
        }

        public IActionResult Details(int id)
        {
            var discountModel = _context.Discounts.Find(id);
            if (discountModel == null)
            {
                return NotFound();
            }

            return View(discountModel);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Percent")] Discount discountModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discountModel);

                await _context.SaveChangesAsync();
                StatusMessage = "Vừa tạo mã giảm giá mới: " + discountModel.Code;
                return RedirectToAction(nameof(Index));
            }

            return View(discountModel);
        }

        // GET: Blog/Post/Delete/5
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var discount = _context.Discounts.Find(id);

            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: Blog/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);

            if (discount == null)
            {
                return NotFound();
            }

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();

            StatusMessage = "Bạn vừa mã giảm giá: " + discount.Code;

            return RedirectToAction(nameof(Index));
        }

        private bool DiscountExists(int id)
        {
            return _context.Discounts.Any(e => e.Id == id);
        }
    }
}