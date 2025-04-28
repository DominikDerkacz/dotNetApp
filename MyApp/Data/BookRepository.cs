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
        // Pobiera dane o wszystkich zapisanych książkach
        public async Task<List<BookData>> GetAllBookDataAsync()
        {
            using (var context = new BookDbContext())
            {
                return await context.Books.ToListAsync();
            }
        }

        // Pobiera dane o konkretnej książce po ID
        public async Task<BookData> GetBookDataByIdAsync(int bookId)
        {
            using (var context = new BookDbContext())
            {
                return await context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            }
        }

        // Zapisuje lub aktualizuje dane książki
        public async Task SaveBookDataAsync(BookData bookData)
        {
            using (var context = new BookDbContext())
            {
                var existingBook = await context.Books.FirstOrDefaultAsync(b => b.Id == bookData.Id);

                if (existingBook != null)
                {
                    // Aktualizuj istniejący wpis
                    existingBook.IsRead = bookData.IsRead;
                    existingBook.IsToRead = bookData.IsToRead;
                    existingBook.IsFavorite = bookData.IsFavorite;
                    existingBook.LastModified = DateTime.Now;
                }
                else
                {
                    // Dodaj nowy wpis
                    bookData.LastModified = DateTime.Now;
                    context.Books.Add(bookData);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}