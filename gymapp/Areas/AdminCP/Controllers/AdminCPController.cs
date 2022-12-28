using App.Controllers;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Admin.Controllers
{
    [Area("AdminCP")]
    [Authorize(Roles = RoleName.Administrator)]
    public class AdminCPController : Controller
    {
        protected readonly GymAppDbContext _context;

        public AdminCPController(GymAppDbContext context)
        {
            _context = context;
        }

        [Route("/admin/")]
        public async Task<IActionResult> Index()
        {
            ViewBag.totalMembers = await _context.Users.CountAsync();
            ViewBag.totalInstructors = await _context.Instructors.CountAsync();
            ViewBag.totalRooms = await _context.Rooms.CountAsync();
            ViewBag.totalProducts = await _context.Products.CountAsync();

            var payments = await _context.Payments.ToListAsync();
            decimal total = 0;
            foreach (var payment in payments)
            {
                total += payment.TotalPrice;
            }

            var signupClasses = await _context.SignupClasses.Include(p => p.Payment).ToListAsync();
            decimal totalClass = 0;
            foreach (var signupClass in signupClasses)
            {
                totalClass += signupClass.Payment.TotalPrice;
            }

            var products = await _context.PaymentDetails.Include(p => p.Payment).ToListAsync();
            decimal totalProduct = 0;
            foreach (var product in products)
            {
                totalProduct += product.Payment.TotalPrice;
            }

            var signupMembeships = await _context.SignupMemberships.Include(p => p.Payment).ToListAsync();
            decimal totalMembership = 0;
            foreach (var signupMembership in signupMembeships)
            {
                totalMembership += signupMembership.Payment.TotalPrice;
            }

            ViewBag.total = total;
            ViewBag.totalClass = totalClass;
            ViewBag.totalProduct = totalProduct;
            ViewBag.totalMembership = totalMembership;

            return View();
        }
    }
}