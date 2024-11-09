using FinanciaRed.Model.DTO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditApplications {
    /// <summary>
    /// Interaction logic for ViewCreditApplications.xaml
    /// </summary>
    public partial class ViewCreditApplications : Page {
        private ObservableCollection<DTO_CreditRequest_Consult> retrievedCreditRequests = new ObservableCollection<DTO_CreditRequest_Consult> ();
        private ObservableCollection<DTO_CreditRequest_Consult> filteredCreditRequests = new ObservableCollection<DTO_CreditRequest_Consult> ();

        public ViewCreditApplications () {
            InitializeComponent ();
        }

        private void ClickSearchCreditApplications (object sender, RoutedEventArgs e) {
            
        }

        private void ClickDefineOpinion (object sender, RoutedEventArgs e) {
        
        }

        private void ClickRegisterCreditApplication (object sender, RoutedEventArgs e) {
            RegistrerCreditApplication creditRequestWindow = new RegistrerCreditApplication ();
            creditRequestWindow.ShowDialog ();
        }

        private void ClicShowDetailsCreditApplication (object sender, RoutedEventArgs e) {
            // Obtener el botón que fue clicado
            Button button = sender as Button;
            // Obtener los datos de la fila a través del DataContext del botón
            //DTO_Client_Consult rowData = button.DataContext as DTO_Client_Consult;
            if (button.DataContext is DTO_CreditRequest_Consult rowData) {
                ViewDetailsCreditApplication viewDetailsCreditRequestWindow = new ViewDetailsCreditApplication (rowData.IdCreditRequest);
                viewDetailsCreditRequestWindow.ShowDialog ();
            }
        }
    }
}
