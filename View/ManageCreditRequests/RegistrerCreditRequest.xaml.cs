using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinanciaRed.View.ManageCreditRequests {
    /// <summary>
    /// Interaction logic for RegistrerCreditRequest.xaml
    /// </summary>
    public partial class RegistrerCreditRequest : Window {
        private bool[] _IsCorrectStage1 = new bool[2] { false, false };
        private bool[] _IsCorrectStage2 = new bool[3] { false, false, false };

        public RegistrerCreditRequest () {
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

        private void ClickBackStage1 (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Visible;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
        }

        public void ClickContinueStage2 (object sender, RoutedEventArgs e) {
            VerifyStage1 ();

            if (_IsCorrectStage1.All (x => x == true)) {
                stackPanel_Stage1.Visibility = Visibility.Collapsed;
                stackPanel_Stage2.Visibility = Visibility.Visible;
            } else {
                MessageBox.Show ("Faltan datos por ingresar o algunos datos están incorrectos.", "Formulario incompleto.");
            }
        }

        public async void ClickFinishRegistry (object sender, RoutedEventArgs e) {
            VerifyStage2 ();

            if (_IsCorrectStage2.All (x => x == true)) {
                await SaveDataInDatabase ();
            } else {
                MessageBox.Show ("Faltan datos por ingresar o algunos datos están incorrectos.", "Formulario incompleto.");
            }
        }

        private void VerifyStage1 () {
            if (string.IsNullOrEmpty (textBox_SolicitedAmount.Text)) {
                label_ErrorSolicitedAmount.Content = "Campo necesario.";
                label_ErrorSolicitedAmount.Visibility = Visibility.Visible;
                _IsCorrectStage1[0] = false;
            }

            if (comboBox_TimeLimit.SelectedIndex == 0) {
                label_ErrorTimeLimit.Content = "Campo necesario.";
                label_ErrorTimeLimit.Visibility = Visibility.Visible;
                _IsCorrectStage1[1] = false;
            }
        }

        private void VerifyStage2 () {

        }

        private async Task SaveDataInDatabase () {
            

            
        }

        private void PreviewTextInput_OnlyNumbers (object sender, TextCompositionEventArgs e) {
            if (!char.IsDigit (e.Text, 0)) {
                e.Handled = true;
            }
        }
    }
}
