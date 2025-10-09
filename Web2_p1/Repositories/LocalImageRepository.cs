using Microsoft.EntityFrameworkCore;
using Web2_p1.Data;
using Web2_p1.Models.Domain;

namespace Web2_p1.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _dbContext;

        public LocalImageRepository(IWebHostEnvironment environment, AppDbContext dbContext)
        {
            _environment = environment;
            _dbContext = dbContext;
        }

        public async Task<Image> UploadAsync(Image image)
        {
            var localPath = Path.Combine(_environment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localPath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            image.FilePath = localPath;
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image;
        }

        public async Task<IEnumerable<Image>> GetAllAsync() => await _dbContext.Images.ToListAsync();

        public async Task<Image?> GetByIdAsync(Guid id) => await _dbContext.Images.FindAsync(id);
    }
}
