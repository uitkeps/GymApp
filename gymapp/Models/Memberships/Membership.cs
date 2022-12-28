using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.Memberships
{
    [Table("Memberships")]
    public class Membership
    {
        [Key]
        public int MembershipId { get; set; }

        [Display(Name = "Tên gói tập")]
        [Required(ErrorMessage = "Tên gói tập không được để trống")]
        public string Level { get; set; }

        [Display(Name = "Giá")]
        [Range(0, 1000000000, ErrorMessage = "Giá không hợp lệ")]
        [Required(ErrorMessage = "Giá gói tập không được để trống")]
        public decimal Fee { get; set; }

        [Display(Name = "Thời hạn")]
        [Range(1, 12, ErrorMessage = "Thời hạn phải từ 1 đến 12 tháng")]
        [Required(ErrorMessage = "Thời hạn gói tập không được để trống")]
        public int Duration { get; set; }

        [Display(Name = "Số giờ tập")]
        [Range(1, 24, ErrorMessage = "Vui lòng nhập số giờ tập lớn hơn 0 và nhỏ hơn 24")]
        [Required(ErrorMessage = "Số giờ tập không được để trống")]
        public int Hours { get; set; }

        [Display(Name = "Đặc quyền")]
        [Required(ErrorMessage = "Đặc quyền không được để trống")]
        public string Bonus { get; set; }
    }
}