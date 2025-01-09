using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditPromotions {
    /// <summary>
    /// Interaction logic for ViewCreditPromotions.xaml
    /// </summary>
    public partial class ViewCreditPromotions : Page {
        private ObservableCollection<DTO_CreditPromotion_Consult> retrievedCreditPromotions = new ObservableCollection<DTO_CreditPromotion_Consult> ();

        public ViewCreditPromotions () {
            InitializeComponent ();

            _ = RetrieveCreditPromotionsDB ();
        }

        private async Task RetrieveCreditPromotionsDB () {
            MessageResponse<List<DTO_CreditPromotion_Consult>> messageResponseCreditPromotions =
                await DAO_CreditPromotion.GetAsync ();
            this.retrievedCreditPromotions = new ObservableCollection<DTO_CreditPromotion_Consult> (messageResponseCreditPromotions.DataRetrieved);
            dataGrid_CreditPromotions.ItemsSource = null;
            dataGrid_CreditPromotions.ItemsSource = retrievedCreditPromotions;
        }

        private async void ClickSearchCreditPromotions (object sender, RoutedEventArgs e) {
            string keyText = textBox_KeyWord.Text;

            MessageResponse<List<DTO_CreditPromotion_Consult>> messageResponseFilterCreditPromotions =
                await DAO_CreditPromotion.GetAsync (keyText);

            this.retrievedCreditPromotions = new ObservableCollection<DTO_CreditPromotion_Consult> (messageResponseFilterCreditPromotions.DataRetrieved);
            dataGrid_CreditPromotions.ItemsSource = null;
            dataGrid_CreditPromotions.ItemsSource = retrievedCreditPromotions;
        }

        private async void ClickChangeState (object sender, RoutedEventArgs e) {
            DTO_CreditPromotion_Consult selectedCreditPromotion = dataGrid_CreditPromotions.SelectedItem as DTO_CreditPromotion_Consult;

            if (selectedCreditPromotion == null) {
                MessageBox.Show (
                    "Seleccione una promoción de crédito primero de la tabla para poder continuar.",
                    "Selección requerida",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                ConfirmationMessageChangeStateCreditPromotion.idPromotion = selectedCreditPromotion.IdCreditPromotion;
                bool? result = await ConfirmationMessageChangeStateCreditPromotion.Show (selectedCreditPromotion.Name);
                ConfirmationMessageChangeStateCreditPromotion.idPromotion = 0;
                if (result == true) {
                    MessageBox.Show (
                        "Se ha modificado correctamente la promoción de crédito.", 
                        "Modificación completa.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ClickRegisterPromotion (object sender, RoutedEventArgs e) {
            RegisterCreditPromotion registerPromotionWindow = new RegisterCreditPromotion ();
            registerPromotionWindow.ShowDialog ();

            _ = RetrieveCreditPromotionsDB ();
        }

        private void ClicShowDetailsCreditPromotions (object sender, RoutedEventArgs e) {
            Button button = sender as Button;

            if (button.DataContext is DTO_CreditPromotion_Consult rowData) {
                ViewDetailsCreditPromotion viewDetailsPromotionWindow = new ViewDetailsCreditPromotion (rowData.IdCreditPromotion);
                viewDetailsPromotionWindow.ShowDialog ();
            }
        }
    }
}
