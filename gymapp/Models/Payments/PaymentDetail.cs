using App.Models.Products;

namespace App.Models.Payments
{
    public class PaymentDetail
    {
        public int PaymentID { get; set; }

        public Payment? Payment { get; set; }

        public int ProductID { get; set; }

        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}