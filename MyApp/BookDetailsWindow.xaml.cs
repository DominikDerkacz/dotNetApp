using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MyApp.Data;
using MyApp.Books;

namespace MyApp
{
    /// <summary>
    /// Okno szczegółów książki, umożliwiające wyświetlanie informacji o książce, ocenianie jej i zarządzanie notatkami.
    /// </summary>
    public partial class BookDetailsWindow : Window
    {
        private Book _currentBook;  ///< Obiekt reprezentujący aktualnie wybraną książkę.
        private BookRepository _bookRepository;  ///< Repozytorium do zapisywania danych książek.

        /// <summary>
        /// Konstruktor inicjalizujący okno szczegółów książki.
        /// </summary>
        /// <param name="selectedBook">Obiekt książki, który ma być wyświetlony w oknie.</param>
        public BookDetailsWindow(Book selectedBook)
        {
            InitializeComponent();
            _currentBook = selectedBook;
            _bookRepository = new BookRepository();

            // Ustawienie tekstów w interfejsie użytkownika
            TitleTextBlock.Text = selectedBook.Title;
            UpdateRatingDisplay();
            PublisherTextBlock.Text = $"Wydawnictwo: {selectedBook.Publisher}";
            YearTextBlock.Text = $"Rok wydania: {selectedBook.Year}";
            PagesTextBlock.Text = $"Liczba stron: {selectedBook.Pages}";
            ISBNTextBlock.Text = $"ISBN: {selectedBook.ISBN}";

            // Wyświetlanie notatek, jeśli istnieją
            if (selectedBook.Notes != null && selectedBook.Notes.Any(note => !string.IsNullOrWhiteSpace(note)))
            {
                NotesList.ItemsSource = selectedBook.Notes.Where(note => !string.IsNullOrWhiteSpace(note));
                NotesList.Visibility = Visibility.Visible;
                NotesEmptyMessage.Visibility = Visibility.Collapsed;
            }
            else
            {
                NotesList.Visibility = Visibility.Collapsed;
                NotesEmptyMessage.Visibility = Visibility.Visible;
            }

            // Wyświetlanie złoczyńców, jeśli istnieją
            if (selectedBook.Villains != null && selectedBook.Villains.Any(v => !string.IsNullOrWhiteSpace(v.Name)))
            {
                VillainsList.ItemsSource = selectedBook.Villains
                    .Where(v => !string.IsNullOrWhiteSpace(v.Name))
                    .ToList();
                VillainsList.Visibility = Visibility.Visible;
                VillainEmptyMessage.Visibility = Visibility.Collapsed;
            }
            else
            {
                VillainsList.Visibility = Visibility.Collapsed;
                VillainEmptyMessage.Visibility = Visibility.Visible;
            }

            // Zaktualizowanie widoczności przycisków oceny
            UpdateRatingButtonsVisibility();
        }

        /// <summary>
        /// Aktualizuje wyświetlanie oceny książki.
        /// </summary>
        private void UpdateRatingDisplay()
        {
            if (_currentBook.Rating.HasValue)
            {
                RatingTextBlock.Text = $"Ocena: {_currentBook.Rating.Value:0.0}";
            }
            else
            {
                RatingTextBlock.Text = "Ocena: nie określono";
            }
        }

        /// <summary>
        /// Aktualizuje widoczność przycisków związanych z oceną książki.
        /// </summary>
        private void UpdateRatingButtonsVisibility()
        {
            RemoveRatingButton.Visibility = _currentBook.Rating.HasValue ? Visibility.Visible : Visibility.Collapsed;
            EditRatingButton.Content = _currentBook.Rating.HasValue ? "Edytuj ocenę" : "Dodaj ocenę";
        }

        /// <summary>
        /// Zamknięcie okna szczegółów książki.
        /// </summary>
        /// <param name="sender">Źródło zdarzenia.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Otwiera okno edycji oceny książki.
        /// </summary>
        /// <param name="sender">Źródło zdarzenia.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private async void EditRating_Click(object sender, RoutedEventArgs e)
        {
            var ratingDialog = new RatingDialog(_currentBook.Rating);
            ratingDialog.Owner = this;

            if (ratingDialog.ShowDialog() == true)
            {
                _currentBook.Rating = ratingDialog.Rating;

                // Zaktualizowanie wyświetlania oceny
                UpdateRatingDisplay();
                UpdateRatingButtonsVisibility();

                // Zapisanie zmiany w bazie danych
                await SaveRatingToDatabase();
            }
        }

        /// <summary>
        /// Usuwa ocenę książki po potwierdzeniu przez użytkownika.
        /// </summary>
        /// <param name="sender">Źródło zdarzenia.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private async void RemoveRating_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć ocenę?", "Potwierdzenie",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _currentBook.Rating = null;

                // Zaktualizowanie wyświetlania oceny
                UpdateRatingDisplay();
                UpdateRatingButtonsVisibility();

                // Zapisanie zmiany w bazie danych
                await SaveRatingToDatabase();
            }
        }

        /// <summary>
        /// Zapisuje dane o książce (w tym ocenę) do bazy danych.
        /// </summary>
        private async Task SaveRatingToDatabase()
        {
            try
            {
                var bookData = new BookData
                {
                    Id = _currentBook.Id,
                    IsRead = _currentBook.IsRead,
                    IsToRead = _currentBook.IsToRead,
                    IsFavorite = _currentBook.IsFavorite,
                    Rating = _currentBook.Rating
                };

                // Zapisanie danych książki w bazie
                await _bookRepository.SaveBookDataAsync(bookData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania oceny: {ex.Message}", "Błąd",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
