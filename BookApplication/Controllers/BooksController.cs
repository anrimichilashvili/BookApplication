using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BookApplication.Models;

public class BooksController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BooksController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("BookAPI");
        var books = await client.GetFromJsonAsync<IEnumerable<Book>>("books");

        if (books == null)
        {
            return View(new List<Book>()); 
        }

        return View(books);
    }
    [Route("Books/Create")]
    public async Task<IActionResult> Create()
    {
        var client = _httpClientFactory.CreateClient("BookAPI");

        var authors = await client.GetFromJsonAsync<IEnumerable<Author>>("authors");
        var activeAuthors = authors?.Where(a => !a.IsDeleted).ToList();

        ViewBag.Authors = activeAuthors;

        return View(new BookCreateViewModel());
    }

    [HttpPost]
    [Route("Books/CreateNewBook")]  
    public async Task<IActionResult> Create(BookCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("BookAPI");

            var newBookDto = new
            {
                model.Title,
                model.Description,
                model.ImageUrl,
                model.Rating,
                PublishedDate = DateTime.UtcNow, 
                AuthorIds = model.AuthorIds 
            };

            var response = await client.PostAsJsonAsync("books", newBookDto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to create the book. Please try again.");
            }
        }

        var clientForAuthors = _httpClientFactory.CreateClient("BookAPI");
        var authorsList = await clientForAuthors.GetFromJsonAsync<IEnumerable<Author>>("authors");
        var activeAuthors = authorsList?.Where(a => !a.IsDeleted).ToList();
        ViewBag.Authors = activeAuthors;

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var client = _httpClientFactory.CreateClient("BookAPI");
        var book = await client.GetFromJsonAsync<Book>($"books/{id}");
        return View(book);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("BookAPI");
            var response = await client.PostAsJsonAsync("books", book);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        return View(book);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var client = _httpClientFactory.CreateClient("BookAPI");

        var book = await client.GetFromJsonAsync<Book>($"books/{id}");
        if (book == null)
        {
            return NotFound();
        }

        var authors = await client.GetFromJsonAsync<IEnumerable<Author>>("authors");
        var activeAuthors = authors?.Where(a => !a.IsDeleted).ToList();

        var viewModel = new BookEditViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            ImageUrl = book.ImageUrl,
            Rating = book.Rating,
            AuthorIds = book.Authors.Select(a => a.Id).ToList()
        };

        ViewBag.Authors = activeAuthors;

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BookEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("BookAPI");

            var updateBookDto = new
            {
                model.Id,
                model.Title,
                model.Description,
                model.ImageUrl,
                model.Rating,
                AuthorIds = model.AuthorIds 
            };

            var response = await client.PutAsJsonAsync($"books/{id}", updateBookDto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to update the book. Please try again.");
            }
        }

        var clientForAuthors = _httpClientFactory.CreateClient("BookAPI");
        var authorsList = await clientForAuthors.GetFromJsonAsync<IEnumerable<Author>>("authors");
        var activeAuthors = authorsList?.Where(a => !a.IsDeleted).ToList();
        ViewBag.Authors = activeAuthors;

        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("BookAPI");
        var book = await client.GetFromJsonAsync<Book>($"books/{id}");
        var response = await client.DeleteAsync($"books/{id}");
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Borrow(int id)
    {
        var client = _httpClientFactory.CreateClient("BookAPI");

        var response = await client.PatchAsync($"books/{id}/borrow", null);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        var book = await client.GetFromJsonAsync<Book>($"books/{id}");
        return View("Details", book);
    }

    public async Task<IActionResult> Return(int id)
    {
        var client = _httpClientFactory.CreateClient("BookAPI");

        var response = await client.PatchAsync($"books/{id}/return", null);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        var book = await client.GetFromJsonAsync<Book>($"books/{id}");
        return View("Details", book);
    }
}
