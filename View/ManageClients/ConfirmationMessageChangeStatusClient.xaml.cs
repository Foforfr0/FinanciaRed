using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageClients {
    /// <summary>
    /// Interaction logic for ConfirmationMessageChangeStatusClient.xaml
    /// </summary>
    public partial class ConfirmationMessageChangeStatusClient : Window {
        public static int idClient = 0;
        private static string statusClient = "";
        private static bool HaveActiveCredit = false;
        private static string haveDontHaveCredit = "";

        public ConfirmationMessageChangeStatusClient (string title, string message) {
            InitializeComponent ();

            textBlock_TitleText.Text = title;
            textBlock_MessageText.Text = message;
            textBlock_HaveDontHaveCredit.Text = haveDontHaveCredit;

            InitializeStatuses ();
        }

        public static async Task<bool?> Show (string nameClient) {
            statusClient = await RetrieveStatusClient (idClient);
            await Task.Delay (200);
            haveDontHaveCredit = await RetrieveActiveCreditClient (idClient);
            await Task.Delay (200);
            string title = $"¿Está seguro de cambiar el estado del cliente \"{nameClient}\"?";
            string message = $"El cliente actualmente tiene el estado: ";

            ConfirmationMessageChangeStatusClient box = new ConfirmationMessageChangeStatusClient (title, message);
            return box.ShowDialog ();
        }

        private static async Task<string> RetrieveStatusClient (int idClient) {
            MessageResponse<string> messageResponseStatusClient = await DAO_Client.GetStatusAsync (idClient);
            statusClient = messageResponseStatusClient.DataRetrieved;
            return messageResponseStatusClient.DataRetrieved;
        }

        private static async Task<string> RetrieveActiveCreditClient (int idClient) {
            MessageResponse<bool> messageResponseCreditActiveClient = await DAO_Credit.GetStatusCreditClientAsync (idClient);
            HaveActiveCredit = messageResponseCreditActiveClient.DataRetrieved;
            return messageResponseCreditActiveClient.DataRetrieved ? "SI TIENE " : "NO TIENE ";
        }

        private void InitializeStatuses () {
            switch (statusClient) {
                case "Activo":
                    textBlock_StatusClient.Text = "Activo";
                    radioButton_Option1.Content = "Inactivo";
                    radioButtonOption2.Content = "Muerto";
                    break;
                case "Inactivo":
                    textBlock_StatusClient.Text = "Inactivo";
                    radioButton_Option1.Content = "Activo";
                    radioButtonOption2.Content = "Muerto";
                    break;
                case "Muerto":
                    textBlock_StatusClient.Text = "Muerto";
                    stackPanel_NewStatus.Visibility = Visibility.Collapsed;
                    button_Accept.Visibility = Visibility.Collapsed;
                    button_Cancel.Content = "Aceptar";
                    break;
                default:
                    textBlock_StatusClient.Text = "---";
                    break;
            }
        }

        private async Task ChangeStatusClient (int idNewStatus) {
            MessageResponse<bool> messageResponseUpdateStatus = await DAO_Client.PutAsync (idClient, idNewStatus);
        }

        private async Task ChangeStatusCredit (int idNewStatusCredit) {
            MessageResponse<int> messageResponseUpdateStatues = await DAO_Credit.PutStatusCreditsClientAsync (idClient, idNewStatusCredit);
        }

        private async void OkButton_Click (object sender, RoutedEventArgs e) {
            RadioButton selectedRadioButton = Utils.GetElementVisualTree.GetSelectedRadioButton (stackPanelRadioButtons);

            if (selectedRadioButton == null) {
                MessageBox.Show (
                    "Para poder continuar, seleccione un nuevo estado.", 
                    "Selección requerida.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedStatus = selectedRadioButton.Content.ToString ();
            bool confirmationRequired = false;
            string message = "";
            int idNewStatus = 0;

            // Mensaje según el estado seleccionado
            switch (selectedStatus) {
                case "Activo":
                    message = "El cliente estará activo de nuevo." +
                              "\n¿Desea continuar?";
                    idNewStatus = 1;
                    confirmationRequired = true;
                    break;

                case "Inactivo":
                    if (HaveActiveCredit) {
                        message = "El cliente tiene un crédito activo, si continúa: " +
                                  "    ->El crédito se inhabilitará." +
                                  "\n¿Desea continuar?";
                        confirmationRequired = true;
                    } else {
                        message = "¿Desea continuar?";
                    }
                    idNewStatus = 2;
                    break;

                case "Muerto":
                    if (HaveActiveCredit) {
                        message = "El cliente tiene un crédito activo, si continúa: " +
                                  "\n    ->El crédito se inhabilitará." +
                                  "\n    ->NO SE PODRÁ VOLVER A CAMBIAR EL ESTADO DEL CLIENTE." +
                                  "\n¿Desea continuar?";
                        confirmationRequired = true;
                    } else {
                        message = "NO SE PODRÁ VOLVER A CAMBIAR EL ESTADO DEL CLIENTE.\n¿Desea continuar?";
                    }
                    idNewStatus = 3;
                    break;

                default:
                    message = "¿Desea continuar?";
                    break;
            }

            // Mostrar el MessageBox de confirmación si se requiere o si no hay crédito activo
            if (MessageBox.Show (
                    message,
                    confirmationRequired ? "Acción no reversible" : "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes) {
                await ChangeStatusClient (idNewStatus);
                if (HaveActiveCredit && (idNewStatus == 2 || idNewStatus == 3))
                    await ChangeStatusCredit (2);
                else
                    await ChangeStatusCredit (1);

                statusClient = "";
                HaveActiveCredit = false;
                idClient = 0;
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