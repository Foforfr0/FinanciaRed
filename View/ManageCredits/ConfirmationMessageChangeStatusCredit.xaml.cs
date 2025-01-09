using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.Credit;
using System.Threading.Tasks;
using System.Windows;

namespace FinanciaRed.View.ManageCredits {
    /// <summary>
    /// Interaction logic for ConfirmationMessageChangeStatusCredit.xaml
    /// </summary>
    public partial class ConfirmationMessageChangeStatusCredit : Window {
        private DTO_StatusCredit currentStatusCredit = null;

        public ConfirmationMessageChangeStatusCredit (int idCredit) {
            InitializeComponent ();

            _ = RetrieveStatusCredit (idCredit);
        }

        private async Task RetrieveStatusCredit (int idCredit) {
            MessageResponse<DTO_StatusCredit> response = await DAO_Credit.GetStatusCreditAsync (idCredit);

            if (response.IsError || response.DataRetrieved == null)
                MessageBox.Show (
                    "No se logró obtener el estado actual del crédito.",
                    "Error inesperado.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else {
                DTO_StatusCredit aux = response.DataRetrieved;
                textBlock_StatusCredit.Text = aux.Status;
                textBlock_NewStatus.Text = aux.IdStatus == 2 ? "Incobrable" : "Cobrable";
            }
        }

        private async void ClickAccept (object sender, RoutedEventArgs e) {
            MessageResponse<bool> response = await DAO_Credit.PutStatusCreditAsync (
                currentStatusCredit.IdStatus,
                currentStatusCredit.IdStatus == 2 ? 3 : 2);

            if (response.IsError)
                MessageBox.Show (
                    "No se logró cambiar el estado del crédito",
                    "Error inesperado.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else {
                MessageBox.Show (
                    "Se ha modificado correctamente el crédito.",
                    "Modificación completa.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close ();
            }
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Desea cancelar el cambio de estado del crédito?",
                "¿Cancelar el cambio del estado?",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                this.Close ();
        }
    }
}
