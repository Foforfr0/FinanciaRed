using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.View.ManageClients;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageEmployees {
    /// <summary>
    /// Interaction logic for ConfirmationMessageChangeStatusEmployee.xaml
    /// </summary>
    public partial class ConfirmationMessageChangeStatusEmployee : Window {
        public static int idEmployee = 0;
        private static string statusEmployee = "";

        public ConfirmationMessageChangeStatusEmployee (string title, string message) {
            InitializeComponent ();

            textBlockTitleText.Text = title;
            textBlockMessageText.Text = message;
            InitializeStatuses ();
        }

        public static async Task<bool?> Show (string nameEmployee) {
            statusEmployee = await RetrieveStatusEmployee (idEmployee);
            await Task.Delay (200);
            string title = $"¿Está seguro de cambiar el estado del empleado \"{nameEmployee}\"?";
            string message = $"El empleado actualmente tiene el estado: ";

            ConfirmationMessageChangeStatusEmployee box = new ConfirmationMessageChangeStatusEmployee (title, message);
            return box.ShowDialog ();
        }

        private static async Task<string> RetrieveStatusEmployee (int idEmployee) {
            MessageResponse<string> messageResponseStatusEmployee = await DAO_Employee.GetStatusEmployee (idEmployee);
            statusEmployee = messageResponseStatusEmployee.DataRetrieved;
            return messageResponseStatusEmployee.DataRetrieved;
        }

        private void InitializeStatuses () {
            switch (statusEmployee) {
                case "Activo":
                    textBlockStatusEmployee.Text = "Activo";
                    radioButtonOption1.Content = "Inactivo";
                    radioButtonOption2.Content = "Muerto";
                    break;
                case "Inactivo":
                    textBlockStatusEmployee.Text = "Inactivo";
                    radioButtonOption1.Content = "Activo";
                    radioButtonOption2.Content = "Muerto";
                    break;
                case "Muerto":
                    textBlockStatusEmployee.Text = "Muerto";
                    stackPanelNewStatus.Visibility = Visibility.Collapsed;
                    buttonAccept.Visibility = Visibility.Collapsed;
                    buttonCancel.Content = "Aceptar";
                    break;
                default:
                    textBlockStatusEmployee.Text = "---";
                    break;
            }
        }

        private async Task ChangeStatusEmployee (int idNewStatus) {
            MessageResponse<bool> messageResponseUpdateStatus = await DAO_Employee.ChangeStatusEmployee (idEmployee, idNewStatus);
        }

        private async void OkButton_Click (object sender, RoutedEventArgs e) {
            RadioButton selectedRadioButton = Utils.GetElementVisualTree.GetSelectedRadioButton (stackPanelRadioButtons);

            if (selectedRadioButton == null) {
                MessageBox.Show ("Para poder continuar, seleccione un nuevo estado.", "Selección requerida.");
                return;
            }

            string selectedStatus = selectedRadioButton.Content.ToString ();
            bool confirmationRequired = false;
            string message = "";
            int idNewStatus = 0;

            // Mensaje según el estado seleccionado
            switch (selectedStatus) {
                case "Activo":
                    message = "El empleado estará activo de nuevo." +
                              "\n¿Desea continuar?";
                    idNewStatus = 1;
                    confirmationRequired = true;
                    break;

                case "Inactivo":
                    message = "El empleado pasará al estado inactivo." +
                              "\n¿Desea continuar?";
                    idNewStatus = 2;
                    confirmationRequired = true;
                    break;

                case "Muerto":
                    message = "NO SE PODRÁ VOLVER A CAMBIAR EL ESTADO DEL EMPLEADO.\n¿Desea continuar?";
                    idNewStatus = 3;
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
                await ChangeStatusEmployee (idNewStatus);

                idEmployee = 0;
                statusEmployee = "";
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
