using PySec.Pages;
using PySec.Themes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PySec
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Themes_Click(object sender, RoutedEventArgs e)
        {
            if (Themes.IsChecked == true)
            {
                ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
                LogoImage.Source = new BitmapImage(new Uri("pack://application:,,,/LogoWh.png"));
                LogoLabel.Foreground = Brushes.White;
            }
            else
            {
                ThemesController.SetTheme(ThemesController.ThemeTypes.Light);
                LogoImage.Source = new BitmapImage(new Uri("pack://application:,,,/Logo.png"));
                LogoLabel.Foreground = (Brush)new BrushConverter().ConvertFromString("#FF005BAC");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void rdHome_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new Home());
        }

        /*private void rdHome_Loaded(object sender, RoutedEventArgs e)
        {
            if (rdHome.IsChecked == true)
            {
                frameContent.Navigate(new Home());
            }
        }*/

        private void rdIPAddressTools_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new IPAddressTools());
        }

        private void rdIp_Loaded(object sender, RoutedEventArgs e)
        {
            if (rdIp.IsChecked == true)
            {
                frameContent.Navigate(new IPAddressTools());
            }
        }

        private void rdDNSTools_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new DNSTools());
        }

        private void rdWebVulnTools_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new WebVulnTools());
        }

        private void rdOther_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new Other());
        }

        private void rdAbout_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(new About()); 
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}