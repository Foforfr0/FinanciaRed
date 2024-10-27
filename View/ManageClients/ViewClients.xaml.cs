﻿using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageClients {
    /// <summary>
    /// Interaction logic for ViewClients.xaml
    /// </summary>
    /// <TODO>
    /// *Modificar para que al iniciar la ventana no se obtengan todos los elementos 
    ///  de la base de datos, solo de la búsqueda.
    /// *Mostrar los resultados por una tabla con índice.
    /// </TODO>

    public partial class ViewClients : Page {
        private ObservableCollection<DTO_Client_Consult> retrievedClients = new ObservableCollection<DTO_Client_Consult> ();
        private ObservableCollection<DTO_Client_Consult> filteredClients = new ObservableCollection<DTO_Client_Consult> ();

        public ViewClients () {
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
            //TODO
            //Agregar funcionalidad de filtros
            string keyText = textBox_KeyWord.Text;

            filteredClients = FilterData (retrievedClients, keyText);
        }

        private ObservableCollection<DTO_Client_Consult> FilterData (ObservableCollection<DTO_Client_Consult> filteredClients, string keyText) {
            return new ObservableCollection<DTO_Client_Consult> (
                retrievedClients.Where (
                    x => x.FirstName.Contains(keyText)
                )
            );
        }

        private bool FilterPredicate (DTO_Client_Consult item, string filterText, bool isCaseSensitive) {
            if (string.IsNullOrEmpty (filterText))
                return true;

            string value = item.FirstName.ToString (); // Reemplaza con la propiedad que deseas filtrar

            if (isCaseSensitive)
                return value.Contains (filterText);

            return true;
        }

        private void ClickRegisterClient (object sender, RoutedEventArgs e) {
            RegisterClient registrerClientWindow = new RegisterClient ();
            registrerClientWindow.ShowDialog ();

            _ = RetrieveClientsDB ();
        }

        private void ClicShowDetailsClient (object sender, RoutedEventArgs e) {
            // Obtener el botón que fue clicado
            Button button = sender as Button;
            // Obtener los datos de la fila a través del DataContext del botón
            //DTO_Client_Consult rowData = button.DataContext as DTO_Client_Consult;
            if (button.DataContext is DTO_Client_Consult rowData) {
                ViewDetailsClient viewDetailsClientWindow = new ViewDetailsClient (rowData.IdClient);
                viewDetailsClientWindow.ShowDialog ();
            }
        }
    }
}