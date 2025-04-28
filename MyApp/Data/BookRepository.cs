using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;

namespace MyApp.Data
{
    public class BookRepository
    {
   
        public async Task<List<BookData>> GetAllBookDataAsync()
        {
            using (var context = new BookDbContext())
            {
                return await context.Books.ToListAsync();
            }
        }

  
        public async Task<BookData> GetBookDataByIdAsync(int bookId)
        {
            using (var context = new BookDbContext())
            {
                return await context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            }
        }

      
        public async Task SaveBookDataAsync(BookData bookData)
        {
            using (var context = new BookDbContext())
            {
                var existingBook = await context.Books.FirstOrDefaultAsync(b => b.Id == bookData.Id);

                if (existingBook != null)
                {
                 
                    existingBook.IsRead = bookData.IsRead;
                    existingBook.IsToRead = bookData.IsToRead;
                    existingBook.IsFavorite = bookData.IsFavorite;
                    existingBook.Rating = bookData.Rating;
                    existingBook.LastModified = DateTime.Now;
                }
                else
                {
                   
                    bookData.LastModified = DateTime.Now;
                    context.Books.Add(bookData);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}