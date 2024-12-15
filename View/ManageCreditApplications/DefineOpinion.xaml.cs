using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.CreditApplication;
using FinanciaRed.Utils;
using FinanciaRed.View.ManageCreditPolicies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageCreditApplications {
    /// <summary>
    /// Interaction logic for DefineOpinion.xaml
    /// </summary>
    public partial class DefineOpinion : Window {
        private int IdCreditApplication = 0;
        private ObservableCollection<DTO_CreditApplication_CreditPolicies> retrievedCreditPolicies =
            new ObservableCollection<DTO_CreditApplication_CreditPolicies> ();
        private DTO_CreditApplication_Details selectedCreditApplication = null;

        public DefineOpinion (int idCreditApplication) {
            InitializeComponent ();

            IdCreditApplication = idCreditApplication;
            _ = RetrieveCreditApplicationDB ();
            _ = RetrieveCreditPoliciesDB ();
        }

        private async Task RetrieveCreditApplicationDB () {
            MessageResponse<DTO_CreditApplication_Details> messageResponseCreditPolicy =
                await DAO_CreditApplication.GetDetailsCreditApplication (IdCreditApplication);
            selectedCreditApplication = messageResponseCreditPolicy.DataRetrieved;
            ShowDataCreditApplication ();
        }

        private void ShowDataCreditApplication () {
            // Data credit application
            label_NameAdviser.Content = selectedCreditApplication.NameAdviser;
            label_AmountSolicited.Content = selectedCreditApplication.AmountSolicited;
            label_Amountotal.Content = selectedCreditApplication.AmountWithInteres;
            label_InterestRate.Content = selectedCreditApplication.InterestRate;
            label_NumberFortNigths.Content = selectedCreditApplication.NumberFortNights;
            label_DateApplied.Content = selectedCreditApplication.DateSolicited.ToString ("dd/MM/yyyy");
            // Personal data client
            label_ClientFirstName.Content = selectedCreditApplication.ClientFirstName;
            label_ClientMiddleName.Content = selectedCreditApplication.ClientMiddleName;
            label_ClientLastName.Content = selectedCreditApplication.ClientLastName;
            label_DateBirth.Content = selectedCreditApplication.ClientDateBirth.ToString ("dd/MM/yyyy");
            label_ClientGender.Content = selectedCreditApplication.ClientGender;
            label_ClientEmail1.Content = selectedCreditApplication.ClientEmail1;
            label_ClientEmail2.Content = selectedCreditApplication.ClientEmail2;
            label_ClientPhoneNumber1.Content = selectedCreditApplication.ClientPhoneNumber1;
            label_ClientPhoneNumber2.Content = selectedCreditApplication.ClientPhoneNumber2;
            label_ClientAddress.Text = selectedCreditApplication.ClientAddress;
            label_WorkType.Content = selectedCreditApplication.ClientWorkType;
            label_Work.Content = selectedCreditApplication.ClientWork;
            label_MonthlySalary.Content = selectedCreditApplication.ClientMonthlySalary;
            // References client
            label_Reference1FirstName.Content = selectedCreditApplication.ClientReference1.FirstName;
            label_Reference1MiddleName.Content = selectedCreditApplication.ClientReference1.MiddleName;
            label_Reference1LastName.Content = selectedCreditApplication.ClientReference1.LastName;
            label_Reference1Email.Content = selectedCreditApplication.ClientReference1.Email;
            label_Reference1PhoneNumber.Content = selectedCreditApplication.ClientReference1.PhoneNumber;
            label_Reference1Relation.Content = selectedCreditApplication.ClientReference1.RelationshipType;
            label_Reference2FirstName.Content = selectedCreditApplication.ClientReference2.FirstName;
            label_Reference2MiddleName.Content = selectedCreditApplication.ClientReference2.MiddleName;
            label_Reference2LastName.Content = selectedCreditApplication.ClientReference2.LastName;
            label_Reference2Email.Content = selectedCreditApplication.ClientReference2.Email;
            label_Reference2PhoneNumber.Content = selectedCreditApplication.ClientReference2.PhoneNumber;
            label_Reference2Relation.Content = selectedCreditApplication.ClientReference2.RelationshipType;
            // Bank accounts
            label_BankAccount1Bank.Content = selectedCreditApplication.ClientBankAcount1.BankName;
            label_BankAccount1Type.Content = selectedCreditApplication.ClientBankAcount1.CardType;
            label_BankAccount1CLABE.Content = selectedCreditApplication.ClientBankAcount1.CLABE;
            label_BankAccount1CardNumber.Content = selectedCreditApplication.ClientBankAcount1.CardNumber;
            label_BankAccount2Bank.Content = selectedCreditApplication.ClientBankAcount2.BankName;
            label_BankAccount2Type.Content = selectedCreditApplication.ClientBankAcount2.CardType;
            label_BankAccount2CLABE.Content = selectedCreditApplication.ClientBankAcount2.CLABE;
            label_BankAccount2CardNumber.Content = selectedCreditApplication.ClientBankAcount2.CardNumber;

            textBox_Valoration.Text = selectedCreditApplication.Valoration;
        }

        private async Task RetrieveCreditPoliciesDB () {
            MessageResponse<List<DTO_CreditApplication_CreditPolicies>> messageResponseConsultCreditPolicies =
                    await DAO_CreditPolicy.GetCreditPolicies_CreditApplitacion (IdCreditApplication);
            this.retrievedCreditPolicies = new ObservableCollection<DTO_CreditApplication_CreditPolicies> (messageResponseConsultCreditPolicies.DataRetrieved);
            dataGrid_CreditPolicies.ItemsSource = null;
            dataGrid_CreditPolicies.ItemsSource = retrievedCreditPolicies;
        }

        private void ClicShowDetailsCreditPolicy (object sender, RoutedEventArgs e) {
            Button button = sender as Button;

            if (button.DataContext is DTO_CreditApplication_CreditPolicies rowData) {
                ViewDetailsCreditPolicy viewDetailsCreditPolicyWindow =
                    new ViewDetailsCreditPolicy ((dataGrid_CreditPolicies.SelectedItem as DTO_CreditApplication_CreditPolicies).IdCreditPolicy, false);
                viewDetailsCreditPolicyWindow.ShowDialog ();
            }
        }

        private void ClickShowProofINE (object sender, RoutedEventArgs e) {
            ShowPDFWebBrowser.ShowPDF ("INE.pdf", selectedCreditApplication.ProofINE, PdfViewer);
        }

        private void ClickShowProofAddress (object sender, RoutedEventArgs e) {
            ShowPDFWebBrowser.ShowPDF ("ComprobanteDomicilio.pdf", selectedCreditApplication.ProofAddress, PdfViewer);
        }

        private void ClickShowProofLastPayStub (object sender, RoutedEventArgs e) {
            ShowPDFWebBrowser.ShowPDF ("UltimoTalonPago.pdf", selectedCreditApplication.ProofLastVoucher, PdfViewer);
        }

        private void CheckedYesOption (object sender, RoutedEventArgs e) {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton?.DataContext is DTO_CreditApplication_CreditPolicies policy) {
                policy.IsApplied = true;
            }
        }

        private void CheckedNoOption (object sender, RoutedEventArgs e) {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton?.DataContext is DTO_CreditApplication_CreditPolicies policy) {
                policy.IsApplied = false;
            }
        }

        private bool IsAllListChecked () {
            foreach (var policy in retrievedCreditPolicies) {
                if (policy.IsApplied == null) {
                    return false;
                }
            }
            return true;
        }

        private bool CheckOpinionComplete () {
            bool isCorrectOpinion = true;

            if (string.IsNullOrEmpty (textBox_Valoration.Text)) {
                label_ErrorValoration.Content = "Se requiere de una valoración, mínimo 20 caracteres.";
                label_ErrorValoration.Visibility = Visibility.Visible;
                isCorrectOpinion = false;
            } else if (textBox_Valoration.Text.Length < 20) {
                label_ErrorValoration.Content = "Valoración demasiado corta, mínimo 20 caracteres.";
                label_ErrorValoration.Visibility = Visibility.Visible;
                isCorrectOpinion = false;
            } else {
                label_ErrorValoration.Content = "";
                label_ErrorValoration.Visibility = Visibility.Collapsed;
            }
            if (IsAllListChecked () == false) {
                label_ErrorCheckList.Content = "Falta alguna política por confirmar si cumple el cliente.";
                label_ErrorCheckList.Visibility = Visibility.Visible;
                isCorrectOpinion = false;
            } else {
                label_ErrorCheckList.Content = "";
                label_ErrorCheckList.Visibility = Visibility.Collapsed;
            }

            return isCorrectOpinion;
        }

        private async Task SaveOpinionDB () {
            List<string> errors = new List<string> ();

            // Saving checklist policies
            MessageResponse<bool> messageResponseCheckList = await DAO_CreditApplication.SaveCheckListPolicies (
                IdCreditApplication,
                new List<DTO_CreditApplication_CreditPolicies> (retrievedCreditPolicies),
                new List<DTO_CreditApplication_CreditPolicies> (retrievedCreditPolicies).All (a => a.IsApplied ?? false)
            );
            if (!messageResponseCheckList.DataRetrieved) {
                errors.Add ($"Error al guardar la lista de políticas cumplidas.");
            }

            // Saving valoration
            MessageResponse<bool> messageResponseValoration = await DAO_CreditApplication.DefineOpinion (
                IdCreditApplication,
                textBox_Valoration.Text
            );
            if (!messageResponseValoration.DataRetrieved) {
                errors.Add ($"Error al guardar la valoración.");
            }

            // TODO Creating a new Credit object 


            if (errors.Count == 0) {
                MessageBox.Show ("Se ha realizado el dictamen correctamente.\n\nPara consultar el crédito, dirígase a la gestión de créditos.", "Registro exitoso");
                this.Close ();
            } else {
                string errorMessage = string.Join (Environment.NewLine, errors);
                MessageBox.Show ($"Se encontraron los siguientes errores:\n{errorMessage}", "Error inesperado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ClickFinishOpinion (object sender, RoutedEventArgs e) {
            if (CheckOpinionComplete ()) {
                await SaveOpinionDB ();
            }
        }


        private void ClickShowDataApplication (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Visible;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
            stackPanel_Stage5.Visibility = Visibility.Collapsed;
            stackPanel_Stage6.Visibility = Visibility.Collapsed;
        }

        private void ClickShowDataClient (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Visible;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
            stackPanel_Stage5.Visibility = Visibility.Collapsed;
            stackPanel_Stage6.Visibility = Visibility.Collapsed;
        }

        private void ClickShowClientReferences (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Visible;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
            stackPanel_Stage5.Visibility = Visibility.Collapsed;
            stackPanel_Stage6.Visibility = Visibility.Collapsed;
        }

        private void ClickShowBankAccounts (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Visible;
            stackPanel_Stage5.Visibility = Visibility.Collapsed;
            stackPanel_Stage6.Visibility = Visibility.Collapsed;
        }

        private void ClickShowDocs (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
            stackPanel_Stage5.Visibility = Visibility.Visible;
            stackPanel_Stage6.Visibility = Visibility.Collapsed;
        }

        private void ClickShowDefineOpinion (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
            stackPanel_Stage5.Visibility = Visibility.Collapsed;
            stackPanel_Stage6.Visibility = Visibility.Visible;
        }
    }
}
