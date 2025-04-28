using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Data
{
    public class BookData
    {
        public int Id { get; set; }          // ID będzie odpowiadać ID książki z API
        public bool IsRead { get; set; }     // Czy książka została przeczytana
        public bool IsToRead { get; set; }   // Czy do przeczytania
        public bool IsFavorite { get; set; } // Czy ulubiona
        public double? Rating { get; set; }
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
