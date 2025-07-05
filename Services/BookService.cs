using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookManager.Data;
using BookManager.Model;
using Microsoft.EntityFrameworkCore;

namespace BookManager.Services
{
    public class BookService
    {
        private readonly BookStoreContext _context;
        public BookService()
        {
            _context = new BookStoreContext();
        }
        // CRUD Operations for Books
        // lấy ra toàn bộ sách trong database
        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        // lấy sách theo id
        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        // thêm sách
        public async Task<Book> CreateBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        // update sách
        public async Task<Book> UpdateBookAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        // xóa sách
        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // tìm sách theo tên, isbn, mô tả
        public async Task<List<Book>> SearchBooksAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllBooksAsync();

            return await _context.Books
                .Where(b => b.Title.Contains(searchTerm) ||
                           b.ISBN.Contains(searchTerm) ||
                           b.Description.Contains(searchTerm))
                .ToListAsync();
        }

        // lọc sách theo thể loại
        public async Task<List<Book>> FilterBooksByCategoryAsync(int categoryId)
        {
            return await _context.Books
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        }

        // lọc sách theo tác giả
        public async Task<List<Book>> FilterBooksByAuthorAsync(int authorId)
        {
            return await _context.Books
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }

        // lọc sách theo khoảng giá 
        public async Task<List<Book>> FilterBooksByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Books
                .Where(b => b.Price >= minPrice && b.Price <= maxPrice)
                .ToListAsync();
        }
        // CRUD Operations for Authors
        // lấy toàn bộ tác gỉả
        public async Task<List<Authors>> GetAllAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }
        public async Task<Authors> CreateAuthorAsync(Authors author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Authors> UpdateAuthorAsync(Authors author)
        {
            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // CRUD Operations for Categories
        public async Task<List<Categories>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Categories> CreateCategoryAsync(Categories category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Categories> UpdateCategoryAsync(Categories category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // giải phóng tài nguyên
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
