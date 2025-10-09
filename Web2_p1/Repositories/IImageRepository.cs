using Web2_p1.Models.Domain;

namespace Web2_p1.Repositories
{
    public interface IImageRepository
    {
        Task<Image> UploadAsync(Image image);
        Task<IEnumerable<Image>> GetAllAsync();
        Task<Image?> GetByIdAsync(Guid id);

    }
}
