using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.MonthlyEfficiencies;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageEfficiencies {
    /// <summary>
    /// Interaction logic for ViewEfficiencies.xaml
    /// </summary>
    public partial class ViewEfficiencies : Page {
        private DTO_Efficiencies efficiencies = null;

        public ViewEfficiencies () {
            InitializeComponent ();

            datePicker_DateStart.DisplayDateEnd = DateTime.Now;
            datePicker_DateEnd.DisplayDateEnd = DateTime.Now;
        }

        private async void ClickRetrieveEfficiencies (object sender, RoutedEventArgs e) {
            if (datePicker_DateStart.SelectedDate == null || datePicker_DateStart.SelectedDate == null) {
                MessageBox.Show ("Para realizar la consulta de las eficiencias,\nindique la fecha de inicio y fin.", "Fechas requeridas.");
            } else if (datePicker_DateEnd.SelectedDate.Value < datePicker_DateStart.SelectedDate.Value) {
                MessageBox.Show ("La fecha de fin debe ser posterior o igual a la fecha de inicio.", "Fechas no aceptables.");
            } else {
                await RetrieveEfficienciesDB ();
            }
        }

        private async Task RetrieveEfficienciesDB () {
            MessageResponse<DTO_Efficiencies> messageResponseConsutlEfficiences =
                    new MessageResponse<DTO_Efficiencies> ();
            messageResponseConsutlEfficiences = await DAO_Efficiencies.GetEfficiencies (new DTO_Efficiencies {
                DateStart = datePicker_DateStart.SelectedDate ?? DateTime.Now,
                DateEnd = datePicker_DateEnd.SelectedDate ?? DateTime.Now
            });
            if (!messageResponseConsutlEfficiences.IsError) {
                efficiencies = messageResponseConsutlEfficiences.DataRetrieved;

                label_CreditsApplicatedNumb.Content = efficiencies.CreditsApplicatedNumb.ToString ();
                label_CreditsApplicatedPercent.Content = efficiencies.CreditsApplicatedPercent.ToString () + " %";
                label_CreditsAcceptedNumb.Content = efficiencies.CreditsAcceptedNumb.ToString ();
                label_CreditsAcceptedPercent.Content = efficiencies.CreditsAcceptedPercent.ToString () + " %";
                label_CreditsDeclinedNumb.Content = efficiencies.CreditsDeclinedNumb.ToString ();
                label_CreditsDeclinedPercent.Content = efficiencies.CreditsDeclinedPercent.ToString () + " %";
                label_CreditsAwaitingNumb.Content = efficiencies.CreditsAwaitingNumb.ToString ();
                label_CreditsAwaitingPercent.Content = efficiencies.CreditsAwaitingPercent.ToString () + " %";
                label_TotalAmountCredits.Content = "$ " + efficiencies.TotalAmountCredits.ToString ();
                label_AverageAmountCredit.Content = "$ " + efficiencies.AverageAmountCredit.ToString ();
                label_MaximumAmountCredit.Content = "$ " + efficiencies.MaximumAmountCredit.ToString ();
            } else {
                label_CreditsApplicatedNumb.Content = "0";
                label_CreditsApplicatedPercent.Content = "0";
                label_CreditsAcceptedNumb.Content = "0";
                label_CreditsAcceptedPercent.Content = "0";
                label_CreditsDeclinedNumb.Content = "0";
                label_CreditsDeclinedPercent.Content = "0";
                label_CreditsAwaitingNumb.Content = "0";
                label_CreditsAwaitingPercent.Content = "0";
                label_TotalAmountCredits.Content = "0";
                label_AverageAmountCredit.Content = "0";
                label_MaximumAmountCredit.Content = "0";

                MessageBox.Show (messageResponseConsutlEfficiences.Message, "Error inesperado.");
            }
        }

        private void ShowEffficiencies () {
            if (efficiencies.CreditsApplicatedNumb == 0 || efficiencies.CreditsApplicatedNumb == null) {
                label_CreditsApplicatedNumb.Content = "0";
                label_CreditsApplicatedPercent.Content = "0";
                label_CreditsAcceptedNumb.Content = "0";
                label_CreditsAcceptedPercent.Content = "0";
                label_CreditsDeclinedNumb.Content = "0";
                label_CreditsDeclinedPercent.Content = "0";
                label_CreditsAwaitingNumb.Content = "0";
                label_CreditsAwaitingPercent.Content = "0";
                label_TotalAmountCredits.Content = "0";
                label_AverageAmountCredit.Content = "0";
                label_MaximumAmountCredit.Content = "0";
            } else {
                label_CreditsApplicatedNumb.Content = efficiencies.CreditsApplicatedNumb.ToString ();
                label_CreditsApplicatedPercent.Content = efficiencies.CreditsApplicatedPercent.ToString () + " %";
                label_CreditsAcceptedNumb.Content = efficiencies.CreditsAcceptedNumb.ToString ();
                label_CreditsAcceptedPercent.Content = efficiencies.CreditsAcceptedPercent.ToString () + " %";
                label_CreditsDeclinedNumb.Content = efficiencies.CreditsDeclinedNumb.ToString ();
                label_CreditsDeclinedPercent.Content = efficiencies.CreditsDeclinedPercent.ToString () + " %";
                label_CreditsAwaitingNumb.Content = efficiencies.CreditsAwaitingNumb.ToString ();
                label_CreditsAwaitingPercent.Content = efficiencies.CreditsAwaitingPercent.ToString () + " %";
                label_TotalAmountCredits.Content = "$ " + efficiencies.TotalAmountCredits.ToString ();
                label_AverageAmountCredit.Content = "$ " + efficiencies.AverageAmountCredit.ToString ();
                label_MaximumAmountCredit.Content = "$ " + efficiencies.MaximumAmountCredit.ToString ();
            }
        }
    }
}
