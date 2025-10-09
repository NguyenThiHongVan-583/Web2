using System.ComponentModel.DataAnnotations;

namespace Web2_p1.Models.DTO
{
    public class AddBookRequestDTO
    {
        // Sử dụng = null! để khắc phục CS8618 và yêu cầu [Required]
        [Required]
        [MinLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Title không được chứa ký tự đặc biệt.")]
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }

        [Range(0, 5, ErrorMessage = "Rate must be between 0 and 5")]
        public int? Rate { get; set; }

        public string Genre { get; set; } = null!;

        public string CoverUrl { get; set; } = null!;

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public int PublisherID { get; set; }

        // Khắc phục lỗi CS8618: Khởi tạo mặc định cho List
        [Required]
        public List<int> AuthorIds { get; set; } = new List<int>();
        public object ID { get; internal set; }

        // Đã xóa: public int ID { get; set; }
    }
}