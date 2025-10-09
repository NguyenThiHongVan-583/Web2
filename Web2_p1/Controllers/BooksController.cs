using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web2_p1.Data;
using Web2_p1.Models.Domain;
using Web2_p1.Models.DTO;
using Web2_p1.Repositories;
using Microsoft.Extensions.Logging;

namespace Web2_p1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IBookRepository _bookRepository;

        public BooksController(AppDbContext dbContext, IBookRepository bookRepository, IPublisherRepository publisherRepository)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
            _publisherRepository = publisherRepository;
        }

        // CẬP NHẬT PHƯƠNG THỨC GetAll()
        [HttpGet("get-all-books")]
        public IActionResult GetAll(
            [FromQuery] string? filteron,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool isAscending = true, // Giá trị mặc định là true (tăng dần)
            [FromQuery] int pageNumber = 1,      // Giá trị mặc định là 1
            [FromQuery] int pageSize = 1000)     // Giá trị mặc định là 1000 (theo bài tập)
        {
            // Sử dụng repository pattern với tất cả các tham số mới
            var allBooks = _bookRepository.GetAllBooks(
                filteron,
                filterQuery,
                sortBy,
                isAscending,
                pageNumber,
                pageSize);

            return Ok(allBooks);
        }

        [HttpGet]
        [Route("get-book-by-id/{id}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookWithIdDTO = _bookRepository.GetBookById(id);
            return Ok(bookWithIdDTO);
        }
        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            // 1. Kiểm tra validation từ DataAnnotations trong DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 2. Kiểm tra sự tồn tại của PublisherID trong bảng Publishers
            if (!_publisherRepository.PublisherExists(addBookRequestDTO.PublisherID))
            {
                ModelState.AddModelError(nameof(AddBookRequestDTO.PublisherID),
                    "Publisher with this ID does not exist.");
                return BadRequest(ModelState);
            }

            // 3. Nếu hợp lệ, gọi repository để thêm Book
            var createdBook = _bookRepository.AddBook(addBookRequestDTO);

            // 4. Trả về 201 Created cùng location header (chuẩn REST)
            // Fix: Ensure `createdBook` is a domain model or DTO that contains an `ID` property
            return CreatedAtAction(nameof(GetBookById),
                new { id = createdBook.ID }, createdBook);
        }

        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO bookDTO)
        {
            var updateBook = _bookRepository.UpdateBookById(id, bookDTO);
            return Ok(updateBook);
        }
        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var deleteBook = _bookRepository.DeleteBookById(id);
            return Ok(deleteBook);
        }
    }
}