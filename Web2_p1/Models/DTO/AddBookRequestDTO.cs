using System.ComponentModel.DataAnnotations;

namespace Web2_p1.Models.DTO
{
    public class AddBookRequestDTO
    {
        [Required]
        [MinLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Title không được chứa ký tự đặc biệt.")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }

        [Range(0, 5, ErrorMessage = "Rate must be between 0 and 5")]
        public int? Rate { get; set; }

        public string? Genre { get; set; }

        public string? CoverUrl { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public int PublisherID { get; set; }

        public List<int> AuthorIds { get; set; }
        public int ID { get; set; }
    }
}