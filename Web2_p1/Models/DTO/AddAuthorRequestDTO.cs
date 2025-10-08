using System.ComponentModel.DataAnnotations;

namespace Web2_p1.Models.DTO
{
    public class AddAuthorRequestDTO
    {
        [Required]
        [MinLength(3)]
        public string FullName { set; get; }
    }
}