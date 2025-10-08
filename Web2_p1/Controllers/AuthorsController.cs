using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web2_p1.Data;
using Web2_p1.Models.Domain;
using Web2_p1.Models.DTO;
using Web2_p1.Repositories;

namespace Web2_p1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet("get-all-author")]
        public IActionResult GetAllAuthor(
            [FromQuery] string? filteron,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool isAscending = true,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            // Sử dụng repository pattern với tham số Filter, Sort, Pagination
            var allAuthors = _authorRepository.GetAllAuthors(
                filteron,
                filterQuery,
                sortBy,
                isAscending,
                pageNumber,
                pageSize);

            return Ok(allAuthors);
        }

        [HttpGet]
        [Route("get-author-by-id/{id}")]
        public IActionResult GetAuthorById([FromRoute] int id)
        {
            var author = _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost("add-author")]
        public IActionResult AddAuthor([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorAdd = _authorRepository.AddAuthor(addAuthorRequestDTO);
            return Ok(authorAdd);
        }

        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateAuthorById(int id, [FromBody] AuthorNoIdDTO authorNoIdDTO)
        {
            var updateAuthor = _authorRepository.UpdateAuthorById(id, authorNoIdDTO);
            if (updateAuthor == null)
            {
                return NotFound();
            }
            return Ok(updateAuthor);
        }

        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteAuthorById(int id)
        {
            var deleteAuthor = _authorRepository.DeleteAuthorById(id);
            if (deleteAuthor == null)
            {
                return NotFound();
            }
            return Ok(deleteAuthor);
        }
    }
}