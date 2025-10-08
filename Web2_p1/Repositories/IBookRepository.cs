using Web2_p1.Models.Domain;
using Web2_p1.Models.DTO;

namespace Web2_p1.Repositories
{
    public interface IBookRepository
    {
        // CẬP NHẬT PHƯƠNG THỨC GetAllBooks để hỗ trợ Filter, Sort và Pagination
        List<BookWithAuthorAndPublisherDTO> GetAllBooks(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true, // Mặc định là sắp xếp tăng dần
            int pageNumber = 1,      // Mặc định là trang 1
            int pageSize = 1000);    // Mặc định là 1000 bản ghi/trang [cite: 126]

        BookWithAuthorAndPublisherDTO GetBookById(int id);
        AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO);
        AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO);
        Book? DeleteBookById(int id);
    }
}