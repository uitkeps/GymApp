using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace App.Models.Classes
{
    [Table("Instructors")]
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên huấn luyện viên")]
        [Required(ErrorMessage = "Tên huấn luyện viên không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Giới tính không được để trống")]
        public string Gender { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email không hợp lệ")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string Phone { get; set; }

        [Display(Name = "Chuyên môn")]
        [Required(ErrorMessage = "Chuyên môn không được để trống")]
        public string Expertise { get; set; }

        [Display(Name = "Mức lương")]
        [Range(0, double.MaxValue, ErrorMessage = "Mức lương không được nhỏ hơn 0")]
        [Required(ErrorMessage = "Mức lương không được để trống")]
        public decimal Salary { get; set; }

        private List<Class>? Classes { get; set; }
    }
}