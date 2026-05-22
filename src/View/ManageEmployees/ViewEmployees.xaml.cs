using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.View.ManageEmployees;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private void ClickSearchEmployees (object sender, RoutedEventArgs e) {
            //TODO
            //Agregar funcionalidad de filtros
            string keyText = textBox_KeyWord.Text;
            bool WithAdviser = checkBox_WithAdviser.IsChecked ?? false;
            bool WithAnalyst = checkBox_WithCreditAnalyst.IsChecked ?? false;
            bool WithAdministrator = checkBox_WithAdministrator.IsChecked ?? false;

            filteredEmployees = FilterData (retrievedEmployees, keyText, WithAdviser, WithAnalyst, WithAdministrator);
        }

        private ObservableCollection<DTO_Employee_Consult> FilterData (ObservableCollection<DTO_Employee_Consult> filteredEmployees, string keyText, bool WithAdviser, bool WithAnalyst, bool WithAdministrator) {
            return new ObservableCollection<DTO_Employee_Consult> (
                retrievedEmployees.Where (
                    x => FilterPredicate (
                        x, keyText, false))
                );
        }

        private bool FilterPredicate (DTO_Employee_Consult item, string filterText, bool isCaseSensitive) {
            if (string.IsNullOrEmpty (filterText))
                return true;

            string value = item.FirstName.ToString (); // Reemplaza con la propiedad que deseas filtrar

            if (isCaseSensitive)
                return value.Contains (filterText);

            return true;
        }

        private void ClickRegisterEmployee (object sender, RoutedEventArgs e) {
            RegisterEmployee registrerEmployeeWindow = new RegisterEmployee ();
            registrerEmployeeWindow.ShowDialog ();

            _ = RetrieveEmployeesDB ();
        }

        private void ClicShowDetailsEmployee (object sender, RoutedEventArgs e) {
            // Obtener el botón que fue clicado
            Button button = sender as Button;
            // Obtener los datos de la fila a través del DataContext del botón
            //DTO_Employee_Consult rowData = button.DataContext as DTO_Employee_Consult;
            if (button.DataContext is DTO_Employee_Consult rowData) {
                ViewDetailsEmployee viewDetailsEmployeeWindow = new ViewDetailsEmployee (rowData.IdEmployee);
                viewDetailsEmployeeWindow.ShowDialog ();
            }
        }
    }
}
