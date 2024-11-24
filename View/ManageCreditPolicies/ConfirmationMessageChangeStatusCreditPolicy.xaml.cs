using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditPolicies {
    /// <summary>
    /// Interaction logic for ConfirmationMessageChangeStatusCreditPolicy.xaml
    /// </summary>
    public partial class ConfirmationMessageChangeStatusCreditPolicy : Window {
        public static int idCreditPolicy = 0;
        private static string statusCreditPolicy = "";

        public ConfirmationMessageChangeStatusCreditPolicy (string title, string message) {
            InitializeComponent ();

            textBlockTitleText.Text = title;
            textBlockMessageText.Text = message;
            InitializeStatuses ();
        }

        public static new async Task<bool?> Show () {
            statusCreditPolicy = await RetrieveStatusCreditPolicy (idCreditPolicy);
            await Task.Delay (200);
            string title = $"¿Está seguro de cambiar el estado de la política de crédito?";
            string message = $"La política de crédito actualmente tiene el estado: ";

            ConfirmationMessageChangeStatusCreditPolicy box = new ConfirmationMessageChangeStatusCreditPolicy (title, message);
            return box.ShowDialog ();
        }

        private static async Task<string> RetrieveStatusCreditPolicy (int idCreditPolicy) {
            MessageResponse<string> messageResponseStatusCreditPolicy = await DAO_CreditPolicy.GetStatusCreditPolicy (idCreditPolicy);
            statusCreditPolicy = messageResponseStatusCreditPolicy.DataRetrieved;
            return messageResponseStatusCreditPolicy.DataRetrieved;
        }

        private void InitializeStatuses () {
            switch (statusCreditPolicy) {
                case "Activo":
                    textBlockStatusCreditPolicy.Text = "Activo";
                    radioButtonOption1.Content = "Inactivo";
                    break;
                case "Inactivo":
                    textBlockStatusCreditPolicy.Text = "Inactivo";
                    radioButtonOption1.Content = "Activo";
                    break;
                default:
                    textBlockStatusCreditPolicy.Text = "---";
                    break;
            }
        }

        private async Task ChangeStatusCreditPolicy (bool newStatus) {
            MessageResponse<bool> messageResponseUpdateStatus = await DAO_CreditPolicy.ChangeStatusCreditPolicy (idCreditPolicy, newStatus);
        }

        private async void OkButton_Click (object sender, RoutedEventArgs e) {
            RadioButton selectedRadioButton = Utils.GetElementVisualTree.GetSelectedRadioButton (stackPanelRadioButtons);

            if (selectedRadioButton == null) {
                MessageBox.Show ("Para poder continuar, seleccione un nuevo estado.", "Selección requerida.");
                return;
            }

            string selectedStatus = selectedRadioButton.Content.ToString ();
            string message = "";
            bool newStatus = false;

            // Mensaje según el estado seleccionado
            switch (selectedStatus) {
                case "Activo":
                    message = "La política de crédito estará activo de nuevo." +
                              "\n¿Desea continuar?";
                    newStatus = true;
                    break;

                case "Inactivo":
                    message = "La política de crédito pasará al estado inactivo." +
                              "\n¿Desea continuar?";
                    newStatus = false;
                    break;

                default:
                    message = "¿Desea continuar?";
                    break;
            }

            // Mostrar el MessageBox de confirmación si se requiere o si no hay crédito activo
            if (MessageBox.Show (
                    message,
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes) {
                await ChangeStatusCreditPolicy (newStatus);

                idCreditPolicy = 0;
                statusCreditPolicy = "";
                DialogResult = true;

                Close ();
            }
        }

        private void CancelButton_Click (object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close ();
        }
    }
}
