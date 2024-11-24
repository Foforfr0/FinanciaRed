using System.Windows;

namespace FinanciaRed.View.ManageCreditPromotions {
    /// <summary>
    /// Interaction logic for RegisterCreditPromotion.xaml
    /// </summary>
    public partial class RegisterCreditPromotion : Window {
        public RegisterCreditPromotion () {
            InitializeComponent ();
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar modificación.",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes)
                this.Close ();
        }

        private void ClickFinishRegistration (object sender, RoutedEventArgs e) {

        }
    }
}
