// RatingDialog.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyApp
{
    public partial class RatingDialog : Window
    {
        public double Rating { get; private set; }

        public RatingDialog(double? currentRating = null)
        {
            InitializeComponent();

           
            if (currentRating.HasValue)
            {
                RatingSlider.Value = currentRating.Value;
            }

           
            RatingSlider.ValueChanged += (sender, e) =>
            {
                RatingValueText.Text = RatingSlider.Value.ToString("0");
            };

            
            RatingValueText.Text = RatingSlider.Value.ToString("0");
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Rating = RatingSlider.Value;
            DialogResult = true;
        }
    }
}