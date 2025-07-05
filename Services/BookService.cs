using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookManager.Data;
using BookManager.Model;
using System.Diagnostics;

namespace BookManager.Services
{
    public class BookService : IDisposable
    {
        private BookStoreContext _context;

        public BookService()
        {
            try
            {
                _context = new BookStoreContext();
                Debug.WriteLine("BookService: Context created successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"BookService constructor error: {ex.Message}");
                throw;
            }
        }

        // Test database connection
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TestConnectionAsync error: {ex.Message}");
                return false;
            }
        }

        // Enhanced CRUD Operations for Books
        public async Task<List<Books>> GetAllBooksAsync()
        {
            try
            {
                Debug.WriteLine("GetAllBooksAsync: Starting...");
                var books = await _context.Books.ToListAsync();
                Debug.WriteLine($"GetAllBooksAsync: Retrieved {books.Count} books");
                return books;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAllBooksAsync error: {ex.Message}");
                throw new ApplicationException($"Error retrieving books: {ex.Message}", ex);
            }
        }

        public async Task<List<Books>> GetAllBooksWithDetailsAsync()
        {
            try
            {
                Debug.WriteLine("GetAllBooksWithDetailsAsync: Starting...");
                var books = await _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .ToListAsync();
                Debug.WriteLine($"GetAllBooksWithDetailsAsync: Retrieved {books.Count} books with details");
                return books;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAllBooksWithDetailsAsync error: {ex.Message}");
                throw new ApplicationException($"Error retrieving books with details: {ex.Message}", ex);
            }
        }

        public async Task<Books> CreateBookAsync(Books book)
        {
            try
            {
                Debug.WriteLine($"CreateBookAsync: Starting - Title: {book.Title}");

                if (book == null)
                    throw new ArgumentNullException(nameof(book));

                // Set default values if needed
                if (string.IsNullOrWhiteSpace(book.Title))
                    book.Title = "New Book";

                if (book.PublishYear <= 0)
                    book.PublishYear = DateTime.Now.Year;

                if (book.Pages <= 0)
                    book.Pages = 100;

                if (book.Price < 0)
                    book.Price = 0;

                if (book.StockQuantity < 0)
                    book.StockQuantity = 0;

                book.CreatedDate = DateTime.Now;

                Debug.WriteLine($"CreateBookAsync: Adding book to context");
                _context.Books.Add(book);

                Debug.WriteLine($"CreateBookAsync: Saving changes");
                await _context.SaveChangesAsync();

                Debug.WriteLine($"CreateBookAsync: Book created with ID: {book.BookId}");

                // Return the created book with navigation properties
                var createdBook = await _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .FirstOrDefaultAsync(b => b.BookId == book.BookId);

                Debug.WriteLine($"CreateBookAsync: Complete");
                return createdBook ?? book;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CreateBookAsync error: {ex.Message}");
                Debug.WriteLine($"CreateBookAsync stack trace: {ex.StackTrace}");
                throw new ApplicationException($"Error creating book: {ex.Message}", ex);
            }
        }

        public async Task<Books> UpdateBookAsync(Books book)
        {
            try
            {
                Debug.WriteLine($"UpdateBookAsync: Starting - ID: {book.BookId}, Title: {book.Title}");

                if (book == null)
                    throw new ArgumentNullException(nameof(book));

                var existingBook = await _context.Books.FindAsync(book.BookId);
                if (existingBook == null)
                    throw new ArgumentException($"Book with ID {book.BookId} not found");

                Debug.WriteLine($"UpdateBookAsync: Found existing book, updating properties");

                // Update properties
                existingBook.Title = book.Title ?? "Untitled";
                existingBook.ISBN = book.ISBN;
                existingBook.CategoryId = book.CategoryId;
                existingBook.AuthorId = book.AuthorId;
                existingBook.PublishYear = book.PublishYear > 0 ? book.PublishYear : DateTime.Now.Year;
                existingBook.Pages = book.Pages > 0 ? book.Pages : 1;
                existingBook.Price = book.Price >= 0 ? book.Price : 0;
                existingBook.StockQuantity = book.StockQuantity >= 0 ? book.StockQuantity : 0;
                existingBook.Description = book.Description;

                Debug.WriteLine($"UpdateBookAsync: Saving changes");
                await _context.SaveChangesAsync();

                Debug.WriteLine($"UpdateBookAsync: Getting updated book with details");
                // Return updated book with navigation properties
                var updatedBook = await _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .FirstOrDefaultAsync(b => b.BookId == book.BookId);

                Debug.WriteLine($"UpdateBookAsync: Complete");
                return updatedBook ?? existingBook;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateBookAsync error: {ex.Message}");
                Debug.WriteLine($"UpdateBookAsync stack trace: {ex.StackTrace}");
                throw new ApplicationException($"Error updating book: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                Debug.WriteLine($"DeleteBookAsync: Starting - ID: {id}");

                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    Debug.WriteLine($"DeleteBookAsync: Book with ID {id} not found");
                    return false;
                }

                Debug.WriteLine($"DeleteBookAsync: Found book, removing from context");
                _context.Books.Remove(book);

                Debug.WriteLine($"DeleteBookAsync: Saving changes");
                await _context.SaveChangesAsync();

                Debug.WriteLine($"DeleteBookAsync: Complete - Book deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteBookAsync error: {ex.Message}");
                Debug.WriteLine($"DeleteBookAsync stack trace: {ex.StackTrace}");
                throw new ApplicationException($"Error deleting book: {ex.Message}", ex);
            }
        }

        // Search and Filter Operations
        public async Task<List<Books>> SearchBooksAsync(string searchTerm)
        {
            try
            {
                Debug.WriteLine($"SearchBooksAsync: Searching for '{searchTerm}'");

                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetAllBooksWithDetailsAsync();

                searchTerm = searchTerm.ToLower();

                var books = await _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .Where(b => b.Title.ToLower().Contains(searchTerm) ||
                               (b.ISBN != null && b.ISBN.ToLower().Contains(searchTerm)) ||
                               (b.Description != null && b.Description.ToLower().Contains(searchTerm)))
                    .ToListAsync();

                Debug.WriteLine($"SearchBooksAsync: Found {books.Count} books");
                return books;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SearchBooksAsync error: {ex.Message}");
                throw new ApplicationException($"Error searching books: {ex.Message}", ex);
            }
        }

        public async Task<List<Books>> FilterBooksByCategoryAsync(int categoryId)
        {
            try
            {
                Debug.WriteLine($"FilterBooksByCategoryAsync: Filtering by category {categoryId}");

                var books = await _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .Where(b => b.CategoryId == categoryId)
                    .ToListAsync();

                Debug.WriteLine($"FilterBooksByCategoryAsync: Found {books.Count} books");
                return books;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FilterBooksByCategoryAsync error: {ex.Message}");
                throw new ApplicationException($"Error filtering books by category: {ex.Message}", ex);
            }
        }

        public async Task<List<Books>> FilterBooksByAuthorAsync(int authorId)
        {
            try
            {
                Debug.WriteLine($"FilterBooksByAuthorAsync: Filtering by author {authorId}");

                var books = await _context.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .Where(b => b.AuthorId == authorId)
                    .ToListAsync();

                Debug.WriteLine($"FilterBooksByAuthorAsync: Found {books.Count} books");
                return books;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FilterBooksByAuthorAsync error: {ex.Message}");
                throw new ApplicationException($"Error filtering books by author: {ex.Message}", ex);
            }
        }

        // CRUD Operations for Authors
        public async Task<List<Authors>> GetAllAuthorsAsync()
        {
            try
            {
                Debug.WriteLine("GetAllAuthorsAsync: Starting...");
                var authors = await _context.Authors.ToListAsync();
                Debug.WriteLine($"GetAllAuthorsAsync: Retrieved {authors.Count} authors");
                return authors;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAllAuthorsAsync error: {ex.Message}");
                throw new ApplicationException($"Error retrieving authors: {ex.Message}", ex);
            }
        }

        public async Task<Authors> CreateAuthorAsync(Authors author)
        {
            try
            {
                Debug.WriteLine($"CreateAuthorAsync: Creating author {author.FirstName} {author.LastName}");

                if (author == null)
                    throw new ArgumentNullException(nameof(author));

                if (string.IsNullOrWhiteSpace(author.FirstName) || string.IsNullOrWhiteSpace(author.LastName))
                    throw new ArgumentException("Author first name and last name are required");

                author.CreatedDate = DateTime.Now;
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                Debug.WriteLine($"CreateAuthorAsync: Author created with ID: {author.AuthorId}");
                return author;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CreateAuthorAsync error: {ex.Message}");
                throw new ApplicationException($"Error creating author: {ex.Message}", ex);
            }
        }

        public async Task<Authors> UpdateAuthorAsync(Authors author)
        {
            try
            {
                Debug.WriteLine($"UpdateAuthorAsync: Updating author ID: {author.AuthorId}");

                if (author == null)
                    throw new ArgumentNullException(nameof(author));

                _context.Entry(author).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                Debug.WriteLine($"UpdateAuthorAsync: Author updated successfully");
                return author;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateAuthorAsync error: {ex.Message}");
                throw new ApplicationException($"Error updating author: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            try
            {
                Debug.WriteLine($"DeleteAuthorAsync: Deleting author ID: {id}");

                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                    return false;

                // Check if author has books
                var hasBooks = await _context.Books.AnyAsync(b => b.AuthorId == id);
                if (hasBooks)
                    throw new InvalidOperationException("Cannot delete author who has books. Please reassign or delete the books first.");

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                Debug.WriteLine($"DeleteAuthorAsync: Author deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteAuthorAsync error: {ex.Message}");
                throw new ApplicationException($"Error deleting author: {ex.Message}", ex);
            }
        }

        // CRUD Operations for Categories
        public async Task<List<Categories>> GetAllCategoriesAsync()
        {
            try
            {
                Debug.WriteLine("GetAllCategoriesAsync: Starting...");
                var categories = await _context.Categories.ToListAsync();
                Debug.WriteLine($"GetAllCategoriesAsync: Retrieved {categories.Count} categories");
                return categories;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAllCategoriesAsync error: {ex.Message}");
                throw new ApplicationException($"Error retrieving categories: {ex.Message}", ex);
            }
        }

        public async Task<Categories> CreateCategoryAsync(Categories category)
        {
            try
            {
                Debug.WriteLine($"CreateCategoryAsync: Creating category {category.CategoryName}");

                if (category == null)
                    throw new ArgumentNullException(nameof(category));

                if (string.IsNullOrWhiteSpace(category.CategoryName))
                    throw new ArgumentException("Category name is required");

                category.CreatedDate = DateTime.Now;
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                Debug.WriteLine($"CreateCategoryAsync: Category created with ID: {category.CategoryId}");
                return category;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CreateCategoryAsync error: {ex.Message}");
                throw new ApplicationException($"Error creating category: {ex.Message}", ex);
            }
        }

        public async Task<Categories> UpdateCategoryAsync(Categories category)
        {
            try
            {
                Debug.WriteLine($"UpdateCategoryAsync: Updating category ID: {category.CategoryId}");

                if (category == null)
                    throw new ArgumentNullException(nameof(category));

                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                Debug.WriteLine($"UpdateCategoryAsync: Category updated successfully");
                return category;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateCategoryAsync error: {ex.Message}");
                throw new ApplicationException($"Error updating category: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                Debug.WriteLine($"DeleteCategoryAsync: Deleting category ID: {id}");

                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return false;

                // Check if category has books
                var hasBooks = await _context.Books.AnyAsync(b => b.CategoryId == id);
                if (hasBooks)
                    throw new InvalidOperationException("Cannot delete category that has books. Please reassign or delete the books first.");

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                Debug.WriteLine($"DeleteCategoryAsync: Category deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteCategoryAsync error: {ex.Message}");
                throw new ApplicationException($"Error deleting category: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            try
            {
                Debug.WriteLine("BookService: Disposing context");
                _context?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"BookService.Dispose error: {ex.Message}");
            }
        }
    }
}
