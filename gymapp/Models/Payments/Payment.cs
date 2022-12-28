using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Models.Memberships;
using App.Models.Products;

namespace App.Models.Payments
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int PaymentID { set; get; }

        public DateTime DateCreated { set; get; }

        public string? PaymentMode { set; get; }

        [Display(Name = "Khách hàng")]
        public string? UserID { set; get; }

        [Display(Name = "Khách hàng")]
        public AppUser? User { set; get; }

        [Display(Name = "Mã giảm giá")]
        public int? DiscountId { set; get; }

        [Display(Name = "Mã giảm giá")]
        public Discount? Discount { set; get; }

        [Display(Name = "Tổng tiền")]
        public decimal TotalPrice { set; get; }

        private List<PaymentDetail>? PaymentDetails { set; get; }

        private List<SignupMembership>? signupMemberships { set; get; }

        private List<SignupClass>? signupClasses { set; get; }
    }
}