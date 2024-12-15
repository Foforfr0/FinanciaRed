using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditPolicies {
    /// <summary>
    /// Interaction logic for ViewDetailsCreditPolicy.xaml
    /// </summary>
    public partial class ViewDetailsCreditPolicy : Window {
        private DTO_CreditPolicy_Consult selectedCreditPolicy = null;

        public ViewDetailsCreditPolicy (int idCreditPolicy, bool canModificate) {
            InitializeComponent ();

            datePicker_DateStart.IsEnabled = false;
            datePicker_DateEnd.IsEnabled = false;
            ShowRequiredTags (false);
            _ = RetrieveCreditPolciesDB (idCreditPolicy, canModificate);
        }

        private async Task RetrieveCreditPolciesDB (int idCreditPolicy, bool canModificate) {
            MessageResponse<DTO_CreditPolicy_Consult> currentCreditPolicy =
                await DAO_CreditPolicy.GetDetailsCreditPolicy (idCreditPolicy);
            selectedCreditPolicy = currentCreditPolicy.DataRetrieved;
            ShowDataCreditPolicy (canModificate);
        }

        private void ShowDataCreditPolicy (bool canModificate) {
            textBox_Name.Text = selectedCreditPolicy.Name;
            textBox_Description.Text = selectedCreditPolicy.Description;
            datePicker_DateStart.SelectedDate = selectedCreditPolicy.DateStart;
            datePicker_DateEnd.SelectedDate = selectedCreditPolicy.DateEnd;

            button_Modify.Visibility = canModificate ? Visibility.Visible : Visibility.Collapsed;
            stackPanel_DatesData.Visibility = canModificate ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ClickModifyCreditPolicy (object sender, RoutedEventArgs e) {
            textBox_Name.IsReadOnly = false;
            textBox_Description.IsReadOnly = false;
            datePicker_DateStart.IsEnabled = true;
            datePicker_DateEnd.IsEnabled = true;
            button_Modify.Visibility = Visibility.Collapsed;
            grid_ButtonsActions.Visibility = Visibility.Visible;
            ShowRequiredTags (true);
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar modificación.",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes) {
                ShowDataCreditPolicy (true);

                textBox_Name.IsReadOnly = true;
                textBox_Description.IsReadOnly = true;
                datePicker_DateStart.IsEnabled = false;
                datePicker_DateEnd.IsEnabled = false;
                button_Modify.Visibility = Visibility.Visible;
                grid_ButtonsActions.Visibility = Visibility.Collapsed;
                ShowRequiredTags (false);
            }
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
                "Seleccione una fecha válida.",
                startDate: DateTime.Now,
                compareError: "",
                1
            );

            // Validar la fecha de finalización
            isFormOk &= ValidateDateField (
                datePicker_DateEnd.SelectedDate,
                label_ErrorDateEnd,
                "Seleccione una fecha válida.",
                startDate: datePicker_DateStart.SelectedDate,
                compareError: "Seleccione una fecha después o igual a la fecha de inicio.",
                2
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
            string emptyError, DateTime? startDate = null,
            string compareError = "", int numDate = 0) {

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
            DTO_CreditPolicy_Consult newPolicy = new DTO_CreditPolicy_Consult {
                IdCreditPolicy = selectedCreditPolicy.IdCreditPolicy,
                Name = textBox_Name.Text,
                Description = textBox_Description.Text,
                DateStart = (DateTime)datePicker_DateStart.SelectedDate,
                DateEnd = datePicker_DateEnd.SelectedDate
            };

            MessageResponse<bool> WasUpdated = await DAO_CreditPolicy.ModifyCreditPolicy (newPolicy);
            if (WasUpdated.DataRetrieved) {
                MessageBox.Show ("No se pudo realizar el registro correctamente de la política de crédito.", "Error inesperado.");
            } else {
                MessageBox.Show ("Se ha registrado correctamente la nueva política de crédito.", "Registro exitoso.");
                this.Close ();
            }
        }

        private async void ClickFinishModification (object sender, RoutedEventArgs e) {
            if (IsFormCorrect ()) {
                await SaveDataInDatabase ();
            }
        }

        private void ShowRequiredTags (bool CanModificate) {
            requiredField1.Visibility = CanModificate ? Visibility.Visible : Visibility.Collapsed;
            requiredField2.Visibility = CanModificate ? Visibility.Visible : Visibility.Collapsed;
            requiredField3.Visibility = CanModificate ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
