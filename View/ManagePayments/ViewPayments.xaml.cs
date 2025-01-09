using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.Credit;
using FinanciaRed.Utils;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinanciaRed.View.ManagePayments {
    /// <summary>
    /// Interaction logic for ViewPayments.xaml
    /// </summary>
    public partial class ViewPayments : Window {
        private DTO_Credit_Details currentCredit = null;

        public ViewPayments (int idCredit) {
            InitializeComponent ();

            _ = RetrieveData (idCredit);
        }

        private async Task RetrieveData (int idCredit) {
            currentCredit = new DTO_Credit_Details ();
            currentCredit.IdCredit = idCredit;
            MessageResponse<DTO_Credit_Details> responseLayout = await DAO_Credit.GetDetailsAsync (currentCredit.IdCredit);
            currentCredit = responseLayout.DataRetrieved;

            if (currentCredit == null)
                MessageBox.Show (
                    "No se logró obtener el layout de cobros del crédito\no el crédito no se ha iniciado correctamente.",
                    "Sin registro.",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            else
                ShowDataPaymentLayout ();
        }

        private void ShowDataPaymentLayout () {
            try {
                ShowCsvDatagrid.LoadCsvData (currentCredit.PaymentLayout, dataGrid_Payments);
            } catch (Exception ex) {
                MessageBox.Show (
                    $"No se logró cargar el archivo CSV: {ex.Message}",
                    "Error inesperado.",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void PreviewTextInputAmountLeft (object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex ("[^0-9]+");
            e.Handled = regex.IsMatch (e.Text);
        }

        private void ClickLoadLayout (object sender, RoutedEventArgs e) {

        }

        private void ClickSaveLayout (object sender, RoutedEventArgs e) {

        }

        private void ClickCancel (object sender, RoutedEventArgs e) {

        }
    }
}
