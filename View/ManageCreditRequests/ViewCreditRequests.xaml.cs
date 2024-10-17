using FinanciaRed.Model.DTO;
using FinanciaRed.View.ManageClients;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditRequests {
    /// <summary>
    /// Interaction logic for ViewCreditRequests.xaml
    /// </summary>
    public partial class ViewCreditRequests : Page {
        private ObservableCollection<DTO_CreditRequest_Consult> retrievedCreditRequests = new ObservableCollection<DTO_CreditRequest_Consult> ();
        private ObservableCollection<DTO_CreditRequest_Consult> filteredCreditRequests = new ObservableCollection<DTO_CreditRequest_Consult> ();

        public ViewCreditRequests () {
            InitializeComponent ();
        }

        private void ClickSearchCreditRequests (object sender, RoutedEventArgs e) {
            //TODO
            //Agregar funcionalidad de filtros
            string keyText = textBox_KeyWord.Text;

            filteredCreditRequests = FilterData (filteredCreditRequests, keyText);
        }

        private ObservableCollection<DTO_CreditRequest_Consult> FilterData (ObservableCollection<DTO_CreditRequest_Consult> filteredCreditRequests, string keyText) {
            return new ObservableCollection<DTO_CreditRequest_Consult> (
                retrievedCreditRequests.Where (
                    x => FilterPredicate (
                        x, keyText))
                );
        }

        private bool FilterPredicate (DTO_CreditRequest_Consult item, string filterText) {
            if (string.IsNullOrEmpty (filterText))
                return true;

            return item.ClientName.Contains (filterText);
        }

        public void ClickRegisterCreditRequest (object sender, RoutedEventArgs e) {
            RegistrerCreditRequest creditRequestWindow = new RegistrerCreditRequest ();
            creditRequestWindow.ShowDialog ();
        }

        private void ClicShowDetailsCreditRequest (object sender, RoutedEventArgs e) {
            // Obtener el botón que fue clicado
            Button button = sender as Button;
            // Obtener los datos de la fila a través del DataContext del botón
            //DTO_Client_Consult rowData = button.DataContext as DTO_Client_Consult;
            if (button.DataContext is DTO_CreditRequest_Consult rowData) {
                ViewDetailsCreditRequest viewDetailsCreditRequestWindow = new ViewDetailsCreditRequest (rowData.IdCreditRequest);
                viewDetailsCreditRequestWindow.ShowDialog ();
            }
        }
    }
}
