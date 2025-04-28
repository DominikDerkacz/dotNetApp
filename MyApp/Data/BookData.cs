using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Data
{
    public class BookData
    {
        public int Id { get; set; }          
        public bool IsRead { get; set; }     
        public bool IsToRead { get; set; }   
        public bool IsFavorite { get; set; } 
        public double? Rating { get; set; }
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
