using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web2_p1.Models.DTO;
using Web2_p1.Repositories;
using Microsoft.Extensions.Logging;

namespace Web2_p1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Có thể đặt trên từng action thay vì toàn bộ controller
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AuthorsController> _logger; // Thêm ILogger

        // Cập nhật Constructor để tiêm IAuthorRepository và ILogger
        public AuthorsController(IAuthorRepository authorRepository, ILogger<AuthorsController> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        // GET: /api/Authors/get-all-authors?filterOn=FullName&...
        [HttpGet("get-all-authors")] // Đổi tên endpoint từ get-all-author thành get-all-authors cho nhất quán
        [Authorize(Roles = "Read")] // Áp dụng phân quyền Read
        public IActionResult GetAllAuthors( // Đổi tên phương thức từ GetAllAuthor thành GetAllAuthors cho nhất quán
            [FromQuery] string? filteron,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool isAscending = true,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            _logger.LogInformation($"GetAllAuthors action method invoked with: {Request.QueryString}");

            // Truyền đủ 6 tham số xuống Repository
            var allAuthors = _authorRepository.GetAllAuthors(
                filteron,
                filterQuery,
                sortBy,
                isAscending,
                pageNumber,
                pageSize);

            _logger.LogInformation($"Returned {allAuthors.Count} authors.");
            return Ok(allAuthors);
        }

        // GET: /api/Authors/get-author-by-id/5
        [HttpGet("get-author-by-id/{id}")]
        [Authorize(Roles = "Read")] // Áp dụng phân quyền Read
        public IActionResult GetAuthorById([FromRoute] int id)
        {
            var author = _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                _logger.LogWarning($"Author with ID {id} not found.");
                return NotFound();
            }
            return Ok(author);
        }

        // POST: /api/Authors/add-author
        [HttpPost("add-author")]
        [Authorize(Roles = "Write")] // Áp dụng phân quyền Write
        public IActionResult AddAuthor([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorAdd = _authorRepository.AddAuthor(addAuthorRequestDTO);
            _logger.LogInformation($"New Author added: {authorAdd.FullName}");
            return Ok(authorAdd);
        }

        // PUT: /api/Authors/update-author-by-id/5
        [HttpPut("update-author-by-id/{id}")]
        [Authorize(Roles = "Write")] // Áp dụng phân quyền Write
        public IActionResult UpdateAuthorById(int id, [FromBody] AuthorNoIdDTO authorNoIdDTO)
        {
            var updateAuthor = _authorRepository.UpdateAuthorById(id, authorNoIdDTO);
            if (updateAuthor == null)
            {
                _logger.LogWarning($"Attempted to update non-existent Author with ID {id}.");
                return NotFound();
            }
            _logger.LogInformation($"Author ID {id} updated to: {updateAuthor.FullName}");
            return Ok(updateAuthor);
        }

        // DELETE: /api/Authors/delete-author-by-id/5
        [HttpDelete("delete-author-by-id/{id}")]
        [Authorize(Roles = "Write")] // Áp dụng phân quyền Write
        public IActionResult DeleteAuthorById(int id)
        {
            var deleteAuthor = _authorRepository.DeleteAuthorById(id);
            if (deleteAuthor == null)
            {
                _logger.LogWarning($"Attempted to delete non-existent Author with ID {id}.");
                return NotFound();
            }
            _logger.LogInformation($"Author ID {id} deleted.");
            return Ok(deleteAuthor);
        }
    }
}