using System;
using System.Collections.Generic;
using System.Windows;
using System.Threading.Tasks;
using MyApp.Books;
using System.Windows.Controls;
using MyApp;

namespace BookListApp
{
    public partial class MainWindow : Window
    {
        private ApiService _apiService;

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadBooksAsync();
        }

        private async Task LoadBooksAsync()
        {
            List<Book> books = null;

            try
            {
                books = await _apiService.GetBooksAsync();
                BooksListView.ItemsSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wczytywania książek: {ex.Message}");
            }

            if (books != null)
            {
                foreach (var book in books)
                {
                    book.Rating = new Random().Next(1, 10);
                }
            }
        }

        private void BooksListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (BooksListView.SelectedItem is Book selectedBook)
            {
                MessageBox.Show($"Szczegóły książki: {selectedBook.Title}");
                // Tutaj możesz otworzyć nowe okno ze szczegółami
            }
        }

        private void BooksListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Book selectedBook)
            {
                // Tworzymy nowe okno ze szczegółami książki
                var detailsWindow = new BookDetailsWindow(selectedBook);
                detailsWindow.Show(); // lub .ShowDialog() jeśli chcesz, żeby było modalne
            }
        }
    }
}
