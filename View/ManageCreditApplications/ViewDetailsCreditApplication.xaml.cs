using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.CreditApplication;
using FinanciaRed.View.ManageClients;
using FinanciaRed.View.ManageCreditPolicies;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditApplications {
    /// <summary>
    /// Interaction logic for ViewDetailsCreditApplication.xaml
    /// </summary>
    public partial class ViewDetailsCreditApplication : Window {
        private DTO_CreditApplication_Details selectedCreditApplication = null;
        private ObservableCollection<DTO_CreditApplication_CreditPolicies> retrievedCreditPolicies = new ObservableCollection<DTO_CreditApplication_CreditPolicies> ();

        public ViewDetailsCreditApplication (int idCreditApplication) {
            InitializeComponent ();

            _ = RetrieveCreditApplicationDB (idCreditApplication);
            _ = RetrieveCreditPoliciesDB (idCreditApplication);
        }

        private async Task RetrieveCreditPoliciesDB (int idCreditApplication) {
            MessageResponse<List<DTO_CreditApplication_CreditPolicies>> messageResponseConsultCreditPolicies =
                    await DAO_CreditPolicy.GetCreditPolicies_CreditApplitacion (idCreditApplication);
            this.retrievedCreditPolicies = new ObservableCollection<DTO_CreditApplication_CreditPolicies> (messageResponseConsultCreditPolicies.DataRetrieved);
            dataGrid_CreditPolicies.ItemsSource = null;
            dataGrid_CreditPolicies.ItemsSource = retrievedCreditPolicies;
        }

        private async Task RetrieveCreditApplicationDB (int idCreditApplication) {
            MessageResponse<DTO_CreditApplication_Details> messageResponseCreditAppli = await DAO_CreditApplication.GetDetailsCreditApplication (idCreditApplication);
            this.selectedCreditApplication = messageResponseCreditAppli.DataRetrieved;

            if (messageResponseCreditAppli.IsError) {
                MessageBox.Show ("No se logró recuperar los datos de la solicitud de crédito.\nIntente más tarde.", "Error inesperado");
            } else {
                ShowDataCreditAppplication ();
            }
        }

        private void ShowDataCreditAppplication () {
            label_ClientName.Content = $"{selectedCreditApplication.ClientFirstName} {selectedCreditApplication.ClientMiddleName} {selectedCreditApplication.ClientLastName}";
            label_ClientCURP.Content = selectedCreditApplication.CodeCURP;
            label_ClientRFC.Content = selectedCreditApplication.CodeRFC;
            label_NameAdviser.Content = selectedCreditApplication.NameAdviser;
            label_AmountSolicited.Content = selectedCreditApplication.AmountSolicited;
            label_Amountotal.Content = string.Concat (selectedCreditApplication.AmountSolicited + (selectedCreditApplication.AmountSolicited * selectedCreditApplication.InterestRate));
            label_InterestRate.Content = string.Concat (selectedCreditApplication.InterestRate);
            label_NumberFortNigths.Content = string.Concat (selectedCreditApplication.NumberFortNights);
            label_DateApplied.Content = selectedCreditApplication.DateSolicited;
            label_StatusCreditApplcation.Content = selectedCreditApplication.Status;
            textBox_Valoration.Text = selectedCreditApplication.Valoration;
        }

        private void ClickShowDetailsClient (object sender, RoutedEventArgs e) {
            ViewDetailsClient viewDetailsClientWindow = new ViewDetailsClient (selectedCreditApplication.IdClient, false);
            viewDetailsClientWindow.ShowDialog ();
        }

        private void ClicShowDetailsCreditPolicy (object sender, RoutedEventArgs e) {
            Button button = sender as Button;

            if (button.DataContext is DTO_CreditApplication_CreditPolicies rowData) {
                ViewDetailsCreditPolicy viewDetailsCreditPolicyWindow =
                    new ViewDetailsCreditPolicy ((dataGrid_CreditPolicies.SelectedItem as DTO_CreditApplication_CreditPolicies).IdCreditPolicy, false);
                viewDetailsCreditPolicyWindow.ShowDialog ();
            }
        }
    }
}
