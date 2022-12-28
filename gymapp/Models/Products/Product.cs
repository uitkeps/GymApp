using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace App.Models.Products
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductID { set; get; }

        [Required(ErrorMessage = "Phải có tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        public string ProductName { set; get; }

        [Display(Name = "Mô tả ngắn")]
        public string Description { set; get; }

        [Display(Name = "Chi tiết sản phẩm")]
        public string Content { set; get; }

        [Display(Name = "Ngày tạo")]
        public DateTime? DateCreated { set; get; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? DateUpdated { set; get; }

        [Display(Name = "Người thêm")]
        public string? AuthorId { set; get; }

        [ForeignKey("AuthorId")]
        [Display(Name = "Người thêm")]
        public AppUser? Author { set; get; }

        [Display(Name = "Giá sản phẩm")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá sản phẩm không được nhỏ hơn 0")]
        public decimal Price { set; get; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {2} đến {1}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Url hiển thị")]
        public string? Slug { set; get; }

        [Display(Name = "Danh mục sản phẩm")]
        public int CategoryID { set; get; }

        public Category? Category { set; get; }

        public List<ProductPhoto>? ProductPhotos { set; get; }
    }
}