using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.Classes
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Display(Name = "Tên khóa tập")]
        [Required(ErrorMessage = "Tên khóa tập không được để trống")]
        public string ClassTitle { get; set; }

        [Display(Name = "Lịch học")]
        [Required(ErrorMessage = "Lịch học không được để trống")]
        public string ClassDate { get; set; }

        [Display(Name = "Giờ học")]
        [Required(ErrorMessage = "Giờ học không được để trống")]
        public string ClassPeriod { get; set; }

        [Display(Name = "Học phí")]
        [Range(0, double.MaxValue, ErrorMessage = "Học phí không được nhỏ hơn 0")]
        [Required(ErrorMessage = "Học phí không được để trống")]
        public decimal ClassCost { get; set; }

        [Display(Name = "Phòng tập")]
        public int? RoomId { get; set; }

        [Display(Name = "Phòng tập")]
        public Room? Room { get; set; }

        [Display(Name = "Huấn luyện viên")]
        public int? InstructorId { get; set; }

        [Display(Name = "Huấn luyện viên")]
        public Instructor? Instructor { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? PhotoUrl { get; set; }

        [NotMapped]
        [Display(Name = "Hình ảnh")]
        public IFormFile? ImageFile { get; set; }
    }
}