using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.View.ManageEmployees;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageUsers {
    /// <summary>
    /// Interaction logic for ViewEmployees.xaml
    /// </summary>
    public partial class ViewEmployees : Page {
        private ObservableCollection<DTO_Employee_Consult> retrievedEmployees = new ObservableCollection<DTO_Employee_Consult> ();
        private ObservableCollection<DTO_Employee_Consult> filteredEmployees = new ObservableCollection<DTO_Employee_Consult> ();

        public ViewEmployees () {
            InitializeComponent ();

            _ = RetrieveEmployeesDB ();
        }

        private async Task RetrieveEmployeesDB () {
            MessageResponse<List<DTO_Employee_Consult>> messageResponseConsultEmployees =
                 await DAO_Employee.GetAllEmployees ();
            this.retrievedEmployees = new ObservableCollection<DTO_Employee_Consult> (messageResponseConsultEmployees.DataRetrieved);
            dataGridEmployees.ItemsSource = null;
            dataGridEmployees.ItemsSource = retrievedEmployees;
        }

        private async void ClickSearchEmployees (object sender, RoutedEventArgs e) {
            string keyText = textBoxKeyWord.Text;

            MessageResponse<List<DTO_Employee_Consult>> messageResponseFilterEmployees =
                await DAO_Employee.GetFilteredEmployees (keyText);

            this.filteredEmployees = new ObservableCollection<DTO_Employee_Consult> (messageResponseFilterEmployees.DataRetrieved);
            dataGridEmployees.ItemsSource = null;
            dataGridEmployees.ItemsSource = this.filteredEmployees;
        }

        private async void ClickChangeState (object sender, RoutedEventArgs e) {
            DTO_Employee_Consult selectedEmployee = dataGridEmployees.SelectedItem as DTO_Employee_Consult;

            if (selectedEmployee == null) {
                MessageBox.Show (
                    "Seleccione un empleado primero de la tabla para poder continuar.",
                    "Selección requerida.");
            } else {
                ConfirmationMessageChangeStatusEmployee.idEmployee = selectedEmployee.IdEmployee;
                bool? result = await ConfirmationMessageChangeStatusEmployee.Show (
                    selectedEmployee.FirstName + " " + selectedEmployee.MiddleName);
                ConfirmationMessageChangeStatusEmployee.idEmployee = 0;
                if (result == true) {
                    MessageBox.Show ("Se ha modificado correctamente el estado del empleado.", "Modificación completa.");
                }
            }
        }

        private void ClickRegisterEmployee (object sender, RoutedEventArgs e) {
            RegisterEmployee registrerEmployeeWindow = new RegisterEmployee ();
            registrerEmployeeWindow.ShowDialog ();

            _ = RetrieveEmployeesDB ();
        }

        private void ClicShowDetailsEmployee (object sender, RoutedEventArgs e) {
            Button button = sender as Button;

            if (button.DataContext is DTO_Employee_Consult rowData) {
                ViewDetailsEmployee viewDetailsEmployeeWindow = new ViewDetailsEmployee (rowData.IdEmployee);
                viewDetailsEmployeeWindow.ShowDialog ();
            }
        }
    }
}
