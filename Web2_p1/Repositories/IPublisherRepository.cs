using Web2_p1.Models.Domain;
using Web2_p1.Models.DTO;

namespace Web2_p1.Repositories
{
    public interface IPublisherRepository
    {
        // CẬP NHẬT: Thêm tham số cho Filter, Sort và Pagination
        List<PublisherDTO> GetAllPublishers(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000);

        PublisherNoIdDTO GetPublisherById(int id);
        AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO);
        PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherNoIdDTO);
        // Sửa lỗi chính tả/tên lớp nếu Publishers là tên của Entity Domain Model
        Publishers? DeletePublisherById(int id);
        Publishers? GetPublisherByName(string name);
        bool PublisherExists(int publisherId);
    }
}