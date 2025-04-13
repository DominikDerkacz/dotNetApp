using System;
using System.Collections.Generic;

namespace MyApp.Books
{
    public class Book
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public string Handle { get; set; }
        public string Publisher { get; set; }
        public int Pages { get; set; }
        public List<Villain> Villains { get; set; }

        public double? Rating { get; set; }
        public string Review { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsToRead { get; set; }      
        public bool IsRead { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
    }
}
            