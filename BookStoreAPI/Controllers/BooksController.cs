using BookStore.Shared.Entities;
using BookStoreAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDataService _dataService;

        public BooksController(BookStoreDataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Retrieves all books.
        /// </summary>
        /// <returns>A list of books.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Book>>> GetBooks()
        {
            return await _dataService.GetBooks();
        }

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="book">The book to add.</param>
        /// <returns>The added book.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            var createdBook = await _dataService.AddBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        /// <summary>
        /// Retrieves a book by ID.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>The book with the specified ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _dataService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="book">The updated book details.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            try
            {
                await _dataService.UpdateBook(book);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a book by ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _dataService.DeleteBook(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
