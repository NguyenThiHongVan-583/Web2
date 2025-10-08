using Web2_p1.Models.Domain;
using Web2_p1.Models.DTO;

namespace Web2_p1.Repositories
{
    public interface IAuthorRepository
    {
        // CẬP NHẬT: Thêm 6 tham số cho Filter, Sort và Pagination
        List<AuthorDTO> GetAllAuthors(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000);

        AuthorNoIdDTO GetAuthorById(int id);
        AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO);
        AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO);
        Author? DeleteAuthorById(int id);
    }
}