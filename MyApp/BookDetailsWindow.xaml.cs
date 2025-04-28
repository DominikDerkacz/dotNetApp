using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MyApp.Data;  // Dodaj ten using
using MyApp.Books;

namespace MyApp
{
    public partial class BookDetailsWindow : Window
    {
        private Book _currentBook;
        private BookRepository _bookRepository;  // Dodaj to pole

        public BookDetailsWindow(Book selectedBook)
        {
            InitializeComponent();
            _currentBook = selectedBook;
            _bookRepository = new BookRepository();  // Inicjalizuj repozytorium

            // Wypełnianie pól szczegółów książki
            TitleTextBlock.Text = selectedBook.Title;
            UpdateRatingDisplay();
            PublisherTextBlock.Text = $"Wydawnictwo: {selectedBook.Publisher}";
            YearTextBlock.Text = $"Rok wydania: {selectedBook.Year}";
            PagesTextBlock.Text = $"Liczba stron: {selectedBook.Pages}";
            ISBNTextBlock.Text = $"ISBN: {selectedBook.ISBN}";

            // Notatki
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

            // Złoczyńcy
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

            // Aktualizuj widoczność przycisków oceny
            UpdateRatingButtonsVisibility();
        }

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

        private void UpdateRatingButtonsVisibility()
        {
            // Pokaż przycisk usuwania tylko jeśli książka ma ocenę
            RemoveRatingButton.Visibility = _currentBook.Rating.HasValue ? Visibility.Visible : Visibility.Collapsed;
            // Zmień tekst przycisku edycji w zależności od tego, czy książka ma już ocenę
            EditRatingButton.Content = _currentBook.Rating.HasValue ? "Edytuj ocenę" : "Dodaj ocenę";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void VillainsDetails(object sender, RoutedEventArgs e)
        {
            // Tworzymy nowe okno ze szczegółami złoczyńcy
            var detailsWindow = new VillainDetailsWindow();
            detailsWindow.Show();
        }

        private async void EditRating_Click(object sender, RoutedEventArgs e)
        {
            var ratingDialog = new RatingDialog(_currentBook.Rating);
            ratingDialog.Owner = this;

            if (ratingDialog.ShowDialog() == true)
            {
                // Aktualizuj model książki
                _currentBook.Rating = ratingDialog.Rating;

                // Aktualizuj UI
                UpdateRatingDisplay();
                UpdateRatingButtonsVisibility();

                // Zapisz do bazy danych
                await SaveRatingToDatabase();
            }
        }

        private async void RemoveRating_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć ocenę?", "Potwierdzenie",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // Usuń ocenę
                _currentBook.Rating = null;

                // Aktualizuj UI
                UpdateRatingDisplay();
                UpdateRatingButtonsVisibility();

                // Zapisz do bazy danych
                await SaveRatingToDatabase();
            }
        }

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