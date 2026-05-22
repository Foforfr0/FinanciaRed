using System.Windows;
using System.Windows.Input;

namespace FinanciaRed.View.ManageCredits {
    /// <summary>
    /// Interaction logic for RegistrerCredit.xaml
    /// </summary>
    public partial class RegistrerCredit : Window {
        public RegistrerCredit () {
            InitializeComponent ();
        }

        public void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar registro.",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes)
                this.Close ();
        }

        public void ClickContinueStage2 (object sender, RoutedEventArgs e) {
        
        }

        private void PreviewTextInput_OnlyNumbers (object sender, TextCompositionEventArgs e) {
            if (!char.IsDigit (e.Text, 0)) {
                e.Handled = true;
            }
        }
    }
}
