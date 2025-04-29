using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyApp.Books
{
    /// <summary>
    /// Reprezentuje książkę z jej właściwościami i funkcjonalnością.
    /// Klasa implementuje interfejs <see cref="INotifyPropertyChanged"/>, 
    /// co umożliwia powiadamianie o zmianach w jej właściwościach.
    /// </summary>
    public class Book : INotifyPropertyChanged
    {
        /// <summary>
        /// Zdarzenie wywoływane, gdy którakolwiek z właściwości książki ulegnie zmianie.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Identyfikator książki.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Rok wydania książki.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Tytuł książki.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Numer ISBN książki.
        /// </summary>
        public string ISBN { get; set; }

        /// <summary>
        /// Uchwyt książki (może służyć do identyfikacji w systemie).
        /// </summary>
        public string Handle { get; set; }

        /// <summary>
        /// Wydawca książki.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Liczba stron w książce.
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Lista notatek związanych z książką.
        /// </summary>
        public List<string> Notes { get; set; }

        /// <summary>
        /// Lista złoczyńców występujących w książce.
        /// </summary>
        public List<Villain> Villains { get; set; }

        private double? _rating;

        /// <summary>
        /// Ocena książki.
        /// Może być null, jeśli brak oceny.
        /// </summary>
        public double? Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Recenzja książki.
        /// </summary>
        public string Review { get; set; }

        private bool _isFavorite;

        /// <summary>
        /// Określa, czy książka jest ulubiona.
        /// </summary>
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite != value)
                {
                    _isFavorite = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isToRead;

        /// <summary>
        /// Określa, czy książka jest do przeczytania.
        /// </summary>
        public bool IsToRead
        {
            get => _isToRead;
            set
            {
                if (_isToRead != value)
                {
                    _isToRead = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isRead;

        /// <summary>
        /// Określa, czy książka została przeczytana.
        /// </summary>
        public bool IsRead
        {
            get => _isRead;
            set
            {
                if (_isRead != value)
                {
                    _isRead = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Data dodania książki.
        /// </summary>
        public DateTime AddedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Wywołuje zdarzenie PropertyChanged, aby poinformować o zmianie właściwości.
        /// </summary>
        /// <param name="propertyName">Nazwa właściwości, która została zmieniona.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
