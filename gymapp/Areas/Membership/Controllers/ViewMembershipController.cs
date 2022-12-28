using App.Areas.Product.Service;
using App.Models;
using App.Models.Memberships;
using App.Models.Payments;
using BraintreeHttp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayPal.Core;
using PayPal.v1.Orders;
using PayPal.v1.Payments;
using System.Net.Mail;
using System.Net;
using Payment = App.Models.Payments.Payment;
using PayPalPayments = PayPal.v1.Payments;

namespace App.Areas.Membership.Controllers
{
    [Area("Membership")]
    [Authorize]
    public class ViewMembershipController : Controller
    {
        private readonly GymAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly string _clientId;
        private readonly string _secretKey;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly CartService _cartService;

        public double TyGiaUSD = 23300;

        public ViewMembershipController(GymAppDbContext context, UserManager<AppUser> userManager, IConfiguration config, IWebHostEnvironment webHostEnvironment, CartService cartService)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _cartService = cartService;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
        }

        [Route("/dich-vu/")]
        public async Task<IActionResult> Index()
        {
            var memberships = await _context.Memberships.ToListAsync();
            var memDefault = await _context.Memberships.FirstOrDefaultAsync();
            var levels = new SelectList(await _context.Memberships.ToListAsync(), "MembershipId", "Level");
            var user = await _userManager.GetUserAsync(User);

            ViewBag.memDefault = memDefault;
            ViewBag.levels = levels;
            ViewBag.user = user;

            return View(memberships);
        }

        [Route("/membership/{id}")]
        public async Task<IActionResult> ShowMembership(int id)
        {
            var membership = _context.Memberships.FirstOrDefault(m => m.MembershipId == id);

            return PartialView("_ShowMembership", membership);
        }

        [HttpGet]
        [Route("/dich-vu/{MembershipId}/xac-nhan-don-hang")]
        public async Task<IActionResult> Checkout(int MembershipId)
        {
            var membership = await _context.Memberships.FirstOrDefaultAsync(m => m.MembershipId == MembershipId);
            var user = await _userManager.GetUserAsync(User);

            ViewBag.membership = membership;
            ViewBag.user = user;

            return View();
        }

        private static int memId;

        [HttpPost]
        [Route("/dich-vu/{MembershipId}/xac-nhan-don-hang")]
        public async Task<IActionResult> Checkout(int MembershipId, [Bind("DateCreated, TotalPrice, PaymentMode")] Payment paymentt)
        {
            var membership = await _context.Memberships.FirstOrDefaultAsync(m => m.MembershipId == MembershipId);
            if (membership == null)
            {
                return NotFound();
            }

            if (paymentt.PaymentMode == "Paypal")
            {
                memId = MembershipId;

                var enviroment = new SandboxEnvironment(_clientId, _secretKey);
                var client = new PayPalHttpClient(enviroment);

                #region Create Paypal Order

                var itemList = new PayPalPayments.ItemList()
                {
                    Items = new List<PayPalPayments.Item>()
                };
                var total = Math.Round((double)membership.Fee / TyGiaUSD, 2);
                itemList.Items.Add(new PayPalPayments.Item()
                {
                    Name = "Gói tập: " + membership.Level,
                    Currency = "USD",
                    Price = total.ToString(),
                    Quantity = "1",
                    Sku = "sku",
                    Tax = "0"
                });

                #endregion Create Paypal Order

                var paypalOrderId = DateTime.Now.Ticks;
                var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                var paymentPayPal = new PayPal.v1.Payments.Payment()
                {
                    Intent = "sale",
                    Transactions = new List<PayPalPayments.Transaction>()
                {
                    new PayPalPayments.Transaction()
                    {
                        Amount = new PayPalPayments.Amount()
                        {
                            Total = total.ToString(),
                            Currency = "USD",
                            Details = new PayPalPayments.AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = total.ToString()
                            }
                        },
                        ItemList = itemList,
                        Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                    RedirectUrls = new PayPalPayments.RedirectUrls()
                    {
                        CancelUrl = $"{hostname}/dich-vu/thanh-toan-paypal/that-bai",
                        ReturnUrl = $"{hostname}/dich-vu/thanh-toan-paypal/thanh-cong"
                    },
                    Payer = new PayPalPayments.Payer()
                    {
                        PaymentMethod = "paypal"
                    }
                };

                PayPalPayments.PaymentCreateRequest request = new PayPalPayments.PaymentCreateRequest();
                request.RequestBody(paymentPayPal);

                try
                {
                    var response = await client.Execute(request);
                    var statusCode = response.StatusCode;
                    PayPal.v1.Payments.Payment result = response.Result<PayPal.v1.Payments.Payment>();

                    var links = result.Links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        PayPalPayments.LinkDescriptionObject lnk = links.Current;
                        if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.Href;
                        }
                    }

                    return Redirect(paypalRedirectUrl);
                    //return Json(new { redirectUrl = paypalRedirectUrl });
                }
                catch (HttpException httpException)
                {
                    var statusCode = httpException.StatusCode;
                    var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                    //Process when Checkout with Paypal fails
                    return Redirect("/dich-vu/thanh-toan-paypal/that-bai");
                    //return Json(new { redirectUrl = "/GioHang/CheckoutFail" });
                }
            }
            
            var user = await _userManager.GetUserAsync(this.User);

            var dateCreated = DateTime.Now;

            var payment = new Payment
            {
                DateCreated = dateCreated,
                PaymentMode = "Trả sau",
                TotalPrice = membership.Fee,
                UserID = user.Id
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var SignupMembership = new SignupMembership
            {
                MembershipId = membership.MembershipId,
                PaymentId = payment.PaymentID,
                UserId = user.Id,
                SignupDate = dateCreated
            };
            _context.SignupMemberships.Add(SignupMembership);
            await _context.SaveChangesAsync();

            // Gửi mail thông báo đơn hàng
            var paymentviewmodel = _cartService.GetTTDatHang();
            var from = new MailAddress("20521068@gm.uit.edu.vn", "GymApp");
            var to = new MailAddress(paymentviewmodel.Email);
            var subject = "Đơn hàng #" + payment.PaymentID;
            var body = "Email body";

            var username = "20521068@gm.uit.edu.vn"; // get from Mailtrap
            var password = "ndomjhbiofjdwegc"; // get from Mailtrap

            var host = "smtp.gmail.com";
            var port = 587;

            var client1 = new SmtpClient(host, port);
            client1.UseDefaultCredentials = false;
            client1.Credentials = new NetworkCredential(username, password);
            client1.EnableSsl = true;

            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

            var strSanPham = "";
            strSanPham += $"<tr>" +
                          $"<td>Gói tập: {membership.Level}</td>" +
                          $"<td>{1}</td>" +
                          $"<td>{String.Format(info, "{0:c}", (membership.Fee))}</td>" +
                          $"</tr>";

            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string contentCustomer = "";
            contentCustomer = System.IO.File.ReadAllText(Path.Combine("Content", "templates", "send2.html"));
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", paymentviewmodel.HoTen);
            contentCustomer = contentCustomer.Replace("{{MaDonHang}}", payment.PaymentID.ToString());
            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
            contentCustomer = contentCustomer.Replace("{{NguyenGia}}", String.Format(info, "{0:c}", membership.Fee));
            contentCustomer = contentCustomer.Replace("{{GiamGia}}", String.Format(info, "{0:c}", 0));
            contentCustomer = contentCustomer.Replace("{{ThanhToan}}", payment.PaymentMode);
            contentCustomer = contentCustomer.Replace("{{TongTien}}", String.Format(info, "{0:c}", membership.Fee));
            contentCustomer = contentCustomer.Replace("{{DiaChi}}", paymentviewmodel.DiaChi);
            contentCustomer = contentCustomer.Replace("{{Email}}", paymentviewmodel.Email);
            contentCustomer = contentCustomer.Replace("{{SoDienThoai}}", paymentviewmodel.SoDienThoai);
            var mail = new MailMessage();
            mail.Subject = subject;
            mail.From = from;
            mail.To.Add(to);
            mail.IsBodyHtml = true;
            mail.Body = contentCustomer;

            client1.Send(mail);

            _cartService.RemoveTTDatHang();

            TempData["SuccessMessage"] = "Đăng ký gói tập: " + membership.Level + " thành công";
            return RedirectToAction("Index");
            //return Json(new { redirectUrl = "/dich-vu/" });
        }

        [Route("/dich-vu/thanh-toan-paypal/thanh-cong", Name = "dichvupaypal")]
        public async Task<IActionResult> CheckoutSuccess()
        {
            if (memId == 0)
            {
                return NotFound();
            }
            var membership = await _context.Memberships.FirstOrDefaultAsync(m => m.MembershipId == memId);
            var user = await _userManager.GetUserAsync(this.User);
            
            var payment = new Payment()
            {
                DateCreated = DateTime.Now,
                UserID = user.Id,
                TotalPrice = membership.Fee,
                PaymentMode = "Paypal",
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            var signupMembership = new SignupMembership()
            {
                PaymentId = payment.PaymentID,
                MembershipId = memId,
                UserId = user.Id,
                SignupDate = DateTime.Now
            };
            _context.SignupMemberships.Add(signupMembership);

            _context.SaveChanges();

            // Gửi mail thông báo đơn hàng
            var paymentviewmodel = _cartService.GetTTDatHang();
            var from = new MailAddress("20521068@gm.uit.edu.vn", "GymApp");
            var to = new MailAddress(paymentviewmodel.Email);
            var subject = "Đơn hàng #" + payment.PaymentID;
            var body = "Email body";

            var username = "20521068@gm.uit.edu.vn"; // get from Mailtrap
            var password = "ndomjhbiofjdwegc"; // get from Mailtrap

            var host = "smtp.gmail.com";
            var port = 587;

            var client1 = new SmtpClient(host, port);
            client1.UseDefaultCredentials = false;
            client1.Credentials = new NetworkCredential(username, password);
            client1.EnableSsl = true;

            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

            var strSanPham = "";
            strSanPham += $"<tr>" +
                          $"<td>Gói tập: {membership.Level}</td>" +
                          $"<td>{1}</td>" +
                          $"<td>{String.Format(info, "{0:c}", (membership.Fee))}</td>" +
                          $"</tr>";

            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string contentCustomer = "";
            contentCustomer = System.IO.File.ReadAllText(Path.Combine("Content", "templates", "send2.html"));
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", paymentviewmodel.HoTen);
            contentCustomer = contentCustomer.Replace("{{MaDonHang}}", payment.PaymentID.ToString());
            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
            contentCustomer = contentCustomer.Replace("{{NguyenGia}}", String.Format(info, "{0:c}", membership.Fee));
            contentCustomer = contentCustomer.Replace("{{GiamGia}}", String.Format(info, "{0:c}", 0));
            contentCustomer = contentCustomer.Replace("{{ThanhToan}}", payment.PaymentMode);
            contentCustomer = contentCustomer.Replace("{{TongTien}}", String.Format(info, "{0:c}", membership.Fee));
            contentCustomer = contentCustomer.Replace("{{DiaChi}}", paymentviewmodel.DiaChi);
            contentCustomer = contentCustomer.Replace("{{Email}}", paymentviewmodel.Email);
            contentCustomer = contentCustomer.Replace("{{SoDienThoai}}", paymentviewmodel.SoDienThoai);
            var mail = new MailMessage();
            mail.Subject = subject;
            mail.From = from;
            mail.To.Add(to);
            mail.IsBodyHtml = true;
            mail.Body = contentCustomer;

            client1.Send(mail);
            
            _cartService.RemoveTTDatHang();

            TempData["SuccessMessage"] = "Đăng ký gói tập: "+ membership.Level + " thành công";
            //TempData["StatusMessage"] = "Đặt hàng thành công";

            return RedirectToAction(nameof(Index));
        }

        [Route("/dich-vu/thanh-toan-paypal/that-bai", Name = "paypalfaill")]
        public IActionResult CheckoutFail()
        {
            var membership = _context.Memberships.FirstOrDefault(m => m.MembershipId == memId);

            if (membership == null)
            {
                return NotFound();
            }

            TempData["ErrorMessage"] = "Đăng ký gói tập: " + membership.Level + " thất bại";
            //TempData["StatusMessage"] = "Đặt hàng thất bại";
            return RedirectToAction(nameof(Index));
        }

    }
}