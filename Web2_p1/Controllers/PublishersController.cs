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
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublishersController(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        [HttpGet("get-all-publisher")]
        public IActionResult GetAllPublisher(
            // THÊM THAM SỐ TỪ QUERY STRING
            [FromQuery] string? filteron,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool isAscending = true,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 1000)
        {
            // Gọi Repository với 6 tham số
            var allPublishers = _publisherRepository.GetAllPublishers(
                filteron,
                filterQuery,
                sortBy,
                isAscending,
                pageNumber,
                pageSize);

            return Ok(allPublishers);
        }

        [HttpGet]
        [Route("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById([FromRoute] int id)
        {
            var publisher = _publisherRepository.GetPublisherById(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return Ok(publisher);
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] AddPublisherRequestDTO addPublisherRequestDTO)
        {
            // Kiểm tra tên Publisher có tồn tại chưa
            var existingPublisher = _publisherRepository.GetPublisherByName(addPublisherRequestDTO.Name);
            if (existingPublisher != null)
            {
                // Nếu tên đã tồn tại, trả về lỗi BadRequest
                ModelState.AddModelError("Name", "Tên nhà xuất bản đã tồn tại.");
                return BadRequest(ModelState);
            }

            // Nếu tên chưa tồn tại, tiến hành thêm mới
            var publisherAdd = _publisherRepository.AddPublisher(addPublisherRequestDTO);
            return Ok(publisherAdd);
        }

        [HttpPut("update-publisher-by-id/{id}")]
        public IActionResult UpdatePublisherById(int id, [FromBody] PublisherNoIdDTO publisherNoIdDTO)
        {
            var updatePublisher = _publisherRepository.UpdatePublisherById(id, publisherNoIdDTO);
            if (updatePublisher == null)
            {
                return NotFound();
            }
            return Ok(updatePublisher);
        }

        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            var deletePublisher = _publisherRepository.DeletePublisherById(id);
            if (deletePublisher == null)
            {
                return NotFound();
            }
            return Ok(deletePublisher);
        }
    }
}