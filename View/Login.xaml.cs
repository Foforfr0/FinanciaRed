using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace FinanciaRed.View {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page {
        private bool isPasswordVisible = false;

        public Login () {
            InitializeComponent ();
        }

        private void ClicShowPassword (object sender, RoutedEventArgs e) {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible) {
                iconEyePassword.Source = new BitmapImage (new Uri ("Images/icon-eye-open.png", UriKind.Relative));
            } else {
                iconEyePassword.Source = new BitmapImage (new Uri ("Images/icon-eye-close.png", UriKind.Relative));
            }
        }

        private void ClickRecoverPassword (Object sender, RoutedEventArgs e) {
        
        }

        private void ClickLogin (object sender, RoutedEventArgs e) {
            NavigationService navService = NavigationService.GetNavigationService (this);
            navService.Navigate (new Uri ("View/MainWindow.xaml", UriKind.Relative));
        }
    }
}
