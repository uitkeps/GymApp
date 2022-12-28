using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.Classes
{
    [Table("Rooms")]
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Display(Name = "Tên phòng")]
        [Required(ErrorMessage = "Tên phòng không được để trống")]
        public string RoomName { get; set; }

        [Display(Name = "Sức chứa")]
        [Range(0, int.MaxValue, ErrorMessage = "Sức chứa không được nhỏ hơn 0")]
        [Required(ErrorMessage = "Sức chứa không được để trống")]
        public int Capacity { get; set; }

        private List<Class>? Classes { get; set; }
    }
}