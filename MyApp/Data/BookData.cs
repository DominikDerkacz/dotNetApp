using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Data
{
    /// <summary>
    /// Reprezentuje dane książki w systemie.
    /// Klasa przechowuje informacje o stanie książki, jej ocenie oraz dacie ostatniej modyfikacji.
    /// </summary>
    public class BookData
    {
        /// <summary>
        /// Identyfikator książki.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Określa, czy książka została przeczytana.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Określa, czy książka jest do przeczytania.
        /// </summary>
        public bool IsToRead { get; set; }

        /// <summary>
        /// Określa, czy książka jest ulubiona.
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// Ocena książki.
        /// Może być null, jeśli brak oceny.
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        /// Data ostatniej modyfikacji danych książki.
        /// Domyślnie ustawiana na bieżącą datę i godzinę.
        /// </summary>
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
