using BookApplication.Models;
using Microsoft.AspNetCore.Mvc;

public class AuthorsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthorsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("BookAPI");
        var authors = await client.GetFromJsonAsync<IEnumerable<Author>>("authors");
        return View(authors);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AuthorUpdate author)
    {
        if (id != author.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("BookAPI");
            var response = await client.PutAsJsonAsync($"authors/{id}", author);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index)); 
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to update the author. Please try again.");
            }
        }
        return View(author);
    }
    public async Task<IActionResult> Details(int id)
    {
        var client = _httpClientFactory.CreateClient("BookAPI");
        var author = await client.GetFromJsonAsync<Author>($"authors/{id}");
        if (author == null)
        {
            return NotFound();
        }
        return View(author);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var client = _httpClientFactory.CreateClient("BookAPI");
        var author = await client.GetFromJsonAsync<Author>($"authors/{id}");
        if (author == null)
        {
            return NotFound();
        }
        return View(author);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("BookAPI");
        var author = await client.GetFromJsonAsync<Author>($"authors/{id}");
        var response = await client.DeleteAsync($"authors/{id}");
        return RedirectToAction(nameof(Index));
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AuthorCreate author)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("BookAPI");
            var response = await client.PostAsJsonAsync("authors", author);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to create the author. Please try again.");
            }
        }

        return View(author);
    }
}
