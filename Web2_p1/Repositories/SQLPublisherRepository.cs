using System.Linq;
using Web2_p1.Data;
using Web2_p1.Models.Domain;
using Web2_p1.Models.DTO;
using Web2_p1.Repositories;

namespace Web2_p1.Repositories
{
    // Giả định Publishers là tên của Domain Model
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLPublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // CẬP NHẬT: Triển khai GetAllPublishers với Filter, Sort, Pagination và trả về DTO
        public List<PublisherDTO> GetAllPublishers(
            string? filteron = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000)
        {
            // 1. Khởi tạo truy vấn cơ sở dưới dạng IQueryable và ánh xạ sang DTO
            var allPublishers = _dbContext.Publishers.Select(publisher => new PublisherDTO()
            {
                Id = publisher.Id,
                Name = publisher.Name // Giả định Domain Model Publishers có thuộc tính Name
            }).AsQueryable();

            // 2. FILTERING (Lọc theo Name)
            if (string.IsNullOrWhiteSpace(filteron) == false &&
                string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filteron.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    allPublishers = allPublishers.Where(x => x.Name.Contains(filterQuery));
                }
            }

            // 3. SORTING (Sắp xếp theo Name)
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    allPublishers = isAscending
                        ? allPublishers.OrderBy(x => x.Name)
                        : allPublishers.OrderByDescending(x => x.Name);
                }
            }

            // 4. PAGINATION (Phân trang)
            var skipResults = (pageNumber - 1) * pageSize;

            // Thực thi truy vấn với Skip/Take và trả về List
            return allPublishers.Skip(skipResults).Take(pageSize).ToList();
        }

        // Cập nhật: Trả về PublisherNoIdDTO (theo IPublisherRepository)
        public PublisherNoIdDTO GetPublisherById(int id)
        {
            var publisher = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);

            if (publisher == null)
            {
                return null;
            }
            // Ánh xạ sang DTO
            return new PublisherNoIdDTO() { Name = publisher.Name };
        }

        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO)
        {
            // Sử dụng Domain Model Publishers
            var publisherDomainModel = new Publishers
            {
                Name = addPublisherRequestDTO.Name
            };
            _dbContext.Publishers.Add(publisherDomainModel);
            _dbContext.SaveChanges();
            return addPublisherRequestDTO;
        }

        // Cập nhật: Trả về PublisherNoIdDTO (theo IPublisherRepository)
        public PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherNoIdDTO)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);

            if (publisherDomain == null)
            {
                return null;
            }
            publisherDomain.Name = publisherNoIdDTO.Name;
            _dbContext.SaveChanges();

            // Ánh xạ lại Domain Model đã cập nhật sang DTO
            return new PublisherNoIdDTO() { Name = publisherDomain.Name };
        }

        // Giữ nguyên kiểu trả về Publishers? (theo IPublisherRepository)
        public Publishers? DeletePublisherById(int id)
        {
            var publisherDomain = _dbContext.Publishers.FirstOrDefault(n => n.Id == id);
            if (publisherDomain != null)
            {
                _dbContext.Publishers.Remove(publisherDomain);
                _dbContext.SaveChanges();
            }
            return publisherDomain;
        }

        // THÊM: Phương thức GetPublisherByName (theo IPublisherRepository)
        public Publishers? GetPublisherByName(string name)
        {
            return _dbContext.Publishers.FirstOrDefault(n => n.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // THÊM: Phương thức PublisherExists (theo IPublisherRepository)
        public bool PublisherExists(int publisherId)
        {
            return _dbContext.Publishers.Any(n => n.Id == publisherId);
        }
    }
}