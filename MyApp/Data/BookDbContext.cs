using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Data
{
    public class BookDbContext : DbContext
    {
        public DbSet<BookData> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Plik bazy danych będzie w folderze z aplikacją
            string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "books.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ustaw Id jako klucz główny
            modelBuilder.Entity<BookData>()
                .HasKey(b => b.Id);
        }
    }
}