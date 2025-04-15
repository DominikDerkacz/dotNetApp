using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MyApp.Books;

namespace MyApp
{
    public partial class BookDetailsWindow : Window
    {
        public BookDetailsWindow(Book selectedBook)
        {
            InitializeComponent();

            // Wypełnianie pól szczegółów książki
            TitleTextBlock.Text = selectedBook.Title;
            RatingTextBlock.Text = $"Ocena: {selectedBook.Rating}";
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
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NotesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private void VillainsDetails(object sender, RoutedEventArgs e)
        {
                // Tworzymy nowe okno ze szczegółami książki
                var detailsWindow = new VillainDetailsWindow();
                detailsWindow.Show(); // lub .ShowDialog() jeśli chcesz, żeby było modalne
           
        }
    }
}
