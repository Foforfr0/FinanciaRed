using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.View.ManagePayments;
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
                 await DAO_Credit.GetAsync ();
            this.retrievedCredits = new ObservableCollection<DTO_Credit_Consult> (messageResponseConsultClients.DataRetrieved);
            dataGridCredits.ItemsSource = null;
            dataGridCredits.ItemsSource = retrievedCredits;
        }

        private async void ClickSearchCredits (object sender, RoutedEventArgs e) {
            string ketText = textBox_KeyWord.Text;

            MessageResponse<List<DTO_Credit_Consult>> messageResponseConsultClients =
                 await DAO_Credit.GetAsync (textBox_KeyWord.Text);
            this.retrievedCredits = new ObservableCollection<DTO_Credit_Consult> (messageResponseConsultClients.DataRetrieved);
            dataGridCredits.ItemsSource = null;
            dataGridCredits.ItemsSource = retrievedCredits;
        }

        private void ClickShowPayments (object sender, RoutedEventArgs e) {
            DTO_Credit_Consult selectedCredit = dataGridCredits.SelectedItem as DTO_Credit_Consult;

            if (selectedCredit == null) {
                MessageBox.Show (
                    "Seleccione un credito primero de la tabla para poder continuar.",
                    "Selección requerida",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                if (selectedCredit.IdStatus == 1) {
                    MessageBox.Show (
                        "El crédito no ha sido iniciado por el cliente,\nel cliente no ha firmado el documento.",
                        "Crédito sin iniciar.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                } else {
                    ViewPayments viewPaymentsWindow = new ViewPayments (selectedCredit.IdCredit);
                    viewPaymentsWindow.ShowDialog ();
                }
            }
        }

        private void ClickChangeState (object sender, RoutedEventArgs e) {
            DTO_Credit_Consult selectedCredits = dataGridCredits.SelectedItem as DTO_Credit_Consult;

            if (selectedCredits == null) {
                MessageBox.Show (
                    "Seleccione un credito primero de la tabla para poder continuar.",
                    "Selección requerida",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                if (selectedCredits.IdStatus == 1) {
                    MessageBox.Show (
                        "El crédito no se ha iniciado formalmente por el cliente\nse requiere de la firma de contrato.",
                        "Crédito sin iniciar.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                } else if (selectedCredits.IdStatus == 4) {
                    MessageBox.Show (
                            "El crédito ya se ha terminado de pagar por el cliente.",
                            "Crédito finalizado.",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                } else {
                    ConfirmationMessageChangeStatusCredit viewPaymentsWindow = new ConfirmationMessageChangeStatusCredit (selectedCredits.IdCredit);
                    viewPaymentsWindow.ShowDialog ();
                }
            }
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
