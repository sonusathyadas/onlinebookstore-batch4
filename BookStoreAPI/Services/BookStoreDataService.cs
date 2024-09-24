using BookStore.Shared.Entities;
using BookStoreAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services
{
    /// <summary>
    /// Provides methods for retrieving, adding, updating, and deleting book data from the database using BookStoreDbContext.
    /// </summary>
    public class BookStoreDataService
    {
        private readonly BookStoreDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreDataService"/> class.
        /// </summary>
        /// <param name="dbContext">The BookStoreDbContext instance.</param>
        public BookStoreDataService(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all books from the database.
        /// </summary>
        /// <returns>A list of books.</returns>
        public async Task<List<Book>> GetBooks()
        {
            return await _dbContext.Books.ToListAsync();
        }

        /// <summary>
        /// Retrieves a book by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the book.</param>
        /// <returns>The book with the specified ID, or null if not found.</returns>
        public async Task<Book?> GetBookById(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            return book ?? null;
        }

        /// <summary>
        /// Searches for books in the database based on a search term.
        /// </summary>
        /// <param name="searchTerm">The search term to match against book titles and authors.</param>
        /// <returns>A list of books matching the search term.</returns>
        public async Task<List<Book>> SearchBooks(string searchTerm)
        {
            return await _dbContext.Books
                .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new book to the database.
        /// </summary>
        /// <param name="book">The book to add.</param>
        /// <returns>The added book.</returns>
        public async Task<Book> AddBook(Book book)
        {
            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();
            return book;
        }

        /// <summary>
        /// Updates an existing book in the database.
        /// </summary>
        /// <param name="book">The updated book.</param>
        /// <returns>The updated book.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the book with the specified ID is not found.</exception>
        public async Task<Book> UpdateBook(Book book)
        {
            var existingBook = await _dbContext.Books.FindAsync(book.Id);
            if (existingBook == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            _dbContext.Entry(existingBook).CurrentValues.SetValues(book);
            await _dbContext.SaveChangesAsync();
            return existingBook;
        }

        /// <summary>
        /// Deletes a book from the database.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the book with the specified ID is not found.</exception>
        public async Task DeleteBook(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
        }

        public List<string> GetDistinctAuthors(List<Book> books)
        {
            // Using a HashSet to store unique author names
            HashSet<string> authors = new HashSet<string>();
            foreach (var book in books)
            {
                authors.Add(book.Author);
            }
            return authors.ToList();
        }
    }
}
