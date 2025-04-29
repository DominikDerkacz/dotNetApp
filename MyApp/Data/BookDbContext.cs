using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Data
{
    /// <summary>
    /// Reprezentuje kontekst bazy danych dla książek, dziedziczy po <see cref="DbContext"/>.
    /// Umożliwia dostęp do danych przechowywanych w bazie danych SQLite.
    /// </summary>
    public class BookDbContext : DbContext
    {
        /// <summary>
        /// Zbiór danych książek w bazie danych.
        /// </summary>
        public DbSet<BookData> Books { get; set; }

        /// <summary>
        /// Konfiguruje opcje połączenia z bazą danych.
        /// Tworzy połączenie z bazą SQLite, której plik znajduje się w bieżącym katalogu.
        /// </summary>
        /// <param name="optionsBuilder">Builder do konfigurowania opcji połączenia.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "books.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        /// <summary>
        /// Konfiguruje model danych. Ustawia klucz główny dla tabeli <see cref="BookData"/>.
        /// </summary>
        /// <param name="modelBuilder">Builder do konfigurowania modelu.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BookData>()
                .HasKey(b => b.Id);
        }
    }
}
