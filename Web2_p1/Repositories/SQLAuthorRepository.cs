using System.Linq;
using Web2_p1.Data;
using Web2_p1.Models.Domain;
using Web2_p1.Models.DTO;
using Web2_p1.Repositories;

namespace Web2_p1.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLAuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // CẬP NHẬT: Phương thức GetAllAuthors() - Thêm 6 tham số Filter, Sort, Pagination
        public List<AuthorDTO> GetAllAuthors(
            string? filteron = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000)
        {
            // 1. Khởi tạo truy vấn dưới dạng IQueryable
            var allAuthors = _dbContext.Authors.Select(author => new AuthorDTO()
            {
                Id = author.Id,
                FullName = author.FullName
            }).AsQueryable();

            // 2. FILTERING (Lọc)
            if (string.IsNullOrWhiteSpace(filteron) == false &&
                string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                // Giả định lọc theo FullName
                if (filteron.Equals("fullname", StringComparison.OrdinalIgnoreCase))
                {
                    allAuthors = allAuthors.Where(x => x.FullName.Contains(filterQuery));
                }
            }

            // 3. SORTING (Sắp xếp)
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                // Giả định sắp xếp theo FullName
                if (sortBy.Equals("fullname", StringComparison.OrdinalIgnoreCase))
                {
                    allAuthors = isAscending
                        ? allAuthors.OrderBy(x => x.FullName)
                        : allAuthors.OrderByDescending(x => x.FullName);
                }
            }

            // 4. PAGINATION (Phân trang)
            var skipResults = (pageNumber - 1) * pageSize;

            // Thực thi truy vấn với Skip/Take và trả về List
            return allAuthors.Skip(skipResults).Take(pageSize).ToList();
        }

        // Phương thức GetAuthorById(int id) - Lấy một tác giả theo ID
        public AuthorNoIdDTO GetAuthorById(int id)
        {
            var author = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (author == null)
            {
                return null;
            }
            // Ánh xạ từ Domain Model sang DTO
            var authorNoIdDTO = new AuthorNoIdDTO()
            {
                FullName = author.FullName
            };
            return authorNoIdDTO;
        }

        // Phương thức AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO) - Thêm một tác giả mới
        public AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO)
        {
            // Ánh xạ từ DTO sang Domain Model
            var authorDomainModel = new Author
            {
                FullName = addAuthorRequestDTO.FullName
            };
            // Thêm tác giả vào CSDL
            _dbContext.Authors.Add(authorDomainModel);
            _dbContext.SaveChanges();
            return addAuthorRequestDTO;
        }

        // Phương thức UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO) - Cập nhật thông tin tác giả
        public AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (authorDomain == null)
            {
                return null;
            }
            authorDomain.FullName = authorNoIdDTO.FullName;
            _dbContext.SaveChanges();

            // Ánh xạ lại Domain Model đã cập nhật sang DTO
            var updatedAuthorDTO = new AuthorNoIdDTO()
            {
                FullName = authorDomain.FullName
            };
            return updatedAuthorDTO;
        }

        // Phương thức DeleteAuthorById(int id) - Xóa một tác giả
        public Author? DeleteAuthorById(int id)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (authorDomain != null)
            {
                _dbContext.Authors.Remove(authorDomain);
                _dbContext.SaveChanges();
            }
            return authorDomain;
        }
    }
}