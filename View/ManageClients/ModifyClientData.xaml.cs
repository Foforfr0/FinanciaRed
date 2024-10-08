using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinanciaRed.View.ManageClients {
    /// <summary>
    /// Interaction logic for ModifyClientData.xaml
    /// </summary>
    public partial class ModifyClientData : Window {
        public DTO_Client_DetailsClient selectedClient = null;
        private bool[] _IsCorrectStage1 = new bool[14] { false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        private bool[] _IsCorrectStage2 = new bool[7] { false, false, false, false, false, false, false };
        private bool[] _IsCorrectStage3 = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };
        private bool[] _IsCorrectStage4 = new bool[9] { false, false, false, false, false, false, false, false, false };

        public ModifyClientData (DTO_Client_DetailsClient selectedClient) {
            InitializeComponent ();

            this.selectedClient = selectedClient;
            _ = LoadVariables ();
        }

        private async Task LoadVariables () {
            comboBox_Gender.Items.Clear ();
            comboBox_Gender.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            comboBox_Gender.Items.Add (new ComboBoxItem { Content = "Masculino" });
            comboBox_Gender.Items.Add (new ComboBoxItem { Content = "Femenino" });

            comboBox_MaritalStatus.Items.Clear ();
            comboBox_MaritalStatus.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_MaritalStatus>> messageResponseMS = await DAO_GeneralVariables.GetAllMaritalStatuses ();
            List<DTO_MaritalStatus> listMS = messageResponseMS.DataRetrieved;
            foreach (DTO_MaritalStatus status in listMS) {
                comboBox_MaritalStatus.Items.Add (new ComboBoxItem { Content = status.Status });
            }

            // Set the DisplayDateStart to 100 years ago
            datePicker_DateBirth.DisplayDateStart = DateTime.Now.AddYears (-100);
            // Set the DisplayDateEnd to the current date
            datePicker_DateBirth.DisplayDateEnd = DateTime.Now.AddYears (-18);


            comboBox_AddressType.Items.Clear ();
            comboBox_AddressType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_AddressType>> messageResponseAT = await DAO_GeneralVariables.GetAllAdressesTypes ();
            List<DTO_AddressType> listAT = messageResponseAT.DataRetrieved;
            foreach (DTO_AddressType type in listAT) {
                comboBox_AddressType.Items.Add (new ComboBoxItem { Content = type.Type });
            }

            comboBox_State.Items.Clear ();
            comboBox_State.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_AddressState>> messageResponseSA = await DAO_GeneralVariables.GetAllAddressStates ();
            List<DTO_AddressState> listSA = messageResponseSA.DataRetrieved;
            foreach (DTO_AddressState state in listSA) {
                comboBox_State.Items.Add (new ComboBoxItem { Content = state.Name });
            }

            comboBox_WorkType.Items.Clear ();
            comboBox_WorkType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_WorkType>> messageResponseWT = await DAO_GeneralVariables.GetAllWorkTypes ();
            List<DTO_WorkType> listWT = messageResponseWT.DataRetrieved;
            foreach (DTO_WorkType type in listWT) {
                comboBox_WorkType.Items.Add (new ComboBoxItem { Content = type.Type });
            }

            comboBox_Reference1RelationshipType.Items.Clear ();
            comboBox_Reference2RelationshipType.Items.Clear ();
            comboBox_Reference1RelationshipType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            comboBox_Reference2RelationshipType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_RelationshipType>> messageResponseRST = await DAO_GeneralVariables.GetAllRelationshipsTypes ();
            List<DTO_RelationshipType> listRST = messageResponseRST.DataRetrieved;
            foreach (DTO_RelationshipType type in listRST) {
                comboBox_Reference1RelationshipType.Items.Add (new ComboBoxItem { Content = type.Type });
                comboBox_Reference2RelationshipType.Items.Add (new ComboBoxItem { Content = type.Type });
            }

            comboBox_BankAccount1Name.Items.Clear ();
            comboBox_BankAccount2Name.Items.Clear ();
            comboBox_BankAccount1Name.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            comboBox_BankAccount2Name.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_Bank>> messageResponseBank = await DAO_GeneralVariables.GetAllBanks ();
            List<DTO_Bank> listBank = messageResponseBank.DataRetrieved;
            foreach (DTO_Bank type in listBank) {
                comboBox_BankAccount1Name.Items.Add (new ComboBoxItem { Content = type.Name });
                comboBox_BankAccount2Name.Items.Add (new ComboBoxItem { Content = type.Name });
            }

            comboBox_BankAccount1CardType.Items.Clear ();
            comboBox_BankAccount2CardType.Items.Clear ();
            comboBox_BankAccount1CardType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            comboBox_BankAccount2CardType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_CardType>> messageResponseTypesCard = await DAO_GeneralVariables.GetAllCardTypes ();
            List<DTO_CardType> listCardTypes = messageResponseTypesCard.DataRetrieved;
            foreach (DTO_CardType type in listCardTypes) {
                comboBox_BankAccount1CardType.Items.Add (new ComboBoxItem { Content = type.Type });
                comboBox_BankAccount2CardType.Items.Add (new ComboBoxItem { Content = type.Type });
            }
            await Task.Delay (500);
            ShowDataClient ();
        }

        private void ShowDataClient () {
            //-----------------------------------------------Personal data
            textBox_Name.Text = selectedClient.FirstName;
            textBox_MiddleName.Text = selectedClient.MiddleName;
            textBox_LastName.Text = selectedClient.LastName;
            datePicker_DateBirth.SelectedDate = selectedClient.DateBirth;
            comboBox_Gender.SelectedIndex = selectedClient.Gender.Equals ("M") ? 1 : 2;
            comboBox_MaritalStatus.SelectedIndex = selectedClient.IdMaritalStatus;
            textBox_CodeCurp.Text = selectedClient.CodeCurp;
            //-----------------------------------------------Address data
            comboBox_State.SelectedIndex = selectedClient.AddressClient.IdState;
            textBox_Municipality.Text = selectedClient.AddressClient.Municipality;
            textBox_PostalCode.Text = selectedClient.AddressClient.PostalCode;
            textBox_Colony.Text = selectedClient.AddressClient.Colony;
            textBox_Street.Text = selectedClient.AddressClient.Street;
            textBox_ExteriorNumber.Text = selectedClient.AddressClient.ExteriorNumber;
            textBox_InteriorNumber.Text = selectedClient.AddressClient.InteriorNumber;
            comboBox_AddressType.SelectedIndex = selectedClient.AddressClient.IdAddressType;
            //-----------------------------------------------Contact data
            textBox_Email1.Text = selectedClient.Email1;
            textBox_Email2.Text = selectedClient.Email2;
            textBox_PhoneNumber1.Text = selectedClient.PhoneNumber1;
            textBox_PhoneNumber2.Text = selectedClient.PhoneNumber2;
            //-----------------------------------------------Work data
            comboBox_WorkType.SelectedIndex = selectedClient.IdWorkType;
            textBox_WorkArea.Text = selectedClient.WorkArea;
            textBox_MonthlySalary.Text = string.Concat (selectedClient.MonthlySalary);
            //-----------------------------------------------Reference contact 1 data
            textBox_Reference1FirstName.Text = selectedClient.Reference2FirstName;
            textBox_Reference1MiddleName.Text = selectedClient.Reference2MiddleName;
            textBox_Reference1LastName.Text = selectedClient.Reference2LastName;
            textBox_Reference1Email.Text = selectedClient.Reference1Email;
            textBox_Reference1PhoneNumber.Text = selectedClient.Reference1PhoneNumber;
            comboBox_Reference1RelationshipType.SelectedIndex = selectedClient.IdReference1RelationshipType;
            //-----------------------------------------------Reference contact 2 data
            textBox_Reference2FirstName.Text = selectedClient.Reference2FirstName;
            textBox_Reference2MiddleName.Text = selectedClient.Reference2MiddleName;
            textBox_Reference2LastName.Text = selectedClient.Reference2LastName;
            textBox_Reference2Email.Text = selectedClient.Reference2Email;
            textBox_Reference2PhoneNumber.Text = selectedClient.Reference2PhoneNumber;
            comboBox_Reference2RelationshipType.SelectedIndex = selectedClient.IdReference2RelationshipType;
            //-----------------------------------------------Financial data
            textBox_CodeRFC.Text = selectedClient.CodeRFC;
            //-----------------------------------------------Bank Account 1 data
            comboBox_BankAccount1Name.SelectedIndex = selectedClient.IdBankAccount1Name;
            textBox_BankAccount1CodeCLABE.Text = selectedClient.BankAccount1CLABE;
            textBox_BankAccount1CardNumber.Text = selectedClient.BankAccount1CardNumber;
            comboBox_BankAccount1CardType.SelectedIndex = selectedClient.IdBankAccount1CardType;
            //-----------------------------------------------Bank Account 2 data
            comboBox_BankAccount2Name.SelectedIndex = selectedClient.IdBankAccount2Name;
            textBox_BankAccount2CodeCLABE.Text = selectedClient.BankAccount2CLABE;
            textBox_BankAccount2CardNumber.Text = selectedClient.BankAccount2CardNumber;
            comboBox_BankAccount2CardType.SelectedIndex = selectedClient.IdBankAccount2CardType;
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar registro.",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes)
                this.Close ();
        }

        private void ClickBackStage1 (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Visible;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
        }

        private void ClickBackStage2 (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Visible;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
        }

        private async void ClickContinueStage2 (object sender, RoutedEventArgs e) {
            VerifyStage1 ();
            bool existsCURP = await DAO_Client.VerifyExistenceCURP (textBox_CodeCurp.Text);
            await Task.Delay (500);

            if (_IsCorrectStage1.All (x => x == true)) {
                if (textBox_CodeCurp.Text.Equals (selectedClient.CodeCurp)) {
                    stackPanel_Stage1.Visibility = Visibility.Collapsed;
                    stackPanel_Stage2.Visibility = Visibility.Visible;
                    stackPanel_Stage3.Visibility = Visibility.Collapsed;
                    stackPanel_Stage4.Visibility = Visibility.Collapsed;
                } else {
                    if (existsCURP) {
                        label_ErrorCodeCurp.Content = "CURP ya existente en la base de datos.";
                        label_ErrorCodeCurp.Visibility = Visibility.Visible;
                        _IsCorrectStage1[6] = false;
                    } else {
                        label_ErrorCodeCurp.Content = "";
                        label_ErrorCodeCurp.Visibility = Visibility.Collapsed;
                        _IsCorrectStage1[6] = true;
                        stackPanel_Stage1.Visibility = Visibility.Collapsed;
                        stackPanel_Stage2.Visibility = Visibility.Visible;
                        stackPanel_Stage3.Visibility = Visibility.Collapsed;
                        stackPanel_Stage4.Visibility = Visibility.Collapsed;
                    }
                }
            } else {
                MessageBox.Show ("Faltan datos por ingresar o algunos datos están incorrectos.", "Formulario incompleto.");
            }
        }

        private void ClickBackStage3 (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Visible;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
        }

        private async void ClickContinueStage3 (object sender, RoutedEventArgs e) {
            VerifyStage2 ();

            if (_IsCorrectStage2.All (x => x == true)) {
                bool existsEmail1;
                if (textBox_Email1.Text.Equals (selectedClient.Email1)) {
                    existsEmail1 = false;
                } else {
                    existsEmail1 = await DAO_Client.VerifyExistenceEmail (textBox_Email1.Text);
                }
                bool existsEmail2 = false;
                if (!string.IsNullOrEmpty (textBox_Email2.Text)) {
                    existsEmail2 = await DAO_Client.VerifyExistenceEmail (textBox_Email2.Text);
                }
                if (textBox_Email2.Text.Equals (selectedClient.Email2)) {
                    existsEmail2 = false;
                }
                bool existsPhoneNumber1;
                if (textBox_PhoneNumber1.Text.Equals (selectedClient.PhoneNumber1)) {
                    existsPhoneNumber1 = false;
                } else {
                    existsPhoneNumber1 = await DAO_Client.VerifyExistencePhoneNumber (textBox_PhoneNumber1.Text);
                }
                bool existsPhoneNumber2 = false;
                if (!string.IsNullOrEmpty (textBox_PhoneNumber2.Text)) {
                    existsPhoneNumber2 = await DAO_Client.VerifyExistencePhoneNumber (textBox_PhoneNumber2.Text);
                }
                if (textBox_PhoneNumber2.Text.Equals (selectedClient.PhoneNumber2)) {
                    existsPhoneNumber2 = false;
                }
                if (existsEmail1) {
                    label_ErrorEmail1.Content = "Correo existente en la base de datos.";
                    label_ErrorEmail1.Visibility = Visibility.Visible;
                }
                if (existsEmail2) {
                    label_ErrorEmail2.Content = "Correo existente en la base de datos.";
                    label_ErrorEmail2.Visibility = Visibility.Visible;
                }
                if (existsPhoneNumber1) {
                    label_ErrorPhoneNumber1.Content = "Número existente en la base de datos.";
                    label_ErrorPhoneNumber1.Visibility = Visibility.Visible;
                }
                if (existsPhoneNumber2) {
                    label_ErrorPhoneNumber2.Content = "Número existente en la base de datos.";
                    label_ErrorPhoneNumber2.Visibility = Visibility.Visible;
                }
                if (!existsEmail1 && !existsEmail2 && !existsPhoneNumber1 && !existsPhoneNumber2) {
                    stackPanel_Stage1.Visibility = Visibility.Collapsed;
                    stackPanel_Stage2.Visibility = Visibility.Collapsed;
                    stackPanel_Stage3.Visibility = Visibility.Visible;
                    stackPanel_Stage4.Visibility = Visibility.Collapsed;
                }
            } else {
                MessageBox.Show ("Faltan datos por ingresar o algunos datos están incorrectos.", "Formulario incompleto.");
            }
        }

        private void ClickBackStage4 (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Visible;
        }

        private void ClickContinueStage4 (object sender, RoutedEventArgs e) {
            VerifyStage3 ();

            if (_IsCorrectStage3.All (x => x == true)) {
                stackPanel_Stage1.Visibility = Visibility.Collapsed;
                stackPanel_Stage2.Visibility = Visibility.Collapsed;
                stackPanel_Stage3.Visibility = Visibility.Collapsed;
                stackPanel_Stage4.Visibility = Visibility.Visible;
            } else {
                MessageBox.Show ("Faltan datos por ingresar o algunos datos están incorrectos.", "Formulario incompleto.");
            }
        }

        private void PreviewTextInput_OnlyNumbers (object sender, TextCompositionEventArgs e) {
            if (!char.IsDigit (e.Text, 0)) {
                e.Handled = true;
            }
        }

        private async void ClickFinishModification (object sender, RoutedEventArgs e) {
            VerifyStage4 ();
            bool existsRFC = await DAO_Client.VerifyExistenceRFC (textBox_CodeRFC.Text);
            await Task.Delay (500);

            if (_IsCorrectStage4.All (x => x == true)) {
                if (textBox_CodeCurp.Text.Equals (selectedClient.CodeCurp)) {
                    await SaveDataInDatabase ();
                } else {
                    if (existsRFC) {
                        label_ErrorCodeRFC.Content = "RFC ya existente en la base de datos.";
                        label_ErrorCodeRFC.Visibility = Visibility.Visible;
                        _IsCorrectStage4[6] = false;
                    } else {
                        await SaveDataInDatabase ();
                    }
                }
            } else {
                MessageBox.Show ("Faltan datos por ingresar o algunos datos están incorrectos.", "Formulario incompleto.");
            }
        }

        private async Task SaveDataInDatabase () {
            DTO_Client_DetailsClient newClient = new DTO_Client_DetailsClient {
                IdClient = selectedClient.IdClient,
                FirstName = textBox_Name.Text,
                MiddleName = textBox_MiddleName.Text,
                LastName = textBox_LastName.Text,
                DateBirth = DateTime.Parse (datePicker_DateBirth.Text),
                Gender = comboBox_Gender.SelectedIndex == 1 ? "M" : "F",
                IdMaritalStatus = comboBox_MaritalStatus.SelectedIndex,
                CodeCurp = textBox_CodeCurp.Text,
                AddressClient = new DTO_AddressClient {
                    IdState = comboBox_State.SelectedIndex,
                    Municipality = textBox_Municipality.Text,
                    PostalCode = textBox_PostalCode.Text,
                    Colony = textBox_Colony.Text,
                    Street = textBox_Street.Text,
                    ExteriorNumber = textBox_ExteriorNumber.Text,
                    InteriorNumber = textBox_InteriorNumber.Text,
                    IdAddressType = comboBox_AddressType.SelectedIndex
                },
                Email1 = textBox_Email1.Text,
                Email2 = textBox_Email2.Text,
                PhoneNumber1 = textBox_PhoneNumber1.Text,
                PhoneNumber2 = textBox_PhoneNumber2.Text,
                IdWorkType = comboBox_WorkType.SelectedIndex,
                WorkArea = textBox_WorkArea.Text,
                MonthlySalary = float.Parse (textBox_MonthlySalary.Text),
                Reference1FirstName = textBox_Reference1FirstName.Text,
                Reference1MiddleName = textBox_Reference1MiddleName.Text,
                Reference1LastName = textBox_Reference1LastName.Text,
                Reference1Email = textBox_Reference1Email.Text,
                Reference1PhoneNumber = textBox_Reference1PhoneNumber.Text,
                IdReference1RelationshipType = comboBox_Reference1RelationshipType.SelectedIndex,
                Reference2FirstName = textBox_Reference2FirstName.Text,
                Reference2MiddleName = textBox_Reference2MiddleName.Text,
                Reference2LastName = textBox_Reference2LastName.Text,
                Reference2Email = textBox_Reference2Email.Text,
                Reference2PhoneNumber = textBox_Reference2PhoneNumber.Text,
                IdReference2RelationshipType = comboBox_Reference2RelationshipType.SelectedIndex,
                CodeRFC = textBox_CodeRFC.Text,
                IdBankAccount1Name = comboBox_BankAccount1Name.SelectedIndex,
                BankAccount1CLABE = textBox_BankAccount1CodeCLABE.Text,
                BankAccount1CardNumber = textBox_BankAccount1CardNumber.Text,
                IdBankAccount1CardType = comboBox_BankAccount1CardType.SelectedIndex,
                StatusActive = this.selectedClient.StatusActive
            };
            if ((bool)checkBox_SameAccount.IsChecked) {
                newClient.IdBankAccount2Name = newClient.IdBankAccount1Name;
                newClient.BankAccount2CLABE = newClient.BankAccount1CLABE;
                newClient.BankAccount2CardNumber = newClient.BankAccount1CardNumber;
                newClient.IdBankAccount2CardType = newClient.IdBankAccount1CardType;
            } else {
                newClient.IdBankAccount2Name = comboBox_BankAccount2Name.SelectedIndex;
                newClient.BankAccount2CLABE = textBox_BankAccount2CodeCLABE.Text;
                newClient.BankAccount2CardNumber = textBox_BankAccount2CardNumber.Text;
                newClient.IdBankAccount2CardType = comboBox_BankAccount2CardType.SelectedIndex;
            }

            MessageResponse<bool> responseModifyClient = DAO_Client.SaveChangesDataClient (newClient);
            if (responseModifyClient.IsError) {
                MessageBox.Show ("Ha ocurrido un error inesperado.\nIntente más tarde.", "Error inesperado.");
            } else {
                MessageBox.Show ($"Se ha modificado correctamente al\nCLIENTE: \"{newClient.FirstName} {newClient.MiddleName} {newClient.LastName}\"", "Modificación completa.");
                this.Close ();
            }
        }

        //Stage 1 validations---------------------------------------------------------------------------
        private void TextChanged_Name (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_Name.Text, true, false)) {
                label_ErrorName.Content = "Nombre no válido.";
                label_ErrorName.Visibility = Visibility.Visible;
                _IsCorrectStage1[0] = false;
            } else {
                label_ErrorName.Content = "";
                label_ErrorName.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[0] = true;
            }
        }

        private void TextChanged_MiddleName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_MiddleName.Text, false, false)) {
                label_ErrorMiddleName.Content = "Apellido paterno no válido.";
                label_ErrorMiddleName.Visibility = Visibility.Visible;
                _IsCorrectStage1[1] = false;
            } else {
                label_ErrorMiddleName.Content = "";
                label_ErrorMiddleName.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[1] = true;
            }
        }

        private void TextChanged_LastName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_LastName.Text, false, false)) {
                label_ErrorLastName.Content = "Apellido materno no válido.";
                label_ErrorLastName.Visibility = Visibility.Visible;
                _IsCorrectStage1[2] = false;
            } else {
                label_ErrorLastName.Content = "";
                label_ErrorLastName.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[2] = true;
            }
        }

        private void TextChanged_CodeCurp (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidCURP (textBox_CodeCurp.Text)) {
                label_ErrorCodeCurp.Content = "CURP no válido.";
                label_ErrorCodeCurp.Visibility = Visibility.Visible;
                _IsCorrectStage1[6] = false;
            } else {
                label_ErrorCodeCurp.Content = "";
                label_ErrorCodeCurp.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[6] = true;
            }
        }

        private void TextChanged_Municipality (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_Municipality.Text, true, false)) {
                label_ErrorMunicipality.Content = "Nombre de municipio no válido.";
                label_ErrorMunicipality.Visibility = Visibility.Visible;
                _IsCorrectStage1[8] = false;
            } else {
                label_ErrorMunicipality.Content = "";
                label_ErrorMunicipality.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[8] = true;
            }
        }

        private void TextChanged_PostalCode (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidPostalCode (textBox_PostalCode.Text)) {
                label_ErrorPostalCode.Content = "Código postal no válido.";
                label_ErrorPostalCode.Visibility = Visibility.Visible;
                _IsCorrectStage1[9] = false;
            } else {
                label_ErrorPostalCode.Content = "";
                label_ErrorPostalCode.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[9] = true;
            }
        }

        private void TextChanged_Colony (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_Colony.Text, true, false)) {
                label_ErrorColony.Content = "Nombre de colonia no válido.";
                label_ErrorColony.Visibility = Visibility.Visible;
                _IsCorrectStage1[10] = false;
            } else {
                label_ErrorColony.Content = "";
                label_ErrorColony.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[10] = true;
            }
        }

        private void TextChanged_Street (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_Street.Text, true, true)) {
                label_ErrorStreet.Content = "Nombre de calle no válido.";
                label_ErrorStreet.Visibility = Visibility.Visible;
                _IsCorrectStage1[11] = false;
            } else {
                label_ErrorStreet.Content = "";
                label_ErrorStreet.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[11] = true;
            }
        }

        private void VerifyStage1 () {
            if (string.IsNullOrEmpty (textBox_Name.Text)) {
                label_ErrorName.Content = "Campo necesario.";
                label_ErrorName.Visibility = Visibility.Visible;
                _IsCorrectStage1[0] = false;
            }

            if (string.IsNullOrEmpty (textBox_MiddleName.Text)) {
                label_ErrorMiddleName.Content = "Campo necesario.";
                label_ErrorMiddleName.Visibility = Visibility.Visible;
                _IsCorrectStage1[1] = false;
            }

            if (string.IsNullOrEmpty (textBox_LastName.Text)) {
                label_ErrorLastName.Content = "Campo necesario.";
                label_ErrorLastName.Visibility = Visibility.Visible;
                _IsCorrectStage1[2] = false;
            }

            if (datePicker_DateBirth.SelectedDate == null) {
                label_ErrorDateBirth.Content = "Campo necesario.";
                label_ErrorDateBirth.Visibility = Visibility.Visible;
                _IsCorrectStage1[3] = false;
            } else {
                label_ErrorDateBirth.Content = "";
                label_ErrorDateBirth.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[3] = true;
            }

            if (comboBox_Gender.SelectedIndex == 0) {
                label_ErrorGender.Content = "Campo necesario.";
                label_ErrorGender.Visibility = Visibility.Visible;
                _IsCorrectStage1[4] = false;
            } else {
                label_ErrorGender.Content = "";
                label_ErrorGender.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[4] = true;
            }

            if (comboBox_MaritalStatus.SelectedIndex == 0) {
                label_ErrorMaritalStatus.Content = "Campo necesario.";
                label_ErrorMaritalStatus.Visibility = Visibility.Visible;
                _IsCorrectStage1[5] = false;
            } else {
                label_ErrorMaritalStatus.Content = "";
                label_ErrorMaritalStatus.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[5] = true;
            }

            if (string.IsNullOrEmpty (textBox_CodeCurp.Text)) {
                label_ErrorCodeCurp.Content = "Campo necesario.";
                label_ErrorCodeCurp.Visibility = Visibility.Visible;
                _IsCorrectStage1[6] = false;
            }

            if (comboBox_State.SelectedIndex == 0) {
                label_ErrorState.Content = "Campo necesario.";
                label_ErrorState.Visibility = Visibility.Visible;
                _IsCorrectStage1[7] = false;
            } else {
                label_ErrorState.Content = "";
                label_ErrorState.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[7] = true;
            }

            if (string.IsNullOrEmpty (textBox_Municipality.Text)) {
                label_ErrorMunicipality.Content = "Campo necesario.";
                label_ErrorMunicipality.Visibility = Visibility.Visible;
                _IsCorrectStage1[8] = false;
            }

            if (string.IsNullOrEmpty (textBox_PostalCode.Text)) {
                label_ErrorPostalCode.Content = "Campo necesario.";
                label_ErrorPostalCode.Visibility = Visibility.Visible;
                _IsCorrectStage1[9] = false;
            }

            if (string.IsNullOrEmpty (textBox_Colony.Text)) {
                label_ErrorColony.Content = "Campo necesario.";
                label_ErrorColony.Visibility = Visibility.Visible;
                _IsCorrectStage1[10] = false;
            }

            if (string.IsNullOrEmpty (textBox_Street.Text)) {
                label_ErrorStreet.Content = "Campo necesario.";
                label_ErrorStreet.Visibility = Visibility.Visible;
                _IsCorrectStage1[11] = false;
            }

            if (string.IsNullOrEmpty (textBox_ExteriorNumber.Text)) {
                label_ErrorExteriorNumber.Content = "Campo necesario.";
                label_ErrorExteriorNumber.Visibility = Visibility.Visible;
                _IsCorrectStage1[12] = false;
            } else {
                label_ErrorExteriorNumber.Content = "";
                label_ErrorExteriorNumber.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[12] = true;
            }

            if (comboBox_AddressType.SelectedIndex == 0) {
                label_ErrorAddressType.Content = "Campo necesario.";
                label_ErrorAddressType.Visibility = Visibility.Visible;
                _IsCorrectStage1[13] = false;
            } else {
                label_ErrorAddressType.Content = "";
                label_ErrorAddressType.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[13] = true;
            }
        }

        //Stage 2 validations---------------------------------------------------------------------------
        private void TextChanged_Email1 (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidEmail (textBox_Email1.Text)) {
                label_ErrorEmail1.Content = "Correo electrónico no válido.";
                label_ErrorEmail1.Visibility = Visibility.Visible;
                _IsCorrectStage2[0] = false;
            } else {
                label_ErrorEmail1.Content = "";
                label_ErrorEmail1.Visibility = Visibility.Collapsed;
                _IsCorrectStage2[0] = true;
            }
        }

        private void TextChanged_Email2 (object sender, TextChangedEventArgs e) {
            if (string.IsNullOrEmpty (textBox_Email2.Text)) {
                label_ErrorEmail2.Content = "";
                label_ErrorEmail2.Visibility = Visibility.Collapsed;
                _IsCorrectStage2[1] = true;
            } else {
                if (CheckFormat.IsValidEmail (textBox_Email2.Text)) {
                    label_ErrorEmail2.Content = "";
                    label_ErrorEmail2.Visibility = Visibility.Collapsed;
                    _IsCorrectStage2[1] = true;
                } else {
                    label_ErrorEmail2.Content = "Correo electrónico no válido.";
                    label_ErrorEmail2.Visibility = Visibility.Visible;
                    _IsCorrectStage2[1] = false;
                }
            }
        }

        private void TextChanged_PhoneNumber1 (object sender, TextChangedEventArgs e) {
            textBox_PhoneNumber1.Text = CheckFormat.FormatPhoneNumber (
                                            textBox_PhoneNumber1.Text.Replace ("-", "")
                                        );
            textBox_PhoneNumber1.CaretIndex = textBox_PhoneNumber1.Text.Length;

            if (!CheckFormat.IsValidPhoneNumber (textBox_PhoneNumber1.Text)) {
                label_ErrorPhoneNumber1.Content = "Número de teléfono no válido.";
                label_ErrorPhoneNumber1.Visibility = Visibility.Visible;
                _IsCorrectStage2[2] = false;
            } else {
                label_ErrorPhoneNumber1.Content = "";
                label_ErrorPhoneNumber1.Visibility = Visibility.Collapsed;
                _IsCorrectStage2[2] = true;
            }
        }

        private void TextChanged_PhoneNumber2 (object sender, TextChangedEventArgs e) {
            textBox_PhoneNumber2.Text = CheckFormat.FormatPhoneNumber (
                                            textBox_PhoneNumber2.Text.Replace ("-", "")
                                        );
            textBox_PhoneNumber2.CaretIndex = textBox_PhoneNumber2.Text.Length;

            if (!string.IsNullOrEmpty (textBox_PhoneNumber2.Text)) {
                if (!CheckFormat.IsValidPhoneNumber (textBox_PhoneNumber2.Text)) {
                    label_ErrorPhoneNumber2.Content = "Número de teléfono no válido.";
                    label_ErrorPhoneNumber2.Visibility = Visibility.Visible;
                    _IsCorrectStage2[3] = false;
                } else {
                    label_ErrorPhoneNumber2.Content = "";
                    label_ErrorPhoneNumber2.Visibility = Visibility.Collapsed;
                    _IsCorrectStage2[3] = true;
                }
            } else {
                label_ErrorPhoneNumber2.Content = "";
                label_ErrorPhoneNumber2.Visibility = Visibility.Collapsed;
                _IsCorrectStage2[3] = true;
            }
        }

        private void TextChanged_WorkArea (object sender, TextChangedEventArgs e) {
            if (!string.IsNullOrEmpty (textBox_WorkArea.Text)) {
                label_ErrorWorkArea.Content = "";
                label_ErrorWorkArea.Visibility = Visibility.Visible;
                _IsCorrectStage2[5] = true;
            }
        }

        private void TextChanged_MonthlySalary (object sender, TextChangedEventArgs e) {
            if (string.IsNullOrEmpty (textBox_MonthlySalary.Text)) {
                label_ErrorMonthly.Content = "Campo necesario";
                label_ErrorMonthly.Visibility = Visibility.Visible;
                _IsCorrectStage2[6] = false;
            } else {
                label_ErrorMonthly.Content = "";
                label_ErrorMonthly.Visibility = Visibility.Collapsed;
                _IsCorrectStage2[6] = true;
            }
        }

        private void VerifyStage2 () {
            if (string.IsNullOrEmpty (textBox_Email1.Text)) {
                label_ErrorEmail1.Content = "Campo necesario.";
                label_ErrorEmail1.Visibility = Visibility.Visible;
                _IsCorrectStage2[0] = false;
            }

            if (textBox_Email1.Text.Equals (textBox_Email2.Text)) {
                label_ErrorEmail2.Content = "Correo electrónico igual al anterior.";
                label_ErrorEmail2.Visibility = Visibility.Visible;
                _IsCorrectStage2[1] = false;
            }
            if (string.IsNullOrEmpty (textBox_Email2.Text)) {
                label_ErrorEmail2.Content = "";
                label_ErrorEmail2.Visibility = Visibility.Collapsed;
                _IsCorrectStage2[1] = true;
            }

            if (string.IsNullOrEmpty (textBox_PhoneNumber1.Text)) {
                label_ErrorPhoneNumber1.Content = "Campo necesario.";
                label_ErrorPhoneNumber1.Visibility = Visibility.Visible;
                _IsCorrectStage2[2] = false;
            }

            if (textBox_PhoneNumber1.Text.Equals (textBox_PhoneNumber2.Text)) {
                label_ErrorPhoneNumber2.Content = "Número igual al anterior.";
                label_ErrorPhoneNumber2.Visibility = Visibility.Visible;
                _IsCorrectStage2[3] = false;
            }
            if (string.IsNullOrEmpty (textBox_PhoneNumber2.Text)) {
                label_ErrorPhoneNumber2.Content = "";
                label_ErrorPhoneNumber2.Visibility = Visibility.Collapsed;
                _IsCorrectStage2[3] = true;
            }

            if (comboBox_WorkType.SelectedIndex == 0) {
                label_ErrorWorkType.Content = "Campo necesario.";
                label_ErrorWorkType.Visibility = Visibility.Visible;
                _IsCorrectStage2[4] = false;
            } else {
                label_ErrorWorkType.Content = "";
                label_ErrorWorkType.Visibility = Visibility.Collapsed;
                _IsCorrectStage2[4] = true;
            }

            if (string.IsNullOrEmpty (textBox_WorkArea.Text)) {
                label_ErrorWorkArea.Content = "Campo necesario.";
                label_ErrorWorkArea.Visibility = Visibility.Visible;
                _IsCorrectStage2[5] = false;
            }

            if (string.IsNullOrEmpty (textBox_MonthlySalary.Text)) {
                label_ErrorMonthly.Content = "Campo necesario.";
                label_ErrorMonthly.Visibility = Visibility.Visible;
                _IsCorrectStage2[6] = false;
            }
        }

        //Stage 3 validations---------------------------------------------------------------------------
        private void TextChanged_Reference1FirstName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_Reference1FirstName.Text, true, false)) {
                label_ErrorReference1FirstName.Content = "Nombre no válido.";
                label_ErrorReference1FirstName.Visibility = Visibility.Visible;
                _IsCorrectStage3[0] = false;
            } else {
                label_ErrorReference1FirstName.Content = "";
                label_ErrorReference1FirstName.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[0] = true;
            }
        }

        private void TextChanged_Reference1MiddleName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_Reference1MiddleName.Text, true, false)) {
                label_ErrorReference1MiddleName.Content = "Apellido paterno no válido.";
                label_ErrorReference1MiddleName.Visibility = Visibility.Visible;
                _IsCorrectStage3[1] = false;
            } else {
                label_ErrorReference1MiddleName.Content = "";
                label_ErrorReference1MiddleName.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[1] = true;
            }
        }

        private void TextChanged_Reference1LastName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_Reference1LastName.Text, true, false)) {
                label_ErrorReference1LastName.Content = "Apellido materno no válido.";
                label_ErrorReference1LastName.Visibility = Visibility.Visible;
                _IsCorrectStage3[2] = false;
            } else {
                label_ErrorReference1LastName.Content = "";
                label_ErrorReference1LastName.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[2] = true;
            }
        }

        private void TextChanged_Reference1Email (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidEmail (textBox_Reference1Email.Text)) {
                label_ErrorReference1Email.Content = "Correo electrónico no válido.";
                label_ErrorReference1Email.Visibility = Visibility.Visible;
                _IsCorrectStage3[3] = false;
            } else {
                label_ErrorReference1Email.Content = "";
                label_ErrorReference1Email.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[3] = true;
            }
        }

        private void TextChanged_Reference1PhoneNumber (object sender, TextChangedEventArgs e) {
            textBox_Reference1PhoneNumber.Text = CheckFormat.FormatPhoneNumber (
                                            textBox_Reference1PhoneNumber.Text.Replace ("-", "")
                                        );
            textBox_Reference1PhoneNumber.CaretIndex = textBox_Reference1PhoneNumber.Text.Length;

            if (!CheckFormat.IsValidPhoneNumber (textBox_Reference1PhoneNumber.Text)) {
                label_ErrorReference1PhoneNumber.Content = "Número de teléfono no válido.";
                label_ErrorReference1PhoneNumber.Visibility = Visibility.Visible;
                _IsCorrectStage3[4] = false;
            } else {
                label_ErrorReference1PhoneNumber.Content = "";
                label_ErrorReference1PhoneNumber.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[4] = true;
            }
        }

        private void TextChanged_Reference2FirstName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_Reference2FirstName.Text, true, false)) {
                label_ErrorReference2FirstName.Content = "Nombre no válido.";
                label_ErrorReference2FirstName.Visibility = Visibility.Visible;
                _IsCorrectStage3[6] = false;
            } else {
                label_ErrorReference2FirstName.Content = "";
                label_ErrorReference2FirstName.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[6] = true;
            }
        }

        private void TextChanged_Reference2MiddleName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_Reference2MiddleName.Text, true, false)) {
                label_ErrorReference2MiddleName.Content = "Apellido paterno no válido.";
                label_ErrorReference2MiddleName.Visibility = Visibility.Visible;
                _IsCorrectStage3[7] = false;
            } else {
                label_ErrorReference2MiddleName.Content = "";
                label_ErrorReference2MiddleName.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[7] = true;
            }
        }

        private void TextChanged_Reference2LastName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_Reference2LastName.Text, true, false)) {
                label_ErrorReference2LastName.Content = "Apellido materno no válido.";
                label_ErrorReference2LastName.Visibility = Visibility.Visible;
                _IsCorrectStage3[8] = false;
            } else {
                label_ErrorReference2LastName.Content = "";
                label_ErrorReference2LastName.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[8] = true;
            }
        }

        private void TextChanged_Reference2Email (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidEmail (textBox_Reference2Email.Text)) {
                label_ErrorReference2Email.Content = "Correo electrónico no válido.";
                label_ErrorReference2Email.Visibility = Visibility.Visible;
                _IsCorrectStage3[9] = false;
            } else {
                label_ErrorReference2Email.Content = "";
                label_ErrorReference2Email.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[9] = true;
            }
        }

        private void TextChanged_Reference2PhoneNumber (object sender, TextChangedEventArgs e) {
            textBox_Reference2PhoneNumber.Text = CheckFormat.FormatPhoneNumber (
                                            textBox_Reference2PhoneNumber.Text.Replace ("-", "")
                                        );
            textBox_Reference2PhoneNumber.CaretIndex = textBox_Reference2PhoneNumber.Text.Length;

            if (!CheckFormat.IsValidPhoneNumber (textBox_Reference2PhoneNumber.Text)) {
                label_ErrorReference2PhoneNumber.Content = "Número de teléfono no válido.";
                label_ErrorReference2PhoneNumber.Visibility = Visibility.Visible;
                _IsCorrectStage3[10] = false;
            } else {
                label_ErrorReference2PhoneNumber.Content = "";
                label_ErrorReference2PhoneNumber.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[10] = true;
            }
        }

        private void VerifyStage3 () {
            if (string.IsNullOrEmpty (textBox_Reference1FirstName.Text)) {
                label_ErrorReference1FirstName.Content = "Campo necesario.";
                label_ErrorReference1FirstName.Visibility = Visibility.Visible;
                _IsCorrectStage3[0] = false;
            }

            if (string.IsNullOrEmpty (textBox_Reference1MiddleName.Text)) {
                label_ErrorReference1MiddleName.Content = "Campo necesario.";
                label_ErrorReference1MiddleName.Visibility = Visibility.Visible;
                _IsCorrectStage3[1] = false;
            }

            if (string.IsNullOrEmpty (textBox_Reference1LastName.Text)) {
                label_ErrorReference1LastName.Content = "Campo necesario.";
                label_ErrorReference1LastName.Visibility = Visibility.Visible;
                _IsCorrectStage3[2] = false;
            }

            if (string.IsNullOrEmpty (textBox_Reference1Email.Text)) {
                label_ErrorReference1Email.Content = "Campo necesario.";
                label_ErrorReference1Email.Visibility = Visibility.Visible;
                _IsCorrectStage3[3] = false;
            }

            if (string.IsNullOrEmpty (textBox_Reference1PhoneNumber.Text)) {
                label_ErrorReference1PhoneNumber.Content = "Campo necesario.";
                label_ErrorReference1PhoneNumber.Visibility = Visibility.Visible;
                _IsCorrectStage3[4] = false;
            }

            if (comboBox_Reference1RelationshipType.SelectedIndex == 0) {
                label_ErrorReference1RelationshipType.Content = "Campo necesario.";
                label_ErrorReference1RelationshipType.Visibility = Visibility.Visible;
                _IsCorrectStage3[5] = false;
            } else {
                label_ErrorReference1RelationshipType.Content = "";
                label_ErrorReference1RelationshipType.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[5] = true;
            }

            if (string.IsNullOrEmpty (textBox_Reference2FirstName.Text)) {
                label_ErrorReference2FirstName.Content = "Campo necesario.";
                label_ErrorReference2FirstName.Visibility = Visibility.Visible;
                _IsCorrectStage3[6] = false;
            }

            if (string.IsNullOrEmpty (textBox_Reference2MiddleName.Text)) {
                label_ErrorReference2MiddleName.Content = "Campo necesario.";
                label_ErrorReference2MiddleName.Visibility = Visibility.Visible;
                _IsCorrectStage3[7] = false;
            }

            if (string.IsNullOrEmpty (textBox_Reference2LastName.Text)) {
                label_ErrorReference2LastName.Content = "Campo necesario.";
                label_ErrorReference2LastName.Visibility = Visibility.Visible;
                _IsCorrectStage3[8] = false;
            }

            if (string.IsNullOrEmpty (textBox_Reference2Email.Text)) {
                label_ErrorReference2Email.Content = "Campo necesario.";
                label_ErrorReference2Email.Visibility = Visibility.Visible;
                _IsCorrectStage3[9] = false;
            }

            if (string.IsNullOrEmpty (textBox_Reference2PhoneNumber.Text)) {
                label_ErrorReference2PhoneNumber.Content = "Campo necesario.";
                label_ErrorReference2PhoneNumber.Visibility = Visibility.Visible;
                _IsCorrectStage3[10] = false;
            }

            if (comboBox_Reference2RelationshipType.SelectedIndex == 0) {
                label_ErrorReference2RelationshipType.Content = "Campo necesario.";
                label_ErrorReference2RelationshipType.Visibility = Visibility.Visible;
                _IsCorrectStage3[11] = false;
            } else {
                label_ErrorReference2RelationshipType.Content = "";
                label_ErrorReference2RelationshipType.Visibility = Visibility.Collapsed;
                _IsCorrectStage3[11] = true;
            }
        }

        //Stage 4 validations---------------------------------------------------------------------------
        private void TextChanged_CodeRFC (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidRFC (textBox_CodeCurp.Text, textBox_CodeRFC.Text)) {
                label_ErrorCodeRFC.Content = "RFC no válido o no coincide con el CURP.";
                label_ErrorCodeRFC.Visibility = Visibility.Visible;
                _IsCorrectStage4[0] = false;
            } else {
                label_ErrorCodeRFC.Content = "";
                label_ErrorCodeRFC.Visibility = Visibility.Collapsed;
                _IsCorrectStage4[0] = true;
            }
        }

        private void TextChanged_BanckAccount1CodeCLABE (object sender, TextChangedEventArgs e) {
            textBox_BankAccount1CodeCLABE.Text = CheckFormat.SeparateNumberByGroups (
                                            textBox_BankAccount1CodeCLABE.Text.Replace ("-", ""),
                                            18, 3
                                        );
            textBox_BankAccount1CodeCLABE.CaretIndex = textBox_BankAccount1CodeCLABE.Text.Length;

            if (!CheckFormat.IsValidCLABE (textBox_BankAccount1CodeCLABE.Text)) {
                label_ErrorBankAccount1CodeCLABE.Content = "CLABE no válido.";
                label_ErrorBankAccount1CodeCLABE.Visibility = Visibility.Visible;
                _IsCorrectStage4[2] = false;
            } else {
                label_ErrorBankAccount1CodeCLABE.Content = "";
                label_ErrorBankAccount1CodeCLABE.Visibility = Visibility.Collapsed;
                _IsCorrectStage4[2] = true;
            }
        }

        private void TextChanged_BanckAccount1CardNumber (object sender, TextChangedEventArgs e) {
            textBox_BankAccount1CardNumber.Text = CheckFormat.SeparateNumberByGroups (
                                            textBox_BankAccount1CardNumber.Text.Replace ("-", ""),
                                            16, 4
                                        );
            textBox_BankAccount1CardNumber.CaretIndex = textBox_BankAccount1CardNumber.Text.Length;

            if (!CheckFormat.IsValidCardNumber (textBox_BankAccount1CardNumber.Text)) {
                label_ErrorBankAccount1CardNumber.Content = "Número de tarjeta no válido.";
                label_ErrorBankAccount1CardNumber.Visibility = Visibility.Visible;
                _IsCorrectStage4[3] = false;
            } else {
                label_ErrorBankAccount1CardNumber.Content = "";
                label_ErrorBankAccount1CardNumber.Visibility = Visibility.Collapsed;
                _IsCorrectStage4[3] = true;
            }
        }

        private void TextChanged_BanckAccount2CodeCLABE (object sender, TextChangedEventArgs e) {
            textBox_BankAccount2CodeCLABE.Text = CheckFormat.SeparateNumberByGroups (
                                            textBox_BankAccount2CodeCLABE.Text.Replace ("-", ""),
                                            18, 3
                                        );
            textBox_BankAccount2CodeCLABE.CaretIndex = textBox_BankAccount2CodeCLABE.Text.Length;

            if (!CheckFormat.IsValidCLABE (textBox_BankAccount2CodeCLABE.Text)) {
                label_ErrorBankAccount2CodeCLABE.Content = "CLABE no válido.";
                label_ErrorBankAccount2CodeCLABE.Visibility = Visibility.Visible;
                _IsCorrectStage4[6] = false;
            } else {
                label_ErrorBankAccount2CodeCLABE.Content = "";
                label_ErrorBankAccount2CodeCLABE.Visibility = Visibility.Collapsed;
                _IsCorrectStage4[6] = true;
            }
        }

        private void TextChanged_BanckAccount2CardNumber (object sender, TextChangedEventArgs e) {
            textBox_BankAccount2CardNumber.Text = CheckFormat.SeparateNumberByGroups (
                                            textBox_BankAccount2CardNumber.Text.Replace ("-", ""),
                                            16, 4
                                        );
            textBox_BankAccount2CardNumber.CaretIndex = textBox_BankAccount2CardNumber.Text.Length;

            if (!CheckFormat.IsValidCardNumber (textBox_BankAccount2CardNumber.Text)) {
                label_ErrorBankAccount2CardNumber.Content = "Número de tarjeta no válido.";
                label_ErrorBankAccount2CardNumber.Visibility = Visibility.Visible;
                _IsCorrectStage4[7] = false;
            } else {
                label_ErrorBankAccount2CardNumber.Content = "";
                label_ErrorBankAccount2CardNumber.Visibility = Visibility.Collapsed;
                _IsCorrectStage4[7] = true;
            }
        }

        private void Click_CheckBox_SameAccount (object sender, RoutedEventArgs e) {
            comboBox_BankAccount2Name.IsEnabled = (bool)!checkBox_SameAccount.IsChecked;
            textBox_BankAccount2CodeCLABE.IsEnabled = (bool)!checkBox_SameAccount.IsChecked;
            textBox_BankAccount2CardNumber.IsEnabled = (bool)!checkBox_SameAccount.IsChecked;
            comboBox_BankAccount2CardType.IsEnabled = (bool)!checkBox_SameAccount.IsChecked;
        }

        private void VerifyStage4 () {
            if (string.IsNullOrEmpty (textBox_CodeRFC.Text)) {
                label_ErrorCodeRFC.Content = "Campo necesario.";
                label_ErrorCodeRFC.Visibility = Visibility.Visible;
                _IsCorrectStage4[0] = false;
            }

            if (comboBox_BankAccount1Name.SelectedIndex == 0) {
                label_ErrorBankAccount1Name.Content = "Campo necesario.";
                label_ErrorBankAccount1Name.Visibility = Visibility.Visible;
                _IsCorrectStage4[1] = false;
            } else {
                label_ErrorBankAccount1Name.Content = "";
                label_ErrorBankAccount1Name.Visibility = Visibility.Collapsed;
                _IsCorrectStage4[1] = true;
            }

            if (string.IsNullOrEmpty (textBox_BankAccount1CodeCLABE.Text)) {
                label_ErrorBankAccount1CodeCLABE.Content = "Campo necesario.";
                label_ErrorBankAccount1CodeCLABE.Visibility = Visibility.Visible;
                _IsCorrectStage4[2] = false;
            }

            if (string.IsNullOrEmpty (textBox_BankAccount1CardNumber.Text)) {
                label_ErrorBankAccount1CardNumber.Content = "Campo necesario.";
                label_ErrorBankAccount1CardNumber.Visibility = Visibility.Visible;
                _IsCorrectStage4[3] = false;
            }

            if (comboBox_BankAccount1CardType.SelectedIndex == 0) {
                label_ErrorBankAccount1CardType.Content = "Campo necesario.";
                label_ErrorBankAccount1CardType.Visibility = Visibility.Visible;
                _IsCorrectStage4[4] = false;
            } else {
                label_ErrorBankAccount1CardType.Content = "";
                label_ErrorBankAccount1CardType.Visibility = Visibility.Collapsed;
                _IsCorrectStage4[4] = true;
            }

            if ((bool)!checkBox_SameAccount.IsChecked) {
                if (comboBox_BankAccount2Name.SelectedIndex == 0) {
                    label_ErrorBankAccount2Name.Content = "Campo necesario.";
                    label_ErrorBankAccount2Name.Visibility = Visibility.Visible;
                    _IsCorrectStage4[5] = false;
                } else {
                    label_ErrorBankAccount2Name.Content = "";
                    label_ErrorBankAccount2Name.Visibility = Visibility.Collapsed;
                    _IsCorrectStage4[5] = true;
                }

                if (string.IsNullOrEmpty (textBox_BankAccount2CodeCLABE.Text)) {
                    label_ErrorBankAccount2CodeCLABE.Content = "Campo necesario.";
                    label_ErrorBankAccount2CodeCLABE.Visibility = Visibility.Visible;
                    _IsCorrectStage4[6] = false;
                }

                if (string.IsNullOrEmpty (textBox_BankAccount2CardNumber.Text)) {
                    label_ErrorBankAccount2CardNumber.Content = "Campo necesario.";
                    label_ErrorBankAccount2CardNumber.Visibility = Visibility.Visible;
                    _IsCorrectStage4[7] = false;
                }

                if (comboBox_BankAccount2CardType.SelectedIndex == 0) {
                    label_ErrorBankAccount2CardType.Content = "Campo necesario.";
                    label_ErrorBankAccount2CardType.Visibility = Visibility.Visible;
                    _IsCorrectStage4[8] = false;
                } else {
                    label_ErrorBankAccount2CardType.Content = "";
                    label_ErrorBankAccount2CardType.Visibility = Visibility.Collapsed;
                    _IsCorrectStage4[8] = true;
                }
            } else {
                _IsCorrectStage4[5] = true;
                _IsCorrectStage4[6] = true;
                _IsCorrectStage4[7] = true;
                _IsCorrectStage4[8] = true;
            }
        }
    }
}
