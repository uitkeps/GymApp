using ProductModel = App.Models.Products.Product;

namespace App.Areas.Product.Models
{
    public class CartItem
    {
        public int quantity { set; get; }
        public ProductModel product { set; get; }
    }
}