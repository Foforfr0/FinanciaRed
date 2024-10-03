using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.View.ManageClients;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCredits {
    /// <summary>
    /// Interaction logic for ViewCredits.xaml
    /// </summary>
    public partial class ViewCredits : Page {
        private ObservableCollection<DTO_Credit_Consult> retrievedCredits = new ObservableCollection<DTO_Credit_Consult> ();

        public ViewCredits () {
            InitializeComponent ();

            _ = RetrieveCreditsDB ();
        }

        private async Task RetrieveCreditsDB () {
            MessageResponse<List<DTO_Credit_Consult>> messageResponseConsultClients =
                 await DAO_Credit.GetAllCredits ();
            this.retrievedCredits = new ObservableCollection<DTO_Credit_Consult> (messageResponseConsultClients.DataRetrieved);
            dataGrid_Credits.ItemsSource = null;
            dataGrid_Credits.ItemsSource = retrievedCredits;
        }

        private void ClickSearchCredits (object sender, RoutedEventArgs e) {
        
        }

        private void ClickRegisterCredit (object sender, RoutedEventArgs e) {
            RegistrerCredit registerCreditWindow = new RegistrerCredit ();
            registerCreditWindow.ShowDialog ();
        }

        private void ClicShowDetailsCredit (object sender, RoutedEventArgs e) {
            // Obtener el botón que fue clicado
            Button button = sender as Button;
            // Obtener los datos de la fila a través del DataContext del botón
            //DTO_Credit_Consult rowData = button.DataContext as DTO_Credit_Consult;
            if (button.DataContext is DTO_Credit_Consult rowData) {
                DetailsClient detailsClientWindow = new DetailsClient (rowData.IdCredit);
                detailsClientWindow.ShowDialog ();
            }
        }
    }
}
