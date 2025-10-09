using Microsoft.AspNetCore.Mvc;
using Web2_p1.Models.Domain;
using Web2_p1.Models.DTO;
using Web2_p1.Repositories;

namespace Web2_p1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(IImageRepository imageRepository, ILogger<ImagesController> logger)
        {
            _imageRepository = imageRepository;
            _logger = logger;
        }

        #region Upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request.File);

            var image = new Image
            {
                FileName = Path.GetFileNameWithoutExtension(request.File.FileName),
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileDescription = request.FileDescription,
                File = request.File
            };

            var uploadedImage = await _imageRepository.UploadAsync(image);
            return Ok(uploadedImage);
        }
        #endregion

        #region Get All
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll() => Ok(await _imageRepository.GetAllAsync());
        #endregion

        #region Download
        [HttpGet("download/{id:Guid}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var image = await _imageRepository.GetByIdAsync(id);
            if (image == null)
                return NotFound();

            var bytes = await System.IO.File.ReadAllBytesAsync(image.FilePath);
            return File(bytes, "application/octet-stream", image.FileName + image.FileExtension);
        }
        #endregion

        private void ValidateFileUpload(IFormFile file)
        {
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowed.Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new Exception(" Chỉ cho phép file ảnh (.jpg, .png, .gif)");
        }
    }
}
