using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace FinanciaRed.View.ManageEmployees {
    /// <summary>
    /// Interaction logic for ViewDetailsEmployee.xaml
    /// </summary>
    public partial class ViewDetailsEmployee : Window {
        private DTO_Employee_Details selectedEmployee = null;

        public ViewDetailsEmployee (int idEmployee) {
            InitializeComponent ();

            _ = LoadDetailsEmployee (idEmployee);
        }

        private async Task RetrieveDataEmployeeDB (int idEmployee) {
            MessageResponse<DTO_Employee_Details> messageResponseDetailsEmployee = await DAO_Employee.GetAsync (idEmployee, false);
            selectedEmployee = messageResponseDetailsEmployee.DataRetrieved;
        }

        private async Task LoadDetailsEmployee (int idEmployee) {
            await RetrieveDataEmployeeDB (idEmployee);
            ShowDataEmployeeOverFields ();
        }

        private void ShowDataEmployeeOverFields () {
            label_FirstName.Content = selectedEmployee.FirstName;
            label_MiddleName.Content = selectedEmployee.MiddleName;
            label_LastName.Content = selectedEmployee.LastName;
            datePicker_DateBirth.SelectedDate = selectedEmployee.DateBirth;
            label_Gender.Content = selectedEmployee.Gender.Equals ("M") ? "Masculino" : "Femenino";
            label_CodeRFC.Content = selectedEmployee.CodeRFC;
            label_CodeCurp.Content = selectedEmployee.CodeCURP;
            label_Email.Content = selectedEmployee.Email;
            label_Role.Content = selectedEmployee.Rol;
        }

        private void ClickModifyEmployee (object sender, EventArgs e) {
            if (selectedEmployee == null) {
                MessageBox.Show (
                    "No se pudo recuperar los datos del empleado.\nIntente más tarde.", 
                    "Error inesperado.", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
                ModifyEmployeeData modifyEmployeeDataWindow = new ModifyEmployeeData (selectedEmployee);
                modifyEmployeeDataWindow.ShowDialog ();
            }
        }
    }
}
