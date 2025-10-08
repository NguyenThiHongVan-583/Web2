using Web2_p1.Models.Domain;
namespace Web2_p1.Models.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
    public class AuthorNoIdDTO
    {
        public string FullName { get; set; }
    }
}