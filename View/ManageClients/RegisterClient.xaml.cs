using System.Windows;

namespace FinanciaRed.View.ManageClients {
    /// <summary>
    /// Interaction logic for RegisterClient.xaml
    /// </summary>
    public partial class RegisterClient : Window {
        public RegisterClient () {
            InitializeComponent ();
        }

        private void ClickCloseWindow (object sender, RoutedEventArgs e) {
            this.Close ();
        }
    }
}
