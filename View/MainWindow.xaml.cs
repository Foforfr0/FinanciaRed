using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FinanciaRed.View {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page {
        public MainWindow () {
            InitializeComponent ();
        }

        private void ClickCheckAccount (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new CheckProfile ());
        }
        private void ClickShowManagementClientsFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewClients());
        }
        private void ClickShowManagementCreditsFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewCredits ());
        }
        private void ClickLogOut (object sender, RoutedEventArgs e) {
            NavigationService navService = NavigationService.GetNavigationService (this);
            navService.Navigate (new Uri ("View/Login.xaml", UriKind.Relative));
        }
    }
}
