using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PySec.Pages 
{
    public static class PlaceholderBehavior
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached(
                "Placeholder",
                typeof(string),
                typeof(PlaceholderBehavior),
                new PropertyMetadata(string.Empty, OnPlaceholderChanged));

        public static string GetPlaceholder(TextBox textBox) =>
            (string)textBox.GetValue(PlaceholderProperty);

        public static void SetPlaceholder(TextBox textBox, string value) =>
            textBox.SetValue(PlaceholderProperty, value);

        private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.GotFocus -= RemovePlaceholder;
                textBox.LostFocus -= AddPlaceholder;

                if (!string.IsNullOrEmpty(e.NewValue as string))
                {
                    textBox.GotFocus += RemovePlaceholder;
                    textBox.LostFocus += AddPlaceholder;
                    AddPlaceholder(textBox, null); 
                }
            }
        }

        private static void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == GetPlaceholder(textBox))
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black; 
            }
        }

        private static void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = GetPlaceholder(textBox);
                textBox.Foreground = Brushes.Gray; 
            }
        }
    }
}
