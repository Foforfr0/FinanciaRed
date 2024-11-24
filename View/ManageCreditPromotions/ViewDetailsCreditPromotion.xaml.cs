using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.CreditPromotion;
using System.Threading.Tasks;
using System.Windows;

namespace FinanciaRed.View.ManageCreditPromotions {
    /// <summary>
    /// Interaction logic for ViewDetailsCreditPromotion.xaml
    /// </summary>
    public partial class ViewDetailsCreditPromotion : Window {
        private DTO_CreditPromotion_Details selectedPromotion = null;

        public ViewDetailsCreditPromotion (int idPromotion) {
            InitializeComponent ();

            _ = LoadDetailsPromotion (idPromotion);
        }

        private async Task LoadDetailsPromotion (int idClient) {
            await RetrieveDataPromotionDB (idClient);
            ShowDataPromotionOverFields ();
        }

        private async Task RetrieveDataPromotionDB (int idPromotion) {
            MessageResponse<DTO_CreditPromotion_Details> messageResponseDetailsPromotion = await DAO_CreditPromotion.GetDetailsCreditPromotion (idPromotion);
            selectedPromotion = messageResponseDetailsPromotion.DataRetrieved;
        }

        private void ShowDataPromotionOverFields () {
            textBlock_Name.Text = selectedPromotion.Name;
            textBlock_NumberFortNigths.Text = selectedPromotion.NumberFortNigths + " quincenas";
            textBlock_InterestRate.Text = selectedPromotion.InterestRate + "%";
            datePicker_DateStart.SelectedDate = selectedPromotion.DateStart;
            datePicker_DateEnd.SelectedDate = selectedPromotion.DateEnd;
        }

        private void ClickModifyPromotion (object sender, RoutedEventArgs e) {
            if (selectedPromotion == null) {
                MessageBox.Show ("No se pudo recuperar los datos de la promoción.\nIntente más tarde.", "Error inesperado.");
            } else {
                ModifyCreditPromotionData modifyPromotionDataWindow = new ModifyCreditPromotionData (selectedPromotion);
                modifyPromotionDataWindow.ShowDialog ();
            }
        }
    }
}
