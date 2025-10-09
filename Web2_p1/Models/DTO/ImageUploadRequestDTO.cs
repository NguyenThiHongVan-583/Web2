namespace Web2_p1.Models.DTO
{
    public class ImageUploadRequestDTO
    {
        public string FileDescription { get; set; }
        public IFormFile File { get; set; }
    }
}
