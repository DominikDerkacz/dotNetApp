using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;

namespace MyApp.Data
{
    /// <summary>
    /// Reprezentuje repozytorium dla danych książek. Zapewnia metody do interakcji z bazą danych książek.
    /// </summary>
    public class BookRepository
    {
        /// <summary>
        /// Pobiera wszystkie dane książek z bazy danych asynchronicznie.
        /// </summary>
        /// <returns>Lista obiektów <see cref="BookData"/> zawierająca dane wszystkich książek.</returns>
        public async Task<List<BookData>> GetAllBookDataAsync()
        {
            using (var context = new BookDbContext())
            {
                return await context.Books.ToListAsync();
            }
        }

        /// <summary>
        /// Pobiera dane książki z bazy danych na podstawie jej identyfikatora.
        /// </summary>
        /// <param name="bookId">Identyfikator książki, dla której mają zostać pobrane dane.</param>
        /// <returns>Obiekt <see cref="BookData"/> reprezentujący książkę, lub <c>null</c>, jeśli książka nie została znaleziona.</returns>
        public async Task<BookData> GetBookDataByIdAsync(int bookId)
        {
            using (var context = new BookDbContext())
            {
                return await context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            }
        }

        /// <summary>
        /// Zapisuje dane książki w bazie danych. Jeśli książka już istnieje, jej dane są aktualizowane.
        /// Jeśli książka nie istnieje, zostaje dodana do bazy danych.
        /// </summary>
        /// <param name="bookData">Obiekt <see cref="BookData"/> zawierający dane książki do zapisania.</param>
        /// <returns>Task reprezentujący operację asynchroniczną zapisania danych.</returns>
        public async Task SaveBookDataAsync(BookData bookData)
        {
            using (var context = new BookDbContext())
            {
                var existingBook = await context.Books.FirstOrDefaultAsync(b => b.Id == bookData.Id);

                if (existingBook != null)
                {
                    // Aktualizacja istniejącej książki
                    existingBook.IsRead = bookData.IsRead;
                    existingBook.IsToRead = bookData.IsToRead;
                    existingBook.IsFavorite = bookData.IsFavorite;
                    existingBook.Rating = bookData.Rating;
                    existingBook.LastModified = DateTime.Now;
                }
                else
                {
                    // Dodanie nowej książki
                    bookData.LastModified = DateTime.Now;
                    context.Books.Add(bookData);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
