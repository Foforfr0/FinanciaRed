using FinanciaRed.Model.DTO.CreditPromotion;
using System.Windows;

namespace FinanciaRed.View.ManageCreditPromotions {
    /// <summary>
    /// Interaction logic for ModifyCreditPromotionData.xaml
    /// </summary>
    public partial class ModifyCreditPromotionData : Window {
        private DTO_CreditPromotion_Details selectedPromotion = null;

        public ModifyCreditPromotionData (DTO_CreditPromotion_Details selectedPromotion) {
            InitializeComponent ();

            this.selectedPromotion = selectedPromotion;
            ShowPromotionData ();
        }

        private void ShowPromotionData () {
            textBox_Name.Text = selectedPromotion.Name;
            textBox_InterestRate.Text = string.Concat (selectedPromotion.InterestRate);
            textBox_NumberFortNigths.Text = string.Concat (selectedPromotion.NumberFortNigths);
            datePicker_DateStart.SelectedDate = selectedPromotion.DateStart;
            datePicker_DateEnd.SelectedDate = selectedPromotion.DateEnd;
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar modificación.",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes)
                this.Close ();
        }

        private void ClickFinishModification (object sender, RoutedEventArgs e) {

        }
    }
}
