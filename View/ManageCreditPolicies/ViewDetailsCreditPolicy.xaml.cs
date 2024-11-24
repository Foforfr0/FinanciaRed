using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace FinanciaRed.View.ManageCreditPolicies {
    /// <summary>
    /// Interaction logic for ViewDetailsCreditPolicy.xaml
    /// </summary>
    public partial class ViewDetailsCreditPolicy : Window {
        private DTO_CreditPolicy_Consult selectedCreditPolicy = null;

        public ViewDetailsCreditPolicy (int idCreditPolicy, bool canModificate) {
            InitializeComponent ();


            _ = RetrieveCreditPolciesDB (idCreditPolicy, canModificate);
        }

        private async Task RetrieveCreditPolciesDB (int idCreditPolicy, bool canModificate) {
            MessageResponse<DTO_CreditPolicy_Consult> currentCreditPolicy = await DAO_CreditPolicy.GetDetailsCreditPolicy (idCreditPolicy);
            selectedCreditPolicy = currentCreditPolicy.DataRetrieved;
            ShowDataCreditPolicy (canModificate);
        }

        private void ShowDataCreditPolicy (bool canModificate) {
            textBox_Name.Text = selectedCreditPolicy.Name;
            textBox_Description.Text = selectedCreditPolicy.Description;
            datePicker_DateStart.SelectedDate = selectedCreditPolicy.DateStart;
            datePicker_DateEnd.SelectedDate = selectedCreditPolicy.DateEnd;

            button_Modify.Visibility = canModificate ? Visibility.Visible : Visibility.Collapsed;
            stackPanel_DatesData.Visibility = canModificate ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ClickModifyCreditPolicy (object sender, RoutedEventArgs e) {
            textBox_Name.IsReadOnly = false;
            textBox_Description.IsReadOnly = false;
            datePicker_DateStart.IsEnabled = true;
            datePicker_DateEnd.IsEnabled = true;
            button_Modify.Visibility = Visibility.Collapsed;
            grid_ButtonsActions.Visibility = Visibility.Visible;
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar modificación.",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes) {
                ShowDataCreditPolicy (true);

                textBox_Name.IsReadOnly = true;
                textBox_Description.IsReadOnly = true;
                datePicker_DateStart.IsEnabled = false;
                datePicker_DateEnd.IsEnabled = false;
                button_Modify.Visibility = Visibility.Visible;
                grid_ButtonsActions.Visibility = Visibility.Collapsed;
            }
        }

        private void ClickFinishModification (object sender, RoutedEventArgs e) {

        }
    }
}
