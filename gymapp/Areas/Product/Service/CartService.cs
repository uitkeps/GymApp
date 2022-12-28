using App.Areas.Product.Models;
using App.Models.Payments;
using gymapp.Areas.Product.Models;
using Newtonsoft.Json;

namespace App.Areas.Product.Service
{
    public class CartService
    {
        public const string CARTKEY = "cart";

        private readonly IHttpContextAccessor _context;

        private readonly HttpContext HttpContext;

        public CartService(IHttpContextAccessor context)
        {
            _context = context;
            HttpContext = context.HttpContext;
        }

        // Lấy cart từ Session (danh sách CartItem)
        public List<CartItem> GetCartItems()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        public void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
            session.Remove("discount");
            session.Remove("ttdathang");
        }

        // Lưu Cart (Danh sách CartItem) vào session
        public void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        // Lưu Discount vào session
        public void SaveDiscountSession(Discount discount)
        {
            var session = HttpContext.Session;
            string jsondiscount = JsonConvert.SerializeObject(discount);
            session.SetString("discount", jsondiscount);
        }

        // Lấy Discount từ session
        public Discount GetDiscount()
        {
            var session = HttpContext.Session;
            string jsondiscount = session.GetString("discount");
            if (jsondiscount != null)
            {
                return JsonConvert.DeserializeObject<Discount>(jsondiscount);
            }
            return null;
        }

        public PaymentViewModel GetTTDatHang()
        {
            var session = HttpContext.Session;
            string ttdathang = session.GetString("ttdathang");
            if (ttdathang != null)
            {
                return JsonConvert.DeserializeObject<PaymentViewModel>(ttdathang);
            }
            return null;
        }

        public void RemoveTTDatHang()
        {
            var session = HttpContext.Session;
            session.Remove("ttdathang");
        }
    }
}