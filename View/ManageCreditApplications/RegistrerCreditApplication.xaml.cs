using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FinanciaRed.View.ManageCreditApplications {
    /// <summary>
    /// Interaction logic for RegistrerCreditApplication.xaml
    /// </summary>
    public partial class RegistrerCreditApplication : Window {
        private DTO_CreditApplication_Create newCreditApplicaton = new DTO_CreditApplication_Create ();
        private bool[] _IsCorrectStage1 = new bool[2] { false, false };
        private bool[] _IsCorrectStage2 = new bool[3] { false, false, false };

        public RegistrerCreditApplication () {
            InitializeComponent ();

            textBox_ClientName.Text = "Erick Utrera Cornejo";
            textBox_CodeCURP.Text = "UTCE020935HVZRDGA1";
            textBox_CodeRFC.Text = "UTCE0209359DJ";

            _ = LoadVariables ();
        }

        private bool ValidateClientForm () {
            bool isFormCorrect = true;

            if (string.IsNullOrEmpty (textBox_ClientName.Text)) {
                label_ErrorClientName.Content = "Campo requerido";
                label_ErrorClientName.Visibility = Visibility.Visible;
                isFormCorrect = false;
            } else {
                label_ErrorClientName.Content = "";
                label_ErrorClientName.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty (textBox_CodeCURP.Text)) {
                label_ErrorCodeCURP.Content = "Campo requerido";
                label_ErrorCodeCURP.Visibility = Visibility.Visible;
                isFormCorrect = false;
            } else {
                label_ErrorCodeCURP.Content = "";
                label_ErrorCodeCURP.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty (textBox_CodeRFC.Text)) {
                label_ErrorCodeRFC.Content = "Campo requerido";
                label_ErrorCodeRFC.Visibility = Visibility.Visible;
                isFormCorrect = false;
            } else {
                label_ErrorCodeRFC.Content = "";
                label_ErrorCodeRFC.Visibility = Visibility.Collapsed;
            }
            return isFormCorrect;
        }

        private async void ClickSearchClient (object sender, RoutedEventArgs e) {
            if (ValidateClientForm ()) {
                MessageResponse<DTO_Client_Search> response =
                    await DAO_Client.GetClientCreditApplicationAsync (
                        textBox_ClientName.Text.Trim (),
                        textBox_CodeCURP.Text.Trim (),
                        textBox_CodeRFC.Text.Trim ()
                    );
                if (!response.IsError) {
                    if (response.DataRetrieved.IdStatus != 1) {
                        label_StatusSearchClient.Content = $"Cliente \"{response.DataRetrieved.Name}\" ubicado. No está activo.";
                    } else {
                        newCreditApplicaton.IdClient = response.DataRetrieved.IdClient;
                        label_StatusSearchClient.Content = $"Cliente \"{response.DataRetrieved.Name}\" ubicado.";
                    }
                } else {
                    label_StatusSearchClient.Content = $"Los datos del cliente no existen en la base de datos.";
                }
            }
        }

        private async Task LoadVariables () {
            MessageResponse<List<DTO_CreditPromotion_Consult>> messageResponseProm =
                await DAO_CreditPromotion.GetAsync (DateTime.Now);

            var promotions = new List<DTO_CreditPromotion_Consult> {
                new DTO_CreditPromotion_Consult { Name = "Seleccione una opción", IdCreditPromotion = 0 }
            };

            if (messageResponseProm.DataRetrieved != null) {
                promotions.AddRange (messageResponseProm.DataRetrieved);
            }

            comboBox_Promotions.ItemsSource = promotions;
            comboBox_Promotions.SelectedIndex = 0;
        }

        private void SelectionChangedPromotion (object sender, SelectionChangedEventArgs e) {
            DTO_CreditPromotion_Consult selectedPromotion = comboBox_Promotions.SelectedItem as DTO_CreditPromotion_Consult;

            if (selectedPromotion != null && selectedPromotion.IdCreditPromotion != 0) {
                newCreditApplicaton.IdPromotion = selectedPromotion.IdCreditPromotion;
                newCreditApplicaton.InterestRate = selectedPromotion.InterestRate;
                newCreditApplicaton.NumberFortNigths = selectedPromotion.NumberFortNigths;
                label_NumberFortNigths.Content = selectedPromotion.NumberFortNigths.ToString () ?? string.Empty;
                label_InterestRate.Content = (selectedPromotion.InterestRate * 100).ToString () ?? string.Empty;
            } else {
                label_NumberFortNigths.Content = "";
                label_InterestRate.Content = "";
            }
        }


        public void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar registro.",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (result == MessageBoxResult.Yes)
                this.Close ();
        }

        private void ClickBackStage1 (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Visible;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
        }

        private bool VerifyStage1 () {
            bool isFormCorrect = true;
            if (string.IsNullOrEmpty (textBox_SolicitedAmount.Text)) {
                label_ErrorSolicitedAmount.Content = "Campo requerido";
                label_ErrorSolicitedAmount.Visibility = Visibility.Visible;
                isFormCorrect = false;
            } else if (int.Parse (textBox_SolicitedAmount.Text) < 5000) {
                label_ErrorSolicitedAmount.Content = "Monto mínimo de $5000";
                label_ErrorSolicitedAmount.Visibility = Visibility.Visible;
                isFormCorrect = false;
            } else {
                newCreditApplicaton.AmountSolicited = int.Parse (textBox_SolicitedAmount.Text);
                label_ErrorSolicitedAmount.Content = "";
                label_ErrorSolicitedAmount.Visibility = Visibility.Collapsed;
            }
            if (newCreditApplicaton.IdPromotion == 0) {
                label_ErrorPromotion.Content = "Se requiere seleccionar una promoción vigente.";
                label_ErrorPromotion.Visibility = Visibility.Visible;
                isFormCorrect = false;
            } else {
                label_ErrorPromotion.Content = "";
                label_ErrorPromotion.Visibility = Visibility.Collapsed;
            }
            if (newCreditApplicaton.IdClient == 0) {
                label_StatusSearchClient.Content = "No se ha seleccionado aún a un cliente.";
                label_StatusSearchClient.Visibility = Visibility.Visible;
                isFormCorrect = false;
            }
            return isFormCorrect;
        }

        public void ClickContinueStage2 (object sender, RoutedEventArgs e) {
            if (VerifyStage1 ()) {
                stackPanel_Stage1.Visibility = Visibility.Collapsed;

                label_SolicitedAmount.Content = "$" + newCreditApplicaton.AmountSolicited;
                label_InteresRate.Content = (newCreditApplicaton.InterestRate * 100) + "%";
                label_TotalAmount.Content = "$" + (newCreditApplicaton.AmountSolicited + (newCreditApplicaton.AmountSolicited * newCreditApplicaton.InterestRate));
                label_TimeLapse.Content = newCreditApplicaton.NumberFortNigths + " quincenas";
                label_RecurringPayment.Content = "$" + (newCreditApplicaton.AmountSolicited + (newCreditApplicaton.AmountSolicited * newCreditApplicaton.InterestRate)) / newCreditApplicaton.NumberFortNigths;
                stackPanel_Stage2.Visibility = Visibility.Visible;
            } else {
                MessageBox.Show (
                    "Faltan datos por ingresar o algunos datos están incorrectos.",
                    "Formulario incompleto.",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async void ClickFinishRegistry (object sender, RoutedEventArgs e) {
            await SaveDataInDatabase ();
        }

        private void UploadProofINE (object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Filter = "Archivos PDF (*.pdf)|*.pdf",
                Title = "Seleccionar el comprobante de INE (PDF)"
            };

            if (openFileDialog.ShowDialog () == true) {
                try {
                    newCreditApplicaton.ProofINE = File.ReadAllBytes (openFileDialog.FileName);
                    label_StatusProofINE.Content = "Subido";
                    label_StatusProofINE.Foreground = new SolidColorBrush (Colors.Black);
                    label_StatusProofINE.Visibility = Visibility.Visible;
                } catch (Exception) {
                    label_StatusProofINE.Content = "Error";
                    label_StatusProofINE.Foreground = new SolidColorBrush (Colors.Red);
                    label_StatusProofINE.Visibility = Visibility.Visible;
                }
            }
        }

        private void UploadProofAddress (object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Filter = "Archivos PDF (*.pdf)|*.pdf",
                Title = "Seleccionar el comprobante de domicilio (PDF)"
            };

            if (openFileDialog.ShowDialog () == true) {
                try {
                    newCreditApplicaton.ProofAddress = File.ReadAllBytes (openFileDialog.FileName);
                    label_StatusProofAddress.Content = "Subido";
                    label_StatusProofAddress.Foreground = new SolidColorBrush (Colors.Black);
                    label_StatusProofAddress.Visibility = Visibility.Visible;
                } catch (Exception) {
                    label_StatusProofAddress.Content = "Error";
                    label_StatusProofAddress.Foreground = new SolidColorBrush (Colors.Red);
                    label_StatusProofAddress.Visibility = Visibility.Visible;
                }
            }
        }

        private void UploadProofLastPayStub (object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Filter = "Archivos PDF (*.pdf)|*.pdf",
                Title = "Seleccionar último talón de pago (PDF)"
            };

            if (openFileDialog.ShowDialog () == true) {
                try {
                    newCreditApplicaton.ProofLastPayStub = File.ReadAllBytes (openFileDialog.FileName);
                    label_StatusProofLastPayStub.Content = "Subido";
                    label_StatusProofLastPayStub.Foreground = new SolidColorBrush (Colors.Black);
                    label_StatusProofLastPayStub.Visibility = Visibility.Visible;
                } catch (Exception) {
                    label_StatusProofLastPayStub.Content = "Error";
                    label_StatusProofLastPayStub.Foreground = new SolidColorBrush (Colors.Red);
                    label_StatusProofLastPayStub.Visibility = Visibility.Visible;
                }
            }
        }

        private bool VerifyStage2 () {
            bool isFormCorrect = true;
            if (newCreditApplicaton.ProofINE.Length <= 0) {
                label_StatusProofINE.Content = "Doc. necesario";
                label_StatusProofINE.Foreground = new SolidColorBrush (Colors.Red);
                label_StatusProofINE.Visibility = Visibility.Visible;
                isFormCorrect = false;
            } else {
                label_StatusProofINE.Content = "Subido";
                label_StatusProofINE.Foreground = new SolidColorBrush (Colors.Black);
                label_StatusProofINE.Visibility = Visibility.Visible;
            }
            if (newCreditApplicaton.ProofAddress.Length <= 0) {
                label_StatusProofAddress.Content = "Doc. necesario";
                label_StatusProofAddress.Foreground = new SolidColorBrush (Colors.Red);
                label_StatusProofAddress.Visibility = Visibility.Visible;
                isFormCorrect = false;
            } else {
                label_StatusProofAddress.Content = "Subido";
                label_StatusProofAddress.Foreground = new SolidColorBrush (Colors.Black);
                label_StatusProofAddress.Visibility = Visibility.Visible;
            }
            if (newCreditApplicaton.ProofLastPayStub.Length <= 0) {
                label_StatusProofLastPayStub.Content = "Doc. necesario";
                label_StatusProofLastPayStub.Foreground = new SolidColorBrush (Colors.Red);
                label_StatusProofLastPayStub.Visibility = Visibility.Visible;
                isFormCorrect = false;
            } else {
                label_StatusProofLastPayStub.Content = "Subido";
                label_StatusProofLastPayStub.Foreground = new SolidColorBrush (Colors.Black);
                label_StatusProofLastPayStub.Visibility = Visibility.Visible;
            }
            return isFormCorrect;
        }

        private async Task SaveDataInDatabase () {
            if (VerifyStage2 ()) {
                newCreditApplicaton.DateApplication = DateTime.Now;
                newCreditApplicaton.IdEmployee = CURRENT_USER.Instance.IdEmployee;
                MessageResponse<bool> messageResponseRegistraCA =
                    await DAO_CreditApplication.PostAsync (newCreditApplicaton);
                if (messageResponseRegistraCA.IsError) {
                    MessageBox.Show (
                        "No se pudo guardar la nueva solicitud de crédito.\nIntente más tarde.", 
                        "Error inesperado.",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                } else {
                    MessageBox.Show (
                        "Se ha registrado correctamente la nueva solicitud de crédito.", 
                        "Registro completo.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close ();
                }
            }
        }

        private void PreviewTextInput_OnlyNumbers (object sender, TextCompositionEventArgs e) {
            if (!char.IsDigit (e.Text, 0)) {
                e.Handled = true;
            }
        }
    }
}
