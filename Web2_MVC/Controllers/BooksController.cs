using Microsoft.AspNetCore.Mvc;
using Web2_MVC.Models;

namespace Web2_MVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BooksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<BookDTO> books = new();
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7245/api/Books/get-all-books");
                response.EnsureSuccessStatusCode();

                books.AddRange(await response.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(books);
        }
    }
}
