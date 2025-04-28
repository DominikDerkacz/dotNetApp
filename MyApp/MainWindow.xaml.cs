    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Threading.Tasks;
    using MyApp.Books;
    using System.Windows.Controls;
    using MyApp;
using MyApp.Data;

    namespace BookListApp
    {
        public partial class MainWindow : Window
        {
            private ApiService _apiService;
            private BookRepository _bookRepository;
            private Dictionary<int, BookData> _bookDataCache;

        public MainWindow()
            {
                InitializeComponent();
                _apiService = new ApiService();
                _bookRepository = new BookRepository();
                _bookDataCache = new Dictionary<int, BookData>();
                LoadBooksAsync();
            }

        private async Task LoadBooksAsync()
        {
            try
            {
                
                var savedBookData = await _bookRepository.GetAllBookDataAsync();
                _bookDataCache = savedBookData.ToDictionary(b => b.Id);

                
                var books = await _apiService.GetBooksAsync();

               
                foreach (var book in books)
                {
                    if (_bookDataCache.TryGetValue(book.Id, out var data))
                    {
                        book.IsRead = data.IsRead;
                        book.IsToRead = data.IsToRead;
                        book.IsFavorite = data.IsFavorite;
                        book.Rating = data.Rating;  
                    }
                    else
                    {
                      
                        book.Rating = null;
                    }       
                }

               
                BooksListView.ItemsSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wczytywania książek: {ex.Message}");
            }
        }
        private void AttachPropertyChangedHandlers(List<Book> books)
        {
           
            foreach (var book in books)
            {
                
            }
        }

        private void BooksListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                if (BooksListView.SelectedItem is Book selectedBook)
                {
                    MessageBox.Show($"Szczegóły książki: {selectedBook.Title}");
                    
                }
            }
        private async void FavoriteCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, true, nameof(Book.IsFavorite));
        }

        private async void FavoriteCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, false, nameof(Book.IsFavorite));
        }

        private async void ToReadCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, true, nameof(Book.IsToRead));
        }

        private async void ToReadCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, false, nameof(Book.IsToRead));
        }

        private async void ReadCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, true, nameof(Book.IsRead));
        }

        private async void ReadCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, false, nameof(Book.IsRead));
        }

        private async Task HandleCheckBoxChange(object sender, bool isChecked, string propertyName)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Book book)
            {
               
                switch (propertyName)
                {
                    case nameof(Book.IsFavorite):
                        book.IsFavorite = isChecked;
                        break;
                    case nameof(Book.IsToRead):
                        book.IsToRead = isChecked;
                        break;
                    case nameof(Book.IsRead):
                        book.IsRead = isChecked;
                        break;
                }

               
                await SaveBookStatusAsync(book);
            }
        }

        private async Task SaveBookStatusAsync(Book book)
        {
            try
            {
                var bookData = new BookData
                {
                    Id = book.Id,
                    IsRead = book.IsRead,
                    IsToRead = book.IsToRead,
                    IsFavorite = book.IsFavorite,
                    Rating = book.Rating
                };

                await _bookRepository.SaveBookDataAsync(bookData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania statusu książki: {ex.Message}");
            }
        }

        private void BooksListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
            {

            }
            private void DetailsButton_Click(object sender, RoutedEventArgs e)
            {
                if (sender is Button button && button.Tag is Book selectedBook)
                {
                    
                    var detailsWindow = new BookDetailsWindow(selectedBook);
                    detailsWindow.Show(); 
                }
            }
        }
    }
