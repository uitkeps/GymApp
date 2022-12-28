using App.Data;
using App.Models;
using App.Models.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Class.Controllers
{
    [Area("Class")]
    [Authorize(Roles = RoleName.Administrator)]
    [Route("/admin/room/{action=Index}/{id?}")]
    public class RoomController : Controller
    {
        private readonly GymAppDbContext _context;

        public RoomController(GymAppDbContext context)
        {
            _context = context;
        }

        [TempData] public string StatusMessage { set; get; }

        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize, string keyword)
        {
            ViewBag.Keyword = keyword;

            IQueryable<Room> rooms;

            if (keyword != null)
            {
                rooms = _context.Rooms
                    .Where(c => c.RoomName.Contains(keyword));
            }
            else
            {
                rooms = _context.Rooms;
            }

            int totalRooms = await rooms.CountAsync();
            if (pagesize <= 0) pagesize = 5;
            int countPages = (int)Math.Ceiling((double)totalRooms / pagesize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    p = pageNumber,
                    pagesize = pagesize,
                    keyword = keyword
                }) ?? string.Empty
            };

            ViewBag.pagingModel = pagingModel;
            ViewBag.totalRooms = totalRooms;

            ViewBag.roomIndex = (currentPage - 1) * pagesize;

            var roomrsInPage = await rooms.Skip((currentPage - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return View(roomrsInPage);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("RoomName,Capacity")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();

                StatusMessage = "Thêm phòng tập mới thành công";
                return RedirectToAction("Index");
            }

            return View(room);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            var roomEdit = new Room
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                Capacity = room.Capacity
            };

            return View(roomEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("RoomId,RoomName,Capacity")] Room room)
        {
            if (room.RoomId != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var roomEdit = await _context.Rooms.FindAsync(room.RoomId);
                if (roomEdit == null)
                {
                    return NotFound();
                }

                roomEdit.RoomName = room.RoomName;
                roomEdit.Capacity = room.Capacity;

                await _context.SaveChangesAsync();

                StatusMessage = "Cập nhật phòng tập: " + room.RoomName + " thành công";
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            try
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();

                StatusMessage = "Xóa phòng tập: "+ room.RoomName +" thành công";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                StatusMessage = "Error: Phòng tập này đang được sử dụng, không thể xóa";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
    }
}