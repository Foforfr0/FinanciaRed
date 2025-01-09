using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditPolicies {
    /// <summary>
    /// Interaction logic for ViewCreditPolicies.xaml
    /// </summary>
    public partial class ViewCreditPolicies : Page {
        private ObservableCollection<DTO_CreditPolicy_Consult> retrievedCreditPolicies = new ObservableCollection<DTO_CreditPolicy_Consult> ();

        public ViewCreditPolicies () {
            InitializeComponent ();

            _ = RetrieveCreditPoliciesDB ();
        }

        private async Task RetrieveCreditPoliciesDB () {
            MessageResponse<List<DTO_CreditPolicy_Consult>> messageResponseConsultCreditPolicies =
                    await DAO_CreditPolicy.GetAsync ();
            this.retrievedCreditPolicies = new ObservableCollection<DTO_CreditPolicy_Consult> (messageResponseConsultCreditPolicies.DataRetrieved);
            dataGrid_CreditPolicies.ItemsSource = null;
            dataGrid_CreditPolicies.ItemsSource = retrievedCreditPolicies;
        }

        private async void ClickSearchCreditPolicies (object sender, RoutedEventArgs e) {
            string keyText = textBoxKeyWord.Text;

            MessageResponse<List<DTO_CreditPolicy_Consult>> messageResponseConsultCreditPolicies =
                await DAO_CreditPolicy.GetAsync (keyText);

            this.retrievedCreditPolicies = new ObservableCollection<DTO_CreditPolicy_Consult> (messageResponseConsultCreditPolicies.DataRetrieved);
            dataGrid_CreditPolicies.ItemsSource = null;
            dataGrid_CreditPolicies.ItemsSource = this.retrievedCreditPolicies;
        }

        private async void ClickChangeState (object sender, RoutedEventArgs e) {
            DTO_CreditPolicy_Consult selectedCreditPolicy = dataGrid_CreditPolicies.SelectedItem as DTO_CreditPolicy_Consult;

            if (selectedCreditPolicy == null) {
                MessageBox.Show (
                    "Seleccione una política de crédito primero de la tabla para poder continuar.",
                    "Selección requerida.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                ConfirmationMessageChangeStatusCreditPolicy.idCreditPolicy = selectedCreditPolicy.IdCreditPolicy;
                bool? result = await ConfirmationMessageChangeStatusCreditPolicy.Show ();
                ConfirmationMessageChangeStatusCreditPolicy.idCreditPolicy = 0;

                if (result == true) {
                    MessageBox.Show (
                        "Se ha modificado correctamente el estado de la política de crédito.", 
                        "Modificación completa.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ClickRegisterCreditPolicy (object sender, RoutedEventArgs e) {
            RegisterCreditPolicy registerCreditPolicyWindow = new RegisterCreditPolicy ();
            registerCreditPolicyWindow.ShowDialog ();

            _ = RetrieveCreditPoliciesDB ();
        }

        private void ClicShowDetailsCreditPolicy (object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            
            if (button.DataContext is DTO_CreditPolicy_Consult rowData) {
                ViewDetailsCreditPolicy viewDetailsCreditPolicyWindow =
                    new ViewDetailsCreditPolicy ((dataGrid_CreditPolicies.SelectedItem as DTO_CreditPolicy_Consult).IdCreditPolicy, true);
                viewDetailsCreditPolicyWindow.ShowDialog ();
            }
        }
    }
}
