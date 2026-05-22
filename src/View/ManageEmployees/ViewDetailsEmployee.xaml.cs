using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.View.ManageClients;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace FinanciaRed.View.ManageEmployees {
    /// <summary>
    /// Interaction logic for ViewDetailsEmployee.xaml
    /// </summary>
    public partial class ViewDetailsEmployee : Window {
        private DTO_Employee_DetailsEmployee selectedEmployee = null;

        public ViewDetailsEmployee (int idEmployee) {
            InitializeComponent ();

            _ = LoadDetailsEmployee (idEmployee);
        }

        private async Task RetrieveDataEmployeeDB (int idEmployee) {
            MessageResponse<DTO_Employee_DetailsEmployee> messageResponseDetailsEmployee = await DAO_Employee.GetDetailsEmployee (idEmployee, false);
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
            label_Email.Content = selectedEmployee.Email;
            label_Rol.Content = selectedEmployee.Rol;
        }

        private void ClickModifyEmployee (object sender, EventArgs e) {
            if (selectedEmployee == null) {
                MessageBox.Show ("No se pudo recuperar los datos del empleado.\nIntente más tarde.", "Error inesperado.");
            } else {
                ModifyEmployeeData modifyEmployeeDataWindow = new ModifyEmployeeData (selectedEmployee);
                modifyEmployeeDataWindow.ShowDialog ();
            }
        }
    }
}
