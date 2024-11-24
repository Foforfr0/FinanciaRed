using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditApplications {
    /// <summary>
    /// Interaction logic for ViewCreditApplications.xaml
    /// </summary>
    public partial class ViewCreditApplications : Page {
        private ObservableCollection<DTO_CreditApplication_Consult> retrievedCreditApplications = new ObservableCollection<DTO_CreditApplication_Consult> ();

        public ViewCreditApplications () {
            InitializeComponent ();

            _ = RetrieveCreditApplicationsDB ();
        }

        private async Task RetrieveCreditApplicationsDB () {
            MessageResponse<List<DTO_CreditApplication_Consult>> messageResponseConsultCreditPolicies =
                    await DAO_CreditApplication.GetAllCreditApplications ();

            this.retrievedCreditApplications = new ObservableCollection<DTO_CreditApplication_Consult> (messageResponseConsultCreditPolicies.DataRetrieved);
            dataGrid_CreditAplications.ItemsSource = null;
            dataGrid_CreditAplications.ItemsSource = retrievedCreditApplications;
        }

        private async void ClickSearchCreditApplications (object sender, RoutedEventArgs e) {
            string keyText = textBoxKeyWord.Text;

            MessageResponse<List<DTO_CreditApplication_Consult>> messageResponseConsultCreditPolicies =
                await DAO_CreditApplication.GetFilteredCreditApplications (keyText);

            this.retrievedCreditApplications = new ObservableCollection<DTO_CreditApplication_Consult> (messageResponseConsultCreditPolicies.DataRetrieved);
            dataGrid_CreditAplications.ItemsSource = null;
            dataGrid_CreditAplications.ItemsSource = this.retrievedCreditApplications;
        }

        private void ClickDefineOpinion (object sender, RoutedEventArgs e) {
            DTO_CreditApplication_Consult selectedCreditApplication = dataGrid_CreditAplications.SelectedItem as DTO_CreditApplication_Consult;

            if (selectedCreditApplication == null) {
                MessageBox.Show (
                    "Seleccione una solicitud de crédito primero de la tabla para poder continuar.",
                    "Selección requerida");
            } else if (selectedCreditApplication.Status != "Aplicado") {
                MessageBox.Show (
                    "Ya se ha aplicado un dictámen a la solicitud de crédito.\nSeleccione otra.",
                    "Ditámen ya realizado");
            } else {
                DefineOpinion defineOpinionWindow = new DefineOpinion (selectedCreditApplication.IdCreditApplication);
                defineOpinionWindow.ShowDialog ();
            }

            _ = RetrieveCreditApplicationsDB ();
        }

        private void ClickRegisterCreditApplication (object sender, RoutedEventArgs e) {
            RegistrerCreditApplication registerCreditApplicationWindow = new RegistrerCreditApplication ();
            registerCreditApplicationWindow.ShowDialog ();

            _ = RetrieveCreditApplicationsDB ();
        }

        private void ClicShowDetailsCreditApplication (object sender, RoutedEventArgs e) {
            Button button = sender as Button;

            if (button.DataContext is DTO_CreditApplication_Consult rowData) {
                ViewDetailsCreditApplication viewDetailsCreditRequestWindow =
                    new ViewDetailsCreditApplication (rowData.IdCreditApplication);
                viewDetailsCreditRequestWindow.ShowDialog ();
            }
        }
    }
}
