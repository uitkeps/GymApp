using App.Areas.Product.Models;
using App.Areas.Product.Service;
using App.Models;
using App.Models.Payments;
using App.Models.Products;
using BraintreeHttp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal.Core;
using PayPal.v1.Orders;
using System.Net.Mail;
using System.Net;
using PayPalPayments = PayPal.v1.Payments;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using elFinder.NetCore;
using gymapp.Areas.Product.Models;
using PayPal.v1.Payments;
using Newtonsoft.Json;
using System.Globalization;

namespace gymapp.Areas.Product.Controllers
{
    [Area("Product")]
    [Authorize]
    public class ViewProductController : Controller
    {
        private readonly GymAppDbContext _context;
        private readonly CartService _cartService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpcontext;

        private readonly HttpContext HttpContext;
        private readonly string _clientId;
        private readonly string _secretKey;

        public double TyGiaUSD = 23300;

        public ViewProductController(GymAppDbContext context, CartService cartService, UserManager<AppUser> userManager, IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpcontext)
        {
            _context = context;
            _cartService = cartService;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
            _httpcontext = httpcontext;
            HttpContext = httpcontext.HttpContext;
        }

        [Route("/san-pham/")]
        [AllowAnonymous]
        public IActionResult Index([FromQuery(Name = "p")] int currentPage, int pagesize, string sort)
        {
            var products = _context.Products.Include(p => p.Category).Include(p => p.ProductPhotos).AsQueryable();
            ViewBag.sort = sort;
            if (sort == "price-asc")
            {
                products = products.OrderBy(p => (p.Price));
            }
            else if (sort == "price-desc")
            {
                products = products.OrderByDescending(p => p.Price);
            }
            else if (sort == "new")
            {
                products = products.OrderByDescending(p => p.DateUpdated);
            }

            var categories = _context.Categories.ToList();

            int totalProducts = products.Count();
            if (pagesize <= 0) pagesize = 9;
            int countPages = (int)Math.Ceiling((double)totalProducts / pagesize);

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
                })
            };

            var productsInPage = products.Skip((currentPage - 1) * pagesize)
                .Take(pagesize);

            ViewBag.categories = categories;
            ViewBag.pagingModel = pagingModel;
            ViewBag.totalProducts = totalProducts;

            return View(productsInPage.ToList());
        }

        private static PaymentViewModel paymentViewModel = new PaymentViewModel();

        [Route("/san-pham/{danhmuc}")]
        [AllowAnonymous]
        public IActionResult GetProductsByCategory([FromQuery(Name = "p")] int currentPage, int pagesize, string danhmuc, string sort)
        {
            var products = _context.Products.Include(p => p.Category).Include(p => p.ProductPhotos.OrderBy(pt => pt.Id)).Where(s => s.Category.Slug == danhmuc).AsQueryable();
            ViewBag.sort = sort;
            if (sort == "price-asc")
            {
                products = products.OrderBy(p => (p.Price));
            }
            else if (sort == "price-desc")
            {
                products = products.OrderByDescending(p => p.Price);
            }
            else if (sort == "new")
            {
                products = products.OrderByDescending(p => p.DateUpdated);
            }

            var categories = _context.Categories.ToList();

            int totalProducts = products.Count();
            if (pagesize <= 0) pagesize = 9;
            int countPages = (int)Math.Ceiling((double)totalProducts / pagesize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("GetProductsByCategory", new
                {
                    p = pageNumber,
                    pagesize = pagesize
                })
            };

            var productsInPage = products.Skip((currentPage - 1) * pagesize)
                .Take(pagesize);

            if (danhmuc != "")
            {
                var category = _context.Categories.FirstOrDefault(s => s.Slug == danhmuc);

                if (category != null)
                {
                    ViewBag.category = category;
                }
            }
            ViewBag.categories = categories;
            ViewBag.pagingModel = pagingModel;
            ViewBag.totalProducts = totalProducts;

            return View(productsInPage.ToList());
        }

        [Route("/{slug}.html")]
        [AllowAnonymous]
        public IActionResult Detail(string slug)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductPhotos.OrderBy(pt => pt.Id))
                .FirstOrDefault(p => p.Slug == slug);
            if (product == null) return NotFound();

            return View(product);
        }

        /// Thêm sản phẩm vào cart
        [Route("addcart/{productid:int}", Name = "addcart")]
        [AllowAnonymous]
        public IActionResult AddToCart([FromRoute] int productid)
        {
            var product = _context.Products
                .Where(p => p.ProductID == productid)
                .Include(p => p.Category)
                .FirstOrDefault();

            if (product == null)
                return NotFound("Không có sản phẩm");

            // Xử lý đưa vào Cart ...
            var cart = _cartService.GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductID == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { quantity = 1, product = product });
            }

            // Lưu cart vào Session
            _cartService.SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart

            TempData["StatusMessage"] = "Thêm sản phẩm vào giỏ hàng thành công";
            return RedirectToAction(nameof(Cart));
        }

        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = _cartService.GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductID == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
            }

            _cartService.SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        /// xóa item trong cart
        [Route("/removecart/{productid:int}", Name = "removecart")]
        [AllowAnonymous]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = _cartService.GetCartItems();
            var cartitem = cart.Find(p => p.product.ProductID == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            _cartService.SaveCartSession(cart);

            var count = _cartService.GetCartItems().Count();
            return RedirectToAction(nameof(Cart));

            ////return Json(new { success = true, count = count });
        }

        [Route("/gio-hang", Name = "cart")]
        [AllowAnonymous]
        public IActionResult Cart()
        {
            ViewBag.discount = _cartService.GetDiscount();

            return View(_cartService.GetCartItems());
        }

        [Route("/chi-tiet-don-hang", Name = "payment")]
        public IActionResult Payment()
        {
            var cart = _cartService.GetCartItems();

            if (cart.Count == 0)
                return RedirectToAction(nameof(Cart));

            decimal totalOld = 0;
            foreach (var item in cart)
            {
                totalOld += item.product.Price * item.quantity;
            }

            decimal total = totalOld;
            var discountCode = _cartService.GetDiscount();
            int? dicountId = null;

            if (discountCode != null)
            {
                dicountId = discountCode.Id;
                total -= (total * discountCode.Percent / 100);
            }

            var user = _userManager.GetUserAsync(User).Result;

            ViewBag.totalOld = totalOld;
            ViewBag.total = total;
            ViewBag.discountPrice = totalOld - total;
            ViewBag.user = user;

            return View();
        }

        [Route("/xac-nhan-don-hang", Name = "checkout")]
        public async Task<IActionResult> Checkout()
        {
            var cart = _cartService.GetCartItems();

            if (cart.Count == 0)
                return RedirectToAction(nameof(Cart));

            decimal total = 0;
            foreach (var item in cart)
            {
                total += item.product.Price * item.quantity;
            }

            decimal nguyengia = total;

            var discountCode = _cartService.GetDiscount();
            int? dicountId = null;
            decimal giamgia = decimal.Zero;
            if (discountCode != null)
            {
                dicountId = discountCode.Id;
                total -= (total * discountCode.Percent / 100);
                giamgia = nguyengia * discountCode.Percent / 100;
            }

            decimal tonggia = nguyengia - giamgia;

            var user = await _userManager.GetUserAsync(this.User);
            var payment = new App.Models.Payments.Payment()
            {
                DateCreated = DateTime.Now,
                UserID = user.Id,
                DiscountId = dicountId,
                PaymentMode = "Trả sau",
                TotalPrice = total
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                var paymentDetail = new PaymentDetail()
                {
                    PaymentID = payment.PaymentID,
                    ProductID = item.product.ProductID,
                    Quantity = item.quantity
                };
                _context.PaymentDetails.Add(paymentDetail);
            }

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

            var client = new SmtpClient(host, port);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = true;

            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

            var strSanPham = "";
            foreach (var item in cart)
            {
                strSanPham += $"<tr>" +
                              $"<td>{item.product.ProductName}</td>" +
                              $"<td>{item.quantity}</td>" +
                              $"<td>{String.Format(info, "{0:c}", (item.quantity * item.product.Price))}</td>" +
                              $"</tr>";
            }

            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string contentCustomer = "";
            contentCustomer = System.IO.File.ReadAllText(Path.Combine("Content", "templates", "send2.html"));
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", paymentviewmodel.HoTen);
            contentCustomer = contentCustomer.Replace("{{MaDonHang}}", payment.PaymentID.ToString());
            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
            contentCustomer = contentCustomer.Replace("{{NguyenGia}}", String.Format(info, "{0:c}", nguyengia));
            contentCustomer = contentCustomer.Replace("{{GiamGia}}", String.Format(info, "{0:c}", giamgia));
            contentCustomer = contentCustomer.Replace("{{ThanhToan}}", payment.PaymentMode);
            contentCustomer = contentCustomer.Replace("{{TongTien}}", String.Format(info, "{0:c}", tonggia));
            contentCustomer = contentCustomer.Replace("{{DiaChi}}", paymentviewmodel.DiaChi);
            contentCustomer = contentCustomer.Replace("{{Email}}", paymentviewmodel.Email);
            contentCustomer = contentCustomer.Replace("{{SoDienThoai}}", user.PhoneNumber);
            var mail = new MailMessage();
            mail.Subject = subject;
            mail.From = from;
            mail.To.Add(to);
            mail.IsBodyHtml = true;
            mail.Body = contentCustomer;

            client.Send(mail);


            _cartService.ClearCart();
            //TempData["SuccessMessage"] = "Đặt hàng thành công";
            TempData["StatusMessage"] = "Đặt hàng thành công";

            return RedirectToAction(nameof(Cart));
            //return Content("Xác nhận đơn hàng thành công");
        }

        [Route("/kiem-tra-ma-giam-gia/", Name = "checkdiscount")]
        [HttpPost]
        public IActionResult CheckDiscount(string code)
        {
            var discount = _context.Discounts.FirstOrDefault(p => p.Code == code);
            if (discount == null)
                return Json(new { success = false, message = "Mã giảm giá không tồn tại" });

            var user = _userManager.GetUserAsync(this.User).Result;
            var payment = _context.Payments.Where(u => u.UserID == user.Id).ToList();

            if (payment.Any(p => p.DiscountId == discount.Id))
                return Json(new { success = false, message = "Bạn đã sử dụng mã giảm giá này" });

            _cartService.SaveDiscountSession(discount);

            int discountPercent = discount.Percent;

            return Json(new { success = true, message = "Mã giảm giá hợp lệ", discount = discountPercent });
        }

        [Route("/thanh-toan-paypal", Name = "paypal")]
        public async Task<IActionResult> PayPalCheckout()
        {
            var enviroment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(enviroment);

            var cart = _cartService.GetCartItems();

            #region Create Paypal Order

            var itemList = new PayPalPayments.ItemList()
            {
                Items = new List<PayPalPayments.Item>()
            };
            var total = Math.Round(cart.Sum(p => (double)p.product.Price * p.quantity) / TyGiaUSD, 2);
            foreach (var item in cart)
            {
                itemList.Items.Add(new PayPalPayments.Item()
                {
                    Name = item.product.ProductName,
                    Currency = "USD",
                    Price = Math.Round((double)item.product.Price / TyGiaUSD, 2).ToString(),
                    Quantity = item.quantity.ToString(),
                    Sku = "sku",
                    Tax = "0"
                });
            }

            #endregion Create Paypal Order

            var paypalOrderId = DateTime.Now.Ticks;
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var payment = new PayPal.v1.Payments.Payment()
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
                    CancelUrl = $"{hostname}/GioHang/CheckoutFail",
                    ReturnUrl = $"{hostname}/thanh-toan-paypal/thanh-cong"
                },
                Payer = new PayPalPayments.Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PayPalPayments.PaymentCreateRequest request = new PayPalPayments.PaymentCreateRequest();
            request.RequestBody(payment);

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
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return Redirect("/GioHang/CheckoutFail");
            }
        }

        public IActionResult CheckoutFail()
        {
            //return Content("Xác nhận đơn hàng thành công");
            return View();
        }

        [Route("/thanh-toan-paypal/thanh-cong", Name = "paypalsuccess")]
        public async Task<IActionResult> CheckoutSuccess()
        {
            var cart = _cartService.GetCartItems();

            if (cart.Count == 0)
                return RedirectToAction(nameof(Cart));

            var user = await _userManager.GetUserAsync(this.User);
            decimal total = 0;
            foreach (var item in cart)
            {
                total += item.product.Price * item.quantity;
            }

            decimal nguyengia = total;
            var discountCode = _cartService.GetDiscount();
            int? dicountId = null;
            decimal giamgia = Decimal.Zero;
            if (discountCode != null)
            {
                dicountId = discountCode.Id;
                total -= (total * discountCode.Percent / 100);
                giamgia = nguyengia * discountCode.Percent / 100;
            }

            decimal tonggia = nguyengia - giamgia;
            var payment = new App.Models.Payments.Payment()
            {
                DateCreated = DateTime.Now,
                UserID = user.Id,
                TotalPrice = total,
                DiscountId = dicountId,
                PaymentMode = "Paypal",
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                var paymentDetail = new PaymentDetail()
                {
                    PaymentID = payment.PaymentID,
                    ProductID = item.product.ProductID,
                    Quantity = item.quantity
                };
                _context.PaymentDetails.Add(paymentDetail);
            }

            _context.SaveChanges();


            var paymentviewmodel = _cartService.GetTTDatHang();
            var from = new MailAddress("20521068@gm.uit.edu.vn", "GymApp");
            var to = new MailAddress(paymentviewmodel.Email);
            var subject = "Đơn hàng #" + payment.PaymentID;
            var body = "Email body";

            var username = "20521068@gm.uit.edu.vn"; // get from Mailtrap
            var password = "ndomjhbiofjdwegc"; // get from Mailtrap

            var host = "smtp.gmail.com";
            var port = 587;

            var client = new SmtpClient(host, port);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = true;

            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

            var strSanPham = "";
            foreach (var item in cart)
            {
                strSanPham += $"<tr>" +
                              $"<td>{item.product.ProductName}</td>" +
                              $"<td>{item.quantity}</td>" +
                              $"<td>{String.Format(info, "{0:c}", (item.quantity * item.product.Price))}</td>" +
                              $"</tr>";
            }

            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string contentCustomer = "";
            contentCustomer = System.IO.File.ReadAllText(Path.Combine("Content", "templates", "send2.html"));
            contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", paymentviewmodel.HoTen);
            contentCustomer = contentCustomer.Replace("{{MaDonHang}}", payment.PaymentID.ToString());
            contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
            contentCustomer = contentCustomer.Replace("{{NguyenGia}}", String.Format(info, "{0:c}", nguyengia));
            contentCustomer = contentCustomer.Replace("{{GiamGia}}", String.Format(info, "{0:c}", giamgia));
            contentCustomer = contentCustomer.Replace("{{ThanhToan}}", payment.PaymentMode);
            contentCustomer = contentCustomer.Replace("{{TongTien}}", String.Format(info, "{0:c}", tonggia));
            contentCustomer = contentCustomer.Replace("{{DiaChi}}", paymentviewmodel.DiaChi);
            contentCustomer = contentCustomer.Replace("{{Email}}", paymentviewmodel.Email);
            contentCustomer = contentCustomer.Replace("{{SoDienThoai}}", user.PhoneNumber);
            var mail = new MailMessage();
            mail.Subject = subject;
            mail.From = from;
            mail.To.Add(to);
            mail.IsBodyHtml = true;
            mail.Body = contentCustomer;

            client.Send(mail);

            _cartService.ClearCart();
            //TempData["SuccessMessage"] = "Đặt hàng thành công";
            TempData["StatusMessage"] = "Đặt hàng thành công";
            return RedirectToAction(nameof(Cart));
        }

        [Route("/luu-thong-tin-dat-hang", Name = "luutt")]
        public void SaveCartSession(PaymentViewModel ViewModel)
        {
            var session = HttpContext.Session;
            string tt = JsonConvert.SerializeObject(ViewModel);
            session.SetString("ttdathang", tt);
        }
    }
}