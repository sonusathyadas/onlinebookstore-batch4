using BookStore.Shared.Entities;
using BookStoreAPI.Data;
using BookStoreAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreAPI.Tests.Services
{
    [TestFixture]
    public class BookStoreDataServiceTests
    {
        private Mock<BookStoreDbContext> _mockDbContext;
        private BookStoreDataService _service;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = new Mock<BookStoreDbContext>(new DbContextOptions<BookStoreDbContext>());
            _service = new BookStoreDataService(_mockDbContext.Object);
        }

        [Test]
        public async Task DeleteBook_BookExists()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book", Author = "Test Author" };
            _mockDbContext.Setup(db => db.Books.FindAsync(1)).ReturnsAsync(book);
            _mockDbContext.Setup(db => db.Books.Remove(book));
            _mockDbContext.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _service.DeleteBook(1);

            // Assert
            _mockDbContext.Verify(db => db.Books.Remove(book), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public async Task DeleteBook_BookDoesNotExist()
        {
            // Arrange
            _mockDbContext.Setup(db => db.Books.FindAsync(1)).ReturnsAsync((Book)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteBook(1));
            Assert.That(ex.Message, Is.EqualTo("Book not found"));
        }
        [Test]
        public async Task UpdateBook_BookExists()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Updated Book", Author = "Updated Author" };
            var existingBook = new Book { Id = 1, Title = "Original Book", Author = "Original Author" };
            _mockDbContext.Setup(db => db.Books.FindAsync(1)).ReturnsAsync(existingBook);
            _mockDbContext.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _service.UpdateBook(book);

            // Assert
            Assert.That(result.Title, Is.EqualTo(book.Title));
            Assert.That(result.Author, Is.EqualTo(book.Author));
            _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public async Task UpdateBook_BookDoesNotExist()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Updated Book", Author = "Updated Author" };
            _mockDbContext.Setup(db => db.Books.FindAsync(1)).ReturnsAsync((Book)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateBook(book));
            Assert.That(ex.Message, Is.EqualTo("Book not found"));
        }
    }
}
