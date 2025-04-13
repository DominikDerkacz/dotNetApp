using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyApp.Books;

namespace MyApp
{
    public partial class BookDetailsWindow : Window
    {
        public Book SelectedBook { get; set; }

        // Konstruktor, który przyjmuje obiekt Book
        public BookDetailsWindow(Book book)
        {
            InitializeComponent();
            SelectedBook = book;

            // Wyświetlanie szczegółów książki w oknie
            TitleTextBlock.Text = book.Title;
            RatingTextBlock.Text = $"Ocena: {book.Rating}";
            PublisherTextBlock.Text = $"Wydawnictwo: {book.Publisher}";
            YearTextBlock.Text = $"Rok: {book.Year}";
            PagesTextBlock.Text = $"Strony: {book.Pages}";
            // Możesz dodać inne szczegóły książki
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
