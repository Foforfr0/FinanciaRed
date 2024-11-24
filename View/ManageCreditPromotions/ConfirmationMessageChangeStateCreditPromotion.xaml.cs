using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditPromotions {
    /// <summary>
    /// Interaction logic for ConfirmationMessageChangeStateCreditPromotion.xaml
    /// </summary>
    public partial class ConfirmationMessageChangeStateCreditPromotion : Window {
        public static int idPromotion = 0;
        private static string statusPromotion = "";


        public ConfirmationMessageChangeStateCreditPromotion (string title, string message) {
            InitializeComponent ();

            textBlock_TitleText.Text = title;
            textBlock_MessageText.Text = message;
            InitializeStatuses ();
        }

        public static async Task<bool?> Show (string namePromotion) {
            statusPromotion = await RetrieveStatusPromotion (idPromotion);
            await Task.Delay (200);

            string title = $"¿Está seguro de cambiar el estado de la promoción \"{namePromotion}\"?";
            string message = $"La promoción actualmente tiene el estado: ";

            ConfirmationMessageChangeStateCreditPromotion box = new ConfirmationMessageChangeStateCreditPromotion (title, message);
            return box.ShowDialog ();
        }

        private static async Task<string> RetrieveStatusPromotion (int idPromotion) {
            MessageResponse<string> messageResponseStatusPromotion = await DAO_CreditPromotion.GetStatusCreditPromotion(idPromotion);
            statusPromotion = messageResponseStatusPromotion.DataRetrieved;
            return messageResponseStatusPromotion.DataRetrieved;
        }

        private void InitializeStatuses () {
            switch (statusPromotion) {
                case "Activo":
                    textBlock_StatusPromotion.Text = "Activo";
                    radioButton_Option1.Content = "Inactivo";
                    break;
                case "Inactivo":
                    textBlock_StatusPromotion.Text = "Inactivo";
                    radioButton_Option1.Content = "Activo";
                    break;
                default:
                    textBlock_StatusPromotion.Text = "---";
                    break;
            }
        }

        private async Task ChangeStatusCreditPromotion (bool isActive) {
            MessageResponse<bool> messageResponseUpdateStatus = await DAO_CreditPromotion.ChangeStatusCreditPromotion(idPromotion, isActive);
        }

        private async void ClickAccept (object sender, RoutedEventArgs e) {
            RadioButton selectedRadioButton = Utils.GetElementVisualTree.GetSelectedRadioButton (stackPanelRadioButtons);

            if (selectedRadioButton == null) {
                MessageBox.Show ("Para poder continuar, seleccione un nuevo estado.", "Selección requerida.");
                return;
            }

            string selectedStatus = selectedRadioButton.Content.ToString ();
            bool confirmationRequired = false;
            string message = "";
            bool isActive = true;

            // Mensaje según el estado seleccionado
            switch (selectedStatus) {
                case "Activo":
                    message = "La promoción estará activo de nuevo." +
                              "\n¿Desea continuar?";
                    isActive = true;
                    confirmationRequired = true;
                    break;

                case "Inactivo":
                    message = "La promoción pasará a estado inactivo." +
                              "\n¿Desea continuar?";
                    isActive = false;
                    confirmationRequired = true;
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
                await ChangeStatusCreditPromotion (isActive);

                statusPromotion = "";
                idPromotion = 0;
                DialogResult = true;

                Close ();
            }
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close ();
        }
    }
}
