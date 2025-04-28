using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyApp.Books
{
    public class Book : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Handle { get; set; }
        public string Publisher { get; set; }
        public int Pages { get; set; }
        public List<string> Notes { get; set; }
        public List<Villain> Villains { get; set; }

        private double? _rating;
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

        public string Review { get; set; }

        private bool _isFavorite;
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

        public DateTime AddedDate { get; set; } = DateTime.Now;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}