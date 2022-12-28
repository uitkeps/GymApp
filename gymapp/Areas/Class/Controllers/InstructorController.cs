using App.Data;
using App.Models;
using App.Models.Classes;
using App.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace App.Areas.Class.Controllers
{
    [Area("Class")]
    [Authorize(Roles = RoleName.Administrator)]
    [Route("admin/{controller}/{action}/{id?}")]
    public class InstructorController : Controller
    {
        private readonly GymAppDbContext _context;

        public InstructorController(GymAppDbContext context)
        {
            _context = context;
        }

        [TempData] public string StatusMessage { set; get; }

        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize, string keyword)
        {
            //var instructors = _context.Instructors;

            ViewBag.Keyword = keyword;

            IOrderedQueryable<Instructor> instructors;
            if (keyword != null)
            {
                instructors = _context.Instructors.Where(p => p.Name.Contains(keyword) || p.Email.Contains(keyword) || p.Phone.Contains(keyword)).OrderBy(p => p.Name);
            }
            else
            {
                instructors = _context.Instructors.OrderBy(p => p.Name);
            }

            int totalInstructors = await instructors.CountAsync();
            if (pagesize <= 0) pagesize = 5;
            int countPages = (int)Math.Ceiling((double)totalInstructors / pagesize);

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
            ViewBag.totalInstructors = totalInstructors;

            ViewBag.instructorIndex = (currentPage - 1) * pagesize;

            var instructorsInPage = await instructors.Skip((currentPage - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return View(instructorsInPage);
        }

        [HttpGet]
        public IActionResult Create()
        {
            List<SelectListItem> expertiseList = new List<SelectListItem>
            {
                new SelectListItem { Text = "-- Chọn chuyên môn --", Value = "-- Chọn chuyên môn --" },
                new SelectListItem { Text = "Gym", Value = "Gym" },
                new SelectListItem { Text = "Yoga", Value = "Yoga" },
                new SelectListItem { Text = "Boxing", Value = "Boxing" },
                new SelectListItem { Text = "Dance", Value = "Dance" },
                new SelectListItem { Text = "Swimming", Value = "Swimming" },
                new SelectListItem { Text = "Cycling", Value = "Cycling" },
                new SelectListItem { Text = "Zumba", Value = "Zumba" },
                new SelectListItem { Text = "Pilates", Value = "Pilates" },
                new SelectListItem { Text = "Aerobics", Value = "Aerobics" }
            };

            ViewData["expertiseList"] = expertiseList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,DateOfBirth,Gender,Email,Phone,Expertise,Salary")] Instructor instructor)
        {
            List<SelectListItem> expertiseList = new List<SelectListItem>
            {
                new SelectListItem { Text = "-- Chọn chuyên môn --", Value = "-- Chọn chuyên môn --" },
                new SelectListItem { Text = "Gym", Value = "Gym" },
                new SelectListItem { Text = "Yoga", Value = "Yoga" },
                new SelectListItem { Text = "Boxing", Value = "Boxing" },
                new SelectListItem { Text = "Dance", Value = "Dance" },
                new SelectListItem { Text = "Swimming", Value = "Swimming" },
                new SelectListItem { Text = "Cycling", Value = "Cycling" },
                new SelectListItem { Text = "Zumba", Value = "Zumba" },
                new SelectListItem { Text = "Pilates", Value = "Pilates" },
                new SelectListItem { Text = "Aerobics", Value = "Aerobics" }
            };
            ViewData["expertiseList"] = expertiseList;
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(instructor);
                await _context.SaveChangesAsync();

                StatusMessage = "Thêm mới huấn luyện viên thành công";
                return RedirectToAction("Index");
            }

            return View(instructor);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            var instructorEdit = new Instructor()
            {
                Id = (int)id,
                Name = instructor.Name,
                DateOfBirth = instructor.DateOfBirth,
                Gender = instructor.Gender,
                Email = instructor.Email,
                Phone = instructor.Phone,
                Expertise = instructor.Expertise,
                Salary = instructor.Salary
            };

            List<SelectListItem> expertiseList = new List<SelectListItem>();
            expertiseList.Add(new SelectListItem { Text = "Gym", Value = "Gym" });
            expertiseList.Add(new SelectListItem { Text = "Yoga", Value = "Yoga" });
            expertiseList.Add(new SelectListItem { Text = "Boxing", Value = "Boxing" });
            expertiseList.Add(new SelectListItem { Text = "Dance", Value = "Dance" });
            expertiseList.Add(new SelectListItem { Text = "Swimming", Value = "Swimming" });
            expertiseList.Add(new SelectListItem { Text = "Cycling", Value = "Cycling" });
            expertiseList.Add(new SelectListItem { Text = "Zumba", Value = "Zumba" });
            expertiseList.Add(new SelectListItem { Text = "Pilates", Value = "Pilates" });
            expertiseList.Add(new SelectListItem { Text = "Aerobics", Value = "Aerobics" });

            ViewBag.expertiseList = expertiseList;

            return View(instructorEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Name,DateOfBirth,Gender,Email,Phone,Expertise,Salary")] Instructor instructor)
        {
            //if (id != instructor.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    var instructorEdit = await _context.Instructors.FindAsync(instructor.Id);
                    if (instructorEdit == null)
                    {
                        return NotFound();
                    }

                    instructorEdit.Name = instructor.Name;
                    instructorEdit.DateOfBirth = instructor.DateOfBirth;
                    instructorEdit.Gender = instructor.Gender;
                    instructorEdit.Email = instructor.Email;
                    instructorEdit.Phone = instructor.Phone;
                    instructorEdit.Expertise = instructor.Expertise;
                    instructorEdit.Salary = instructor.Salary;

                    _context.Update(instructorEdit);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Vừa cập nhật huấn luyện viên";
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var instructor = _context.Instructors.Find(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        [HttpPost]
        public IActionResult Delete(Instructor instructor)
        {
            try
            {
                var instructorDelete = _context.Instructors.Find(instructor.Id);
                if (instructorDelete == null)
                {
                    return NotFound();
                }

                _context.Instructors.Remove(instructorDelete);
                _context.SaveChanges();

                StatusMessage = "Vừa xóa huấn luyện viên";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                StatusMessage = "Error: Huấn luyện viên này đang có lớp học, không thể xóa";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var instructor = _context.Instructors.Find(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }
    }
}