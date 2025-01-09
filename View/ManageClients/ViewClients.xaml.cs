using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageClients {
    /// <summary>
    /// Interaction logic for ViewClients.xaml
    /// </summary>

    public partial class ViewClients : Page {
        private ObservableCollection<DTO_Client_Consult> retrievedClients = new ObservableCollection<DTO_Client_Consult> ();
        private ObservableCollection<DTO_Client_Consult> filteredClients = new ObservableCollection<DTO_Client_Consult> ();

        public ViewClients () {
            InitializeComponent ();

            _ = RetrieveClientsDB ();
        }

        private async Task RetrieveClientsDB () {
            MessageResponse<List<DTO_Client_Consult>> messageResponseConsultClients =
                 await DAO_Client.GetAsync ();
            this.retrievedClients = new ObservableCollection<DTO_Client_Consult> (messageResponseConsultClients.DataRetrieved);
            dataGridClients.ItemsSource = null;
            dataGridClients.ItemsSource = retrievedClients;
        }

        private async void ClickSearchClients (object sender, RoutedEventArgs e) {
            string keyText = textBoxKeyWord.Text;

            MessageResponse<List<DTO_Client_Consult>> messageResponseFilterClients =
                await DAO_Client.GetAsync (keyText);

            this.filteredClients = new ObservableCollection<DTO_Client_Consult> (messageResponseFilterClients.DataRetrieved);
            dataGridClients.ItemsSource = null;
            dataGridClients.ItemsSource = filteredClients;
        }

        private async void ClickChangeState (object sender, RoutedEventArgs e) {
            DTO_Client_Consult selectedClient = dataGridClients.SelectedItem as DTO_Client_Consult;

            if (selectedClient == null) {
                MessageBox.Show (
                    "Seleccione un cliente primero de la tabla para poder continuar.",
                    "Selección requerida",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                ConfirmationMessageChangeStatusClient.idClient = selectedClient.IdClient;
                bool? result = await ConfirmationMessageChangeStatusClient.Show (
                    selectedClient.FirstName + " " + selectedClient.MiddleName);
                ConfirmationMessageChangeStatusClient.idClient = 0;
                if (result == true) {
                    MessageBox.Show (
                        "Se ha modificado correctamente el estado del cliente.", 
                        "Modificación completa.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ClickRegisterClient (object sender, RoutedEventArgs e) {
            RegisterClient registrerClientWindow = new RegisterClient ();
            registrerClientWindow.ShowDialog ();

            _ = RetrieveClientsDB ();
        }

        private void ClicShowDetailsClient (object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            
            if (button.DataContext is DTO_Client_Consult rowData) {
                ViewDetailsClient viewDetailsClientWindow = new ViewDetailsClient (rowData.IdClient, true);
                viewDetailsClientWindow.ShowDialog ();
            }
        }
    }
}