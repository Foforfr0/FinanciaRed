using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.Credit;
using FinanciaRed.View.ManageCreditApplications;
using FinanciaRed.View.ManagePayments;
using System.Threading.Tasks;
using System.Windows;

namespace FinanciaRed.View.ManageCredits {
    /// <summary>
    /// Interaction logic for ViewDetailsCredit.xaml
    /// </summary>
    public partial class ViewDetailsCredit : Window {
        private DTO_Credit_Details currentCredit = null;

        public ViewDetailsCredit (int idCredit) {
            InitializeComponent ();

            _ = RetrieveDataDatabase (idCredit);
        }

        // TODO
        private async Task RetrieveDataDatabase (int idCredit) {
            MessageResponse<DTO_Credit_Details> responseCredit = await DAO_Credit.GetDetailsAsync (idCredit);
            currentCredit = responseCredit.DataRetrieved;
            if (currentCredit != null)
                ShowDataCredit ();
        }

        private void ShowDataCredit () {
            label_StatusCredit.Content = currentCredit.Status.ToString ();
            label_SolicitedAmount.Content = "  $ " + currentCredit.CreditApplication.AmountSolicited.ToString ();
            label_LeftAmount.Content = "  $ " + currentCredit.AmountLeft.ToString ();
            label_InterestRate.Content = "  " + currentCredit.CreditApplication.InterestRate.ToString () + " %";
            label_CreditApplicationDate.Content = "  " + currentCredit.CreditApplication.DateSolicited.ToString ("d");
            label_OpinionDate.Content = "  " + currentCredit.CreditApplication.DateAccepted.ToString ("d");
            if (currentCredit.DateStart == null) {
                label_StartCreditDate.Content = "  " + currentCredit.DateStart?.ToString ("d");
            } else {
                label_StartCreditDate.Content = "  El crédito no ha dado inicio.";
            }
            if (currentCredit.DateEnd == null) {
                label_EndCreditDate.Content = "  " + currentCredit.DateEnd?.ToString ("d");
            } else {
                label_EndCreditDate.Content = "  El crédito no ha dado inicio.";
            }
        }

        private void ClickViewCreditApplication (object sender, RoutedEventArgs e) {
            ViewDetailsCreditApplication viewDetailsCreditRequestWindow =
                    new ViewDetailsCreditApplication (currentCredit.CreditApplication.IdCreditApplication);
            viewDetailsCreditRequestWindow.ShowDialog ();
        }

        private void ClickShowPaymentLayout (object sender, RoutedEventArgs e) {
            if (currentCredit.IdStatus == 1) {
                MessageBox.Show (
                    "El crédito no ha sido iniciado por el cliente,\nel cliente no ha firmado el documento.",
                    "Crédito sin iniciar.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            } else {
                ViewPayments viewPaymentsWindow = new ViewPayments (currentCredit.IdCredit);
                viewPaymentsWindow.ShowDialog ();
            }
        }
    }
}
