using System;
using System.Windows;
using System.Windows.Controls;

namespace MyApp
{
    /// <summary>
    /// Okno dialogowe służące do oceny, które umożliwia użytkownikowi wybór wartości oceny przy pomocy suwaka.
    /// </summary>
    public partial class RatingDialog : Window
    {
        /// <summary>
        /// Właściwość przechowująca wartość oceny wybraną przez użytkownika.
        /// </summary>
        public double Rating { get; private set; }

        /// <summary>
        /// Inicjalizuje nowe okno dialogowe RatingDialog.
        /// Jeśli podano bieżącą ocenę, ustawia ją na suwaku.
        /// </summary>
        /// <param name="currentRating">Bieżąca ocena (opcjonalnie).</param>
        public RatingDialog(double? currentRating = null)
        {
            InitializeComponent(); ///<summary>Inicjalizuje komponenty okna.</summary>

            // Ustawia wartość suwaka, jeśli podano bieżącą ocenę
            if (currentRating.HasValue)
            {
                RatingSlider.Value = currentRating.Value; ///<summary>Ustawia wartość suwaka na bieżącą ocenę.</summary>
            }

            // Obsługuje zdarzenie zmiany wartości suwaka i aktualizuje tekst z wartością
            RatingSlider.ValueChanged += (sender, e) =>
            {
                RatingValueText.Text = RatingSlider.Value.ToString("0"); ///<summary>Aktualizuje tekst z wartością suwaka.</summary>
            };

            // Ustawia początkową wartość tekstu na wartość suwaka
            RatingValueText.Text = RatingSlider.Value.ToString("0"); ///<summary>Inicjalizuje tekst z wartością suwaka.</summary>
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku "OK", zapisując wybraną ocenę i zamykając okno.
        /// </summary>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Rating = RatingSlider.Value; ///<summary>Przypisuje wybraną wartość suwaka do właściwości Rating.</summary>
            DialogResult = true; ///<summary>Ustawia wynik dialogu na true, co powoduje zamknięcie okna.</summary>
        }
    }
}
