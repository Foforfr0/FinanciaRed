using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
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
            dataGridCredits.ItemsSource = null;
            dataGridCredits.ItemsSource = retrievedCredits;
        }

        private void ClickSearchCredits (object sender, RoutedEventArgs e) {
        
        }

        private async void ClickChangeState (object sender, RoutedEventArgs e) {
            
        }

        private void ClickRegisterCredit (object sender, RoutedEventArgs e) {

        }

        private void ClicShowDetailsCredit (object sender, RoutedEventArgs e) {
            Button button = sender as Button;

            if (button.DataContext is DTO_Credit_Consult rowData) {
                ViewDetailsCredit detailsCreditWindow = new ViewDetailsCredit (rowData.IdCredit);
                detailsCreditWindow.ShowDialog ();
            }
        }
    }
}
