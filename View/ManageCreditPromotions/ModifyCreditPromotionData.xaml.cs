using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.CreditPromotion;
using System;
using System.Threading.Tasks;
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
            if (datePicker_DateStart.SelectedDate.Value <= DateTime.Now) {
                datePicker_DateStart.IsEnabled = false;
            } else {
                datePicker_DateStart.IsEnabled = true;
            }

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
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (result == MessageBoxResult.Yes)
                this.Close ();
        }

        private bool ValidateForm () {
            bool isCorrect = true;

            if (string.IsNullOrEmpty (textBox_Name.Text)) {
                label_ErrorName.Content = "Campo necesario.";
                label_ErrorName.Visibility = Visibility.Visible;
                isCorrect = false;
            } else {
                label_ErrorName.Content = "";
                label_ErrorName.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty (textBox_NumberFortNigths.Text)) {
                label_ErrorNumberFortNigths.Content = "Campo necesario.";
                label_ErrorNumberFortNigths.Visibility = Visibility.Visible;
                isCorrect = false;
            } else {
                label_ErrorNumberFortNigths.Content = "";
                label_ErrorNumberFortNigths.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty (textBox_InterestRate.Text)) {
                label_ErrorInteresrRate.Content = "Campo necesario.";
                label_ErrorInteresrRate.Visibility = Visibility.Visible;
                isCorrect = false;
            } else {
                label_ErrorInteresrRate.Content = "";
                label_ErrorInteresrRate.Visibility = Visibility.Collapsed;
            }
            if (datePicker_DateStart.SelectedDate.Value == null) {
                label_ErrorDateStart.Content = "Campo necesario.";
                label_ErrorDateStart.Visibility = Visibility.Visible;
                isCorrect = false;
            } else {
                label_ErrorDateStart.Content = "";
                label_ErrorDateStart.Visibility = Visibility.Collapsed;
            }
            if (datePicker_DateEnd.SelectedDate.Value == null) {
                label_ErrorDateEnd.Content = "Campo necesario.";
                label_ErrorDateEnd.Visibility = Visibility.Visible;
                isCorrect = false;
            } else {
                label_ErrorDateEnd.Content = "";
                label_ErrorDateEnd.Visibility = Visibility.Collapsed;
            }

            return isCorrect;
        }

        private async Task SaveDataInDatabase () {
            DTO_CreditPromotion_Details modifyData = new DTO_CreditPromotion_Details {

            };

            MessageResponse<bool> responseModifyProm = await DAO_CreditPromotion.PutAsync (modifyData);
            if (responseModifyProm.IsError) {
                MessageBox.Show (
                    "Ha ocurrido un error inesperado.\nIntente más tarde.", 
                    "Error inesperado.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
                MessageBox.Show (
                    $"Se ha modificado correctamente la \npromoción: \"{modifyData.Name}", 
                    "Modificación completa.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close ();
            }
        }


        private async void ClickFinishModification (object sender, RoutedEventArgs e) {
            if (ValidateForm ()) {
                await SaveDataInDatabase ();
            }
        }
    }
}
