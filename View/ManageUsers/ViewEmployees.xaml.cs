using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.View.ManageClients;
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
        private ObservableCollection<DTO_Client_Consult> retrievedClients = new ObservableCollection<DTO_Client_Consult> ();

        public ViewEmployees () {
            InitializeComponent ();

            _ = RetrieveClientsDB ();
        }

        private async Task RetrieveClientsDB () {
            MessageResponse<List<DTO_Client_Consult>> messageResponseConsultClients =
                 await DAO_Client.GetAllClients ();
            this.retrievedClients = new ObservableCollection<DTO_Client_Consult> (messageResponseConsultClients.DataRetrieved);
            dataGridClients.ItemsSource = null;
            dataGridClients.ItemsSource = retrievedClients;
        }

        private void ClickSearchClients (object sender, RoutedEventArgs e) {

        }

        private void ClickRegisterClient (object sender, RoutedEventArgs e) {
            RegisterClient registerClientWindow = new RegisterClient ();
            registerClientWindow.ShowDialog ();
        }

        private void ClicShowDetailsClient (object sender, RoutedEventArgs e) {
            // Obtener el botón que fue clicado
            Button button = sender as Button;
            // Obtener los datos de la fila a través del DataContext del botón
            //DTO_Client_Consult rowData = button.DataContext as DTO_Client_Consult;
            if (button.DataContext is DTO_Client_Consult rowData) {
                DetailsClient detailsClientWindow = new DetailsClient (rowData.IdClient);
                detailsClientWindow.ShowDialog ();
            }
        }
    }
}
