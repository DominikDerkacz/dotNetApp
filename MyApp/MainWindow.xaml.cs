using System;
using System.Collections.Generic;
using System.Windows;
using System.Threading.Tasks;
using MyApp.Books;
using System.Windows.Controls;
using MyApp;
using MyApp.Services;
using MyApp.Data;
using System.ComponentModel;
using System.Windows.Data;

namespace BookListApp
{
    /// <summary>
    /// Klasa MainWindow odpowiedzialna jest za wyświetlanie listy książek i ich szczegółów.
    /// Współpracuje zarówno z API, jak i lokalnym repozytorium, aby zarządzać danymi książek.
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApiService _apiService; ///<summary>Serwis do pobierania danych książek z zewnętrznego API.</summary>
        private BookRepository _bookRepository; ///<summary>Repozytorium odpowiedzialne za przechowywanie danych książek lokalnie.</summary>
        private Dictionary<int, BookData> _bookDataCache; ///<summary>Cache do przechowywania statusu książek (IsRead, IsFavorite itp.).</summary>
        private List<Book> _books; ///<summary>Lista książek, która jest wyświetlana w UI.</summary>
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        public MainWindow()
        {
            InitializeComponent(); ///<summary>Inicjalizuje komponenty WPF do interfejsu użytkownika.</summary>
            _apiService = new ApiService(); ///<summary>Inicjalizuje serwis ApiService do pobierania danych książek.</summary>
            _bookRepository = new BookRepository(); ///<summary>Inicjalizuje repozytorium BookRepository do zarządzania danymi lokalnymi.</summary>
            _bookDataCache = new Dictionary<int, BookData>(); ///<summary>Inicjalizuje słownik do cache'owania danych książek.</summary>
            LoadBooksAsync(); ///<summary>Asynchronicznie ładuje książki z API i lokalnego repozytorium.</summary>
        }

        /// <summary>
        /// Asynchronicznie ładuje książki z API oraz z lokalnego repozytorium.
        /// Łączy dane z obu źródeł i aktualizuje interfejs użytkownika.
        /// </summary>
        private async Task LoadBooksAsync()
        {
            try
            {
                // Pobierz zapisane dane książek z lokalnego repozytorium
                var savedBookData = await _bookRepository.GetAllBookDataAsync();
                _bookDataCache = savedBookData.ToDictionary(b => b.Id); ///<summary>Wypełnia cache książek danymi z lokalnego repozytorium.</summary>

                // Pobierz książki z zewnętrznego API
                var books = await _apiService.GetBooksAsync();

                // Połącz dane z lokalnego repozytorium z książkami z API
                foreach (var book in books)
                {
                    if (_bookDataCache.TryGetValue(book.Id, out var data))
                    {
                        book.IsRead = data.IsRead;
                        book.IsToRead = data.IsToRead;
                        book.IsFavorite = data.IsFavorite;
                        book.Rating = data.Rating; ///<summary>Aktualizuje dane książki na podstawie lokalnych zapisów.</summary>
                    }
                    else
                    {
                        // Jeśli brak danych w cache, ustaw domyślną wartość dla ratingu
                        book.Rating = null;
                    }
                }

                _books = books; // Przypisanie książek do listy _books
                
                BooksListView.ItemsSource = books; // Przypisanie książek do widoku
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wczytywania książek: {ex.Message}"); ///<summary>Obsługuje wyjątki i wyświetla komunikat o błędzie.</summary>
            }
        }

        /// <summary>
        /// Funkcja do obsługi kliknięcia dwukrotnego w element listy książek.
        /// Wyświetla szczegóły książki w oknie dialogowym.
        /// </summary>
        private void BooksListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (BooksListView.SelectedItem is Book selectedBook)
            {
                MessageBox.Show($"Szczegóły książki: {selectedBook.Title}"); ///<summary>Pokazuje nazwę książki w oknie dialogowym.</summary>
            }
        }

        /// <summary>
        /// Obsługuje zmianę stanu checkboxa "Ulubione" (zaznaczone/odznaczone).
        /// </summary>
        private async void FavoriteCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, true, nameof(Book.IsFavorite));
        }

        /// <summary>
        /// Obsługuje zmianę stanu checkboxa "Ulubione" (odznaczone).
        /// </summary>
        private async void FavoriteCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, false, nameof(Book.IsFavorite));
        }

        /// <summary>
        /// Obsługuje zmianę stanu checkboxa "Do przeczytania" (zaznaczone/odznaczone).
        /// </summary>
        private async void ToReadCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, true, nameof(Book.IsToRead));
        }

        /// <summary>
        /// Obsługuje zmianę stanu checkboxa "Do przeczytania" (odznaczone).
        /// </summary>
        private async void ToReadCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, false, nameof(Book.IsToRead));
        }

        /// <summary>
        /// Obsługuje zmianę stanu checkboxa "Przeczytane" (zaznaczone/odznaczone).
        /// </summary>
        private async void ReadCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, true, nameof(Book.IsRead));
        }

        /// <summary>
        /// Obsługuje zmianę stanu checkboxa "Przeczytane" (odznaczone).
        /// </summary>
        private async void ReadCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            await HandleCheckBoxChange(sender, false, nameof(Book.IsRead));
        }

        /// <summary>
        /// Ogólna funkcja obsługująca zmianę stanu dowolnego checkboxa związane z książką.
        /// </summary>
        private async Task HandleCheckBoxChange(object sender, bool isChecked, string propertyName)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Book book)
            {
                // Aktualizuje odpowiednią właściwość książki w zależności od zmiany checkboxa
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

                // Zapisuje zaktualizowany status książki do lokalnej bazy danych
                await SaveBookStatusAsync(book);
            }
        }

        /// <summary>
        /// Zapisuje status książki (czy jest przeczytana, do przeczytania, ulubiona) w lokalnej bazie danych.
        /// </summary>
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

                await _bookRepository.SaveBookDataAsync(bookData); ///<summary>Zapisuje dane książki do repozytorium.</summary>
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania statusu książki: {ex.Message}"); ///<summary>Obsługuje wyjątki i wyświetla komunikat o błędzie.</summary>
            }
        }

        /// <summary>
        /// Obsługuje zmianę wyboru książki w widoku listy.
        /// </summary>
        private void BooksListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku szczegółów książki, otwierając nowe okno z jej detalami.
        /// </summary>
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Book selectedBook)
            {
                // Otwarcie okna szczegółów książki
                var detailsWindow = new BookDetailsWindow(selectedBook);
                detailsWindow.Show(); ///<summary>Wyświetla okno szczegółów książki.</summary>
            }
        }

        /// <summary>
        /// Obsługuje zmianę tekstu w polu wyszukiwania, filtrując książki po tytule.
        /// </summary>
        /// <param name="sender">Obiekt, który wywołał zdarzenie, czyli TextBox.</param>
        /// <param name="e">Informacje o zdarzeniu zmiany tekstu.</param>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();

            // Filtrujemy książki po tytule
            var filteredBooks = _books.Where(book => book.Title.ToLower().Contains(searchText)).ToList();

            // Przypisujemy przefiltrowaną listę do ListView
            BooksListView.ItemsSource = filteredBooks;
        }

        /// <summary>
        /// Sortuje książki według wskazanej kolumny i kierunku.
        /// </summary>
        /// <param name="sortBy">Nazwa właściwości, po której książki mają być posortowane.</param>
        /// <param name="direction">Kierunek sortowania (rosnący/malejący).</param>
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(BooksListView.ItemsSource);

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }

        /// <summary>
        /// Obsługuje kliknięcie nagłówka kolumny w celu posortowania książek według wybranej kolumny.
        /// </summary>
        /// <param name="sender">Obiekt, który wywołał zdarzenie, czyli nagłówek kolumny.</param>
        /// <param name="e">Informacje o zdarzeniu kliknięcia.</param>
        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader headerClicked)
            {
                string sortBy = null;

                // Obsługuje klasyczne kolumny (np. Tytuł, Rok)
                if (headerClicked.Column?.DisplayMemberBinding is Binding binding)
                {
                    sortBy = binding.Path.Path;
                }
                // Obsługuje kolumny bez DisplayMemberBinding (np. z CheckBox)
                else if (headerClicked.Tag is string tag)
                {
                    sortBy = tag;
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    ListSortDirection direction;

                    // Sprawdzamy, czy kliknięto w kolumnę z CheckBoxami
                    bool isCheckboxColumn = sortBy == "IsFavorite" || sortBy == "IsToRead" || sortBy == "IsRead";

                    // Dla kolumn z CheckBoxami sortowanie domyślnie malejące
                    if (headerClicked == _lastHeaderClicked)
                    {
                        direction = _lastDirection == ListSortDirection.Ascending
                            ? ListSortDirection.Descending
                            : ListSortDirection.Ascending;
                    }
                    else
                    {
                        // Ustawiamy domyślnie malejąco dla checkboxów, rosnąco dla innych kolumn
                        direction = isCheckboxColumn ? ListSortDirection.Descending : ListSortDirection.Ascending;
                    }

                    Sort(sortBy, direction);

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }




    }
}
