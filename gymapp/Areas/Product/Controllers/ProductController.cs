using App.Data;
using App.Models;
using App.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;
using System;
using ProductModel = App.Models.Products.Product;
using Microsoft.Extensions.Hosting;
using App.Models.Products;

namespace App.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("admin/product/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class ProductController : Controller
    {
        private readonly GymAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(GymAppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string? StatusMessage { get; set; }

        // GET: Product/Product
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize, string keyword)
        {
            ViewBag.Keyword = keyword;

            IOrderedQueryable<ProductModel> products;
            if (keyword != null)
            {
                products = _context.Products
                    .Include(p => p.Author)
                    .Where(p => p.ProductName.Contains(keyword))
                    .OrderByDescending(p => p.DateUpdated);
            }
            else
            {
                products = _context.Products
                    .Include(p => p.Author)
                    .OrderByDescending(p => p.DateUpdated);
            }

            int totalProducts = await products.CountAsync();
            if (pagesize <= 0) pagesize = 5;
            int countPages = (int)Math.Ceiling((double)totalProducts / pagesize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                keyword = keyword,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    p = pageNumber,
                    pagesize = pagesize,
                    keyword = keyword
                }) ?? string.Empty
            };

            ViewBag.pagingModel = pagingModel;
            ViewBag.totalPosts = totalProducts;

            ViewBag.postIndex = (currentPage - 1) * pagesize;

            var productsInPage = await products.Skip((currentPage - 1) * pagesize)
                             .Take(pagesize)
                             .Include(pc => pc.Category)
                             .ToListAsync();

            return View(productsInPage);
        }

        // GET: Prodcut/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Author)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductName,Description,Content,Price,Slug,CategoryID")] ProductModel product)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new SelectList(categories, "Id", "Title");

            if (product.Slug == null)
            {
                product.Slug = AppUtilities.GenerateSlug(product.ProductName);
            }

            if (_context.Products.Any(p => p.Slug == product.Slug))
            {
                ModelState.AddModelError("Slug", "Slug đã tồn tại");
                return View(product);
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(this.User);
                product.DateCreated = product.DateUpdated = DateTime.Now;
                product.AuthorId = user.Id;
                _context.Add(product);

                await _context.SaveChangesAsync();
                StatusMessage = "Vừa thêm mới sản phẩm: " + product.ProductName;
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(c => c.Category).FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");

            var productEdit = new ProductModel()
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Content = product.Content,
                Description = product.Description,
                Price = product.Price,
                Slug = product.Slug,
                CategoryID = product.CategoryID
            };

            return View(productEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,Description,Content,Price,Slug,CategoryID")] ProductModel product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (product.Slug == null)
            {
                product.Slug = AppUtilities.GenerateSlug(product.ProductName);
            }

            ViewData["categories"] = new SelectList(await _context.Categories.ToListAsync(), "Id", "Title");

            if (ModelState.IsValid)
            {
                try
                {
                    var productUpdate = await _context.Products.FindAsync(id);
                    if (productUpdate == null)
                    {
                        return NotFound();
                    }

                    productUpdate.ProductName = product.ProductName;
                    productUpdate.Description = product.Description;
                    productUpdate.Content = product.Content;
                    productUpdate.DateUpdated = DateTime.Now;
                    productUpdate.Price = product.Price;
                    productUpdate.Slug = product.Slug;
                    productUpdate.CategoryID = product.CategoryID;

                    _context.Update(productUpdate);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Vừa cập nhật sản phẩm: " + product.ProductName;
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", product.AuthorId);
            return View(product);
        }

        // GET: Blog/Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Blog/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                StatusMessage = "Bạn vừa xóa sản phẩm: " + product.ProductName;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                StatusMessage = "Error: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

        public class UploadOneFile
        {
            [Required(ErrorMessage = "Phải chọn file upload")]
            [DataType(DataType.Upload)]
            [FileExtensions(Extensions = "png,jpg,jpeg,gif")]
            [Display(Name = "Chọn file upload")]
            public IFormFile FileUpload { get; set; }
        }

        // Product Photo
        [HttpGet]
        public IActionResult UploadPhoto(int id)
        {
            var product = _context.Products.Where(e => e.ProductID == id)
                            .Include(p => p.ProductPhotos)
                            .FirstOrDefault();
            if (product == null)
            {
                return NotFound("Không có sản phẩm");
            }
            ViewData["product"] = product;
            return View(new UploadOneFile());
        }

        [HttpPost, ActionName("UploadPhoto")]
        public async Task<IActionResult> UploadPhotoAsync(int id, [Bind("FileUpload")] UploadOneFile f)
        {
            var product = _context.Products.Where(e => e.ProductID == id)
                .Include(p => p.ProductPhotos)
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound("Không có sản phẩm");
            }
            ViewData["product"] = product;

            if (f != null)
            {
                var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                            + Path.GetExtension(f.FileUpload.FileName);

                var file = Path.Combine("Uploads", "Products", file1);

                using (var filestream = new FileStream(file, FileMode.Create))
                {
                    await f.FileUpload.CopyToAsync(filestream);
                }

                _context.Add(new ProductPhoto()
                {
                    ProductID = product.ProductID,
                    FileName = file1
                });

                await _context.SaveChangesAsync();
            }

            return View(f);
        }

        [HttpPost]
        public IActionResult ListPhotos(int id)
        {
            var product = _context.Products.Where(e => e.ProductID == id)
                .Include(p => p.ProductPhotos)
                .FirstOrDefault();

            if (product == null)
            {
                return Json(
                    new
                    {
                        success = 0,
                        message = "Product not found",
                    }
                );
            }

            var listphotos = product.ProductPhotos.Select(photo => new
            {
                id = photo.Id,
                path = "/contents/Products/" + photo.FileName
            });

            return Json(
                new
                {
                    success = 1,
                    photos = listphotos
                }
            );
        }

        [HttpPost]
        public IActionResult DeletePhoto(int id)
        {
            var photo = _context.ProductPhotos.Where(p => p.Id == id).FirstOrDefault();
            if (photo != null)
            {
                _context.Remove(photo);
                _context.SaveChanges();

                var filename = "Uploads/Products/" + photo.FileName;
                System.IO.File.Delete(filename);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhotoApi(int id, [Bind("FileUpload")] UploadOneFile f)
        {
            var product = _context.Products.Where(e => e.ProductID == id)
                .Include(p => p.ProductPhotos)
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound("Không có sản phẩm");
            }

            if (f != null)
            {
                var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                            + Path.GetExtension(f.FileUpload.FileName);

                var file = Path.Combine("Uploads", "Products", file1);

                using (var filestream = new FileStream(file, FileMode.Create))
                {
                    await f.FileUpload.CopyToAsync(filestream);
                }

                _context.Add(new ProductPhoto()
                {
                    ProductID = product.ProductID,
                    FileName = file1
                });

                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}