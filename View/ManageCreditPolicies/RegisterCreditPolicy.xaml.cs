using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditPolicies {
    /// <summary>
    /// Interaction logic for RegisterCreditPolicy.xaml
    /// </summary>
    public partial class RegisterCreditPolicy : Window {
        public RegisterCreditPolicy () {
            InitializeComponent ();

            datePicker_DateStart.DisplayDateStart = DateTime.Now;
            datePicker_DateEnd.DisplayDateStart = DateTime.Now;
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar registro.",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes)
                this.Close ();
        }

        private bool IsFormCorrect () {
            bool isFormOk = true;

            // Validar el nombre
            isFormOk &= ValidateField (
                textBox_Name.Text,
                label_ErrorName,
                "Ingrese un nombre.",
                "Ingrese un nombre más largo.",
                minLength: 10
            );

            // Validar la descripción
            isFormOk &= ValidateField (
                textBox_Description.Text,
                label_ErrorDescription,
                "Ingrese una descripción.",
                "Ingrese una descripción más larga.",
                minLength: 50
            );

            // Validar la fecha de inicio
            isFormOk &= ValidateDateField (
                datePicker_DateStart.SelectedDate,
                label_ErrorDateStart,
                "Seleccione una fecha válida."
            );

            // Validar la fecha de finalización
            isFormOk &= ValidateDateField (
                datePicker_DateEnd.SelectedDate,
                label_ErrorDateEnd,
                "Seleccione una fecha válida.",
                startDate: datePicker_DateStart.SelectedDate,
                compareError: "Seleccione una fecha después o igual a la fecha de inicio."
            );

            return isFormOk;
        }

        private bool ValidateField (
            string input, Label errorLabel,
            string emptyError, string lengthError,
            int minLength = 0) {

            if (string.IsNullOrEmpty (input)) {
                ShowError (errorLabel, emptyError);
                return false;
            } else if (input.Length <= minLength) {
                ShowError (errorLabel, lengthError);
                return false;
            } else {
                HideError (errorLabel);
                return true;
            }
        }

        private bool ValidateDateField (
            DateTime? date, Label errorLabel,
            string emptyError, int numDate = 0, DateTime? startDate = null,
            string compareError = "") {

            if (date == null && numDate == 1) {
                ShowError (errorLabel, emptyError);
                return false;
            } else if (startDate != null && date < startDate) {
                ShowError (errorLabel, compareError);
                return false;
            } else {
                HideError (errorLabel);
                return true;
            }
        }

        private void ShowError (Label label, string message) {
            label.Content = message;
            label.Visibility = Visibility.Visible;
        }

        private void HideError (Label label) {
            label.Content = "";
            label.Visibility = Visibility.Collapsed;
        }


        private async Task SaveDataInDatabase () {
            DTO_CreditPolicy_Consult newCreditPolicy = new DTO_CreditPolicy_Consult () {
                Name = textBox_Name.Text,
                Description = textBox_Description.Text,
                DateStart = datePicker_DateStart.SelectedDate ?? DateTime.Now,
                DateEnd = datePicker_DateEnd.SelectedDate,
            };

            MessageResponse<bool> responseRegisterCreditPolicy = await DAO_CreditPolicy.RegistryNewCreditPolicy (newCreditPolicy);
            if (responseRegisterCreditPolicy.IsError) {
                MessageBox.Show ("Ha ocurrido un error inesperado.\nIntente más tarde.", "Error inesperado.");
            } else {
                MessageBox.Show ($"Se ha registrado correctamente la nueva política de crédito.", "Modificación completa.");
                this.Close ();
            }
        }

        private async void ClickFinishRegistration (object sender, RoutedEventArgs e) {
            if (IsFormCorrect ()) {
                await SaveDataInDatabase ();
            }
        }
    }
}
