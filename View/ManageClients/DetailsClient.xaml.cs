using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FinanciaRed.View.ManageClients {
    /// <summary>
    /// Interaction logic for DetailsClient.xaml
    /// </summary>
    public partial class DetailsClient : Window {
        private DTO_Client_DetailsClient selectedClient = null;

        public DetailsClient (int idClient) {
            InitializeComponent ();

            _ = LoadDetailsClient (idClient);
        }

        private async Task LoadVariables () {
            comboBox_Gender.Items.Clear ();
            comboBox_Gender.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            comboBox_Gender.Items.Add (new ComboBoxItem {
                Content = "Masculino",   // Lo que se verá en el ComboBox
                Tag = 1 // Para almacenar el ID correspondiente
            });
            comboBox_Gender.Items.Add (new ComboBoxItem {
                Content = "Femenino",   // Lo que se verá en el ComboBox
                Tag = 2 // Para almacenar el ID correspondiente
            });

            comboBox_MaritalStatus.Items.Clear ();
            comboBox_MaritalStatus.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_MaritalStatus>> messageResponseMS = await DAO_GeneralVariables.GetAllMaritalStatuses ();
            List<DTO_MaritalStatus> listMS = messageResponseMS.DataRetrieved;
            foreach (DTO_MaritalStatus status in listMS) {
                comboBox_MaritalStatus.Items.Add (new ComboBoxItem {
                    Content = status.Status,   // Lo que se verá en el ComboBox
                    Tag = status.IdMaritalStatus// Para almacenar el ID correspondiente
                });
            }

            comboBox_AddressType.Items.Clear ();
            comboBox_AddressType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_AddressType>> messageResponseAT = await DAO_GeneralVariables.GetAllAdressesTypes ();
            List<DTO_AddressType> listAT = messageResponseAT.DataRetrieved;
            foreach (DTO_AddressType type in listAT) {
                comboBox_AddressType.Items.Add (new ComboBoxItem {
                    Content = type.Type,   // Lo que se verá en el ComboBox
                    Tag = type.IdAddressType// Para almacenar el ID correspondiente
                });
            }

            comboBox_WorkType.Items.Clear ();
            comboBox_WorkType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_WorkType>> messageResponseWT = await DAO_GeneralVariables.GetAllWorkTypes ();
            List<DTO_WorkType> listWT = messageResponseWT.DataRetrieved;
            foreach (DTO_WorkType type in listWT) {
                comboBox_WorkType.Items.Add (new ComboBoxItem {
                    Content = type.Type,   // Lo que se verá en el ComboBox
                    Tag = type.IdWorkType // Para almacenar el ID correspondiente
                });
            }

            comboBox_Reference1RelationshipType.Items.Clear ();
            comboBox_Reference2RelationshipType.Items.Clear ();
            comboBox_Reference1RelationshipType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            comboBox_Reference2RelationshipType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_RelationshipType>> messageResponseRST = await DAO_GeneralVariables.GetAllRelationshipsTypes ();
            List<DTO_RelationshipType> listRST = messageResponseRST.DataRetrieved;
            foreach (DTO_RelationshipType type in listRST) {
                comboBox_Reference1RelationshipType.Items.Add (new ComboBoxItem {
                    Content = type.Type,   // Lo que se verá en el ComboBox
                    Tag = type.IdRelationshipType // Para almacenar el ID correspondiente
                });
                comboBox_Reference2RelationshipType.Items.Add (new ComboBoxItem {
                    Content = type.Type,   // Lo que se verá en el ComboBox
                    Tag = type.IdRelationshipType // Para almacenar el ID correspondiente
                });
            }

            comboBox_BankAccount1Name.Items.Clear ();
            comboBox_BankAccount2Name.Items.Clear ();
            comboBox_BankAccount1Name.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            comboBox_BankAccount2Name.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_Bank>> messageResponseBank = await DAO_GeneralVariables.GetAllBanks ();
            List<DTO_Bank> listBank = messageResponseBank.DataRetrieved;
            foreach (DTO_Bank type in listBank) {
                comboBox_BankAccount1Name.Items.Add (new ComboBoxItem {
                    Content = type.Name,    // Lo que se verá en el ComboBox
                    Tag = type.IdBank       // Para almacenar el ID correspondiente
                });
                comboBox_BankAccount2Name.Items.Add (new ComboBoxItem {
                    Content = type.Name,    // Lo que se verá en el ComboBox
                    Tag = type.IdBank       // Para almacenar el ID correspondiente
                });
            }

            comboBox_BankAccount1CardType.Items.Clear ();
            comboBox_BankAccount2CardType.Items.Clear ();
            comboBox_BankAccount1CardType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            comboBox_BankAccount2CardType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_CardType>> messageResponseTypesCard = await DAO_GeneralVariables.GetAllCardTypes ();
            List<DTO_CardType> listCardTypes = messageResponseTypesCard.DataRetrieved;
            foreach (DTO_CardType type in listCardTypes) {
                comboBox_BankAccount1CardType.Items.Add (new ComboBoxItem {
                    Content = type.Type,    // Lo que se verá en el ComboBox
                    Tag = type.IdCardType       // Para almacenar el ID correspondiente
                });
                comboBox_BankAccount2CardType.Items.Add (new ComboBoxItem {
                    Content = type.Type,    // Lo que se verá en el ComboBox
                    Tag = type.IdCardType       // Para almacenar el ID correspondiente
                });
            }
        }

        private async Task RetrieveDataClientDB (int idClient) {
            MessageResponse<DTO_Client_DetailsClient> messageResponseDetailsClient = await DAO_Client.GetDetailsClient (idClient);
            selectedClient = messageResponseDetailsClient.DataRetrieved;
        }

        private async Task LoadDetailsClient (int idClient) {
            await LoadVariables ();
            await RetrieveDataClientDB (idClient);
            ShowDataClientOverFields ();
        }

        private void ClicAcceptModification (object sender, RoutedEventArgs e) {
            if (VerifyForm ()) {
                DTO_Client_DetailsClient aux = new DTO_Client_DetailsClient {
                    IdClient = selectedClient.IdClient,
                    Gender = comboBox_Gender.SelectedIndex == 1 ? "M" : "F",
                    IdMaritalStatus = comboBox_MaritalStatus.SelectedIndex,
                    Email1 = textBox_Email1.Text,
                    Email2 = textBox_Email2.Text,
                    PhoneNumber1 = textBox_PhoneNumber1.Text,
                    PhoneNumber2 = textBox_PhoneNumber2.Text,
                    AddressClient = new DTO_AddressClient {
                        ExteriorNumber = textBox_ExteriorNumber.Text,
                        InteriorNumber = textBox_InteriorNumber.Text,
                        Street = textBox_Street.Text,
                        Colony = textBox_Colony.Text,
                        PostalCode = textBox_PostalCode.Text,
                        State = textBox_State.Text,
                        IdAddressType = comboBox_AddressType.SelectedIndex
                    },
                    IdWorkType = comboBox_WorkType.SelectedIndex,
                    WorkArea = textBox_WorkArea.Text,
                    MonthlySalary = int.Parse (textBox_MonthlySalary.Text),
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
                    Reference2PhoneNumber = textBox_Reference1PhoneNumber.Text,
                    IdReference2RelationshipType = comboBox_Reference2RelationshipType.SelectedIndex,
                    IdBankAccount1Name = comboBox_BankAccount1Name.SelectedIndex,
                    BankAccount1CLABE = textBox_BankAccount1CodeCLABE.Text,
                    BankAccount1CardNumber = textBox_BankAccount1CardNumber.Text,
                    IdBankAccount1CardType = comboBox_BankAccount1CardType.SelectedIndex,
                    IdBankAccount2Name = comboBox_BankAccount2Name.SelectedIndex,
                    BankAccount2CLABE = textBox_BankAccount2CodeCLABE.Text,
                    BankAccount2CardNumber = textBox_BankAccount2CardNumber.Text,
                    IdBankAccount2CardType = comboBox_BankAccount2CardType.SelectedIndex,
                    StatusActive = label_StatusClient.Content.ToString ()
                };

                MessageResponse<bool> responseUpdateDataClient = DAO_Client.SaveChangesDataClient (aux);
                if (responseUpdateDataClient.IsError) {
                    MessageBox.Show ("Intente más tarde.", "Error inesperado");
                } else {
                    MessageBox.Show ("Se han modificado los datos del cliente satisfactoriamente", "Modificación existosa");
                    SetReadOnly (true);
                }
            }
        }

        private void ClicCancelModification (object sender, RoutedEventArgs e) {
            ShowDataClientOverFields ();

            SetReadOnly (true);
            label_InstructionPage.Visibility = Visibility.Collapsed;
        }

        private void ClicModifyClient (object sender, RoutedEventArgs e) {
            SetReadOnly (false);
            label_InstructionPage.Visibility = Visibility.Visible;
        }

        private void ClicDeleteClient (object sender, RoutedEventArgs e) {
            label_StatusClient.Content = label_StatusClient.Content.Equals ("Activo") ? "Deshabilitado" : "Activo";
            button_Delete.Content = label_StatusClient.Content.Equals ("Activo") ? "Deshabilitar" : "Activar";
            MessageBox.Show ("Falta validar si tiene crédito activo");
        }

        private void SetReadOnly (bool isReadonly) {
            button_Modify.Visibility = isReadonly ? Visibility.Visible : Visibility.Collapsed;
            button_Delete.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            button_Accept.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            button_Cancel.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;

            //-----------------------------------------------Personal data
            textBox_MaritalStatus.IsReadOnly = isReadonly;
            //-----------------------------------------------Address data
            textBox_State.IsReadOnly = isReadonly;
            textBox_PostalCode.IsReadOnly = isReadonly;
            textBox_Colony.IsReadOnly = isReadonly;
            textBox_Street.IsReadOnly = isReadonly;
            textBox_ExteriorNumber.IsReadOnly = isReadonly;
            textBox_InteriorNumber.IsReadOnly = isReadonly;
            textBox_AddressType.IsReadOnly = isReadonly;
            //-----------------------------------------------Contact data
            textBox_Email1.IsReadOnly = isReadonly;
            textBox_Email2.IsReadOnly = isReadonly;
            textBox_PhoneNumber1.IsReadOnly = isReadonly;
            textBox_PhoneNumber2.IsReadOnly = isReadonly;
            //-----------------------------------------------Work data
            textBox_WorkType.IsReadOnly = isReadonly;
            textBox_WorkArea.IsReadOnly = isReadonly;
            textBox_MonthlySalary.IsReadOnly = isReadonly;
            //-----------------------------------------------Reference conatact 1 data
            textBox_Reference1FirstName.IsReadOnly = isReadonly;
            textBox_Reference1MiddleName.IsReadOnly = isReadonly;
            textBox_Reference1LastName.IsReadOnly = isReadonly;
            textBox_Reference1Email.IsReadOnly = isReadonly;
            textBox_Reference1PhoneNumber.IsReadOnly = isReadonly;
            textBox_Reference1RelationshipType.IsReadOnly = isReadonly;
            //-----------------------------------------------Reference conatact 2 data
            textBox_Reference2FirstName.IsReadOnly = isReadonly;
            textBox_Reference2MiddleName.IsReadOnly = isReadonly;
            textBox_Reference2LastName.IsReadOnly = isReadonly;
            textBox_Reference2Email.IsReadOnly = isReadonly;
            textBox_Reference2PhoneNumber.IsReadOnly = isReadonly;
            textBox_Reference2RelationshipType.IsReadOnly = isReadonly;
            //-----------------------------------------------Bank Account 1 data
            textBox_BankAccount1Name.IsReadOnly = isReadonly;
            textBox_BankAccount1CodeCLABE.IsReadOnly = isReadonly;
            textBox_BankAccount1CardNumber.IsReadOnly = isReadonly;
            textBox_BankAccount1CardType.IsReadOnly = isReadonly;
            //-----------------------------------------------Bank Account 2 data
            textBox_BankAccount2Name.IsReadOnly = isReadonly;
            textBox_BankAccount2CodeCLABE.IsReadOnly = isReadonly;
            textBox_BankAccount2CardNumber.IsReadOnly = isReadonly;
            textBox_BankAccount2CardType.IsReadOnly = isReadonly;

            //-----------------------------------------------Personal data
            comboBox_Gender.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            comboBox_MaritalStatus.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            //-----------------------------------------------Work area data
            textBox_WorkArea.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_MonthlySalary.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            //-----------------------------------------------Address data
            textBox_State.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_PostalCode.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Colony.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Street.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_ExteriorNumber.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_InteriorNumber.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_AddressType.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            comboBox_AddressType.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            comboBox_WorkType.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            //-----------------------------------------------Contact data
            textBox_Email1.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Email2.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_PhoneNumber1.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_PhoneNumber2.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            //-----------------------------------------------Reference conatact 1 data
            textBox_Reference1FirstName.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Reference1MiddleName.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Reference1LastName.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Reference1Email.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Reference1PhoneNumber.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            comboBox_Reference1RelationshipType.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            //-----------------------------------------------Reference conatact 2 data
            textBox_Reference2FirstName.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Reference2MiddleName.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Reference2LastName.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Reference2Email.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_Reference2PhoneNumber.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            comboBox_Reference2RelationshipType.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            //-----------------------------------------------Bank Account 1 data
            comboBox_BankAccount1Name.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            textBox_BankAccount1CodeCLABE.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_BankAccount1CardNumber.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            comboBox_BankAccount1CardType.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            //-----------------------------------------------Bank Account 2 data
            comboBox_BankAccount2Name.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
            textBox_BankAccount2CodeCLABE.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            textBox_BankAccount2CardNumber.BorderBrush = isReadonly ? new SolidColorBrush (Colors.Transparent) : new SolidColorBrush (Colors.LightGray);
            comboBox_BankAccount2CardType.Visibility = isReadonly ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ShowDataClientOverFields () {
            //-----------------------------------------------Personal data
            textBox_FirstName.Text = selectedClient.FirstName;
            textBox_MiddleName.Text = selectedClient.MiddleName;
            textBox_LastName.Text = selectedClient.LastName;
            textBox_DateBirth.Text = selectedClient.DateBirth.ToString ("d");
            textBox_Gender.Text = selectedClient.Gender.Equals ("M") ? "Masculino" : "Femenino";
            comboBox_Gender.SelectedIndex = selectedClient.Gender.Equals ("M") ? 1 : 2;
            textBox_MaritalStatus.Text = selectedClient.MaritalStatus;
            comboBox_MaritalStatus.SelectedIndex = selectedClient.IdMaritalStatus;
            textBox_CodeCurp.Text = selectedClient.CodeCurp;
            //-----------------------------------------------Address data
            textBox_State.Text = selectedClient.AddressClient.State;
            textBox_PostalCode.Text = selectedClient.AddressClient.PostalCode;
            textBox_Colony.Text = selectedClient.AddressClient.Colony;
            textBox_Street.Text = selectedClient.AddressClient.Street;
            textBox_ExteriorNumber.Text = selectedClient.AddressClient.ExteriorNumber;
            textBox_InteriorNumber.Text = selectedClient.AddressClient.InteriorNumber;
            textBox_AddressType.Text = selectedClient.AddressClient.AddressType;
            comboBox_AddressType.SelectedIndex = selectedClient.AddressClient.IdAddressType;
            //-----------------------------------------------Contact data
            textBox_Email1.Text = selectedClient.Email1;
            textBox_Email2.Text = selectedClient.Email2;
            textBox_PhoneNumber1.Text = selectedClient.PhoneNumber1;
            textBox_PhoneNumber2.Text = selectedClient.PhoneNumber2;
            //-----------------------------------------------Work data
            textBox_WorkType.Text = selectedClient.WorkType;
            comboBox_WorkType.SelectedIndex = selectedClient.IdWorkType;
            textBox_WorkArea.Text = selectedClient.WorkArea;
            textBox_MonthlySalary.Text = selectedClient.MonthlySalary.ToString ();
            //-----------------------------------------------Reference conatact 1 data
            textBox_Reference1FirstName.Text = selectedClient.Reference1FirstName;
            textBox_Reference1MiddleName.Text = selectedClient.Reference1MiddleName;
            textBox_Reference1LastName.Text = selectedClient.Reference1LastName;
            textBox_Reference1Email.Text = selectedClient.Reference1Email;
            textBox_Reference1PhoneNumber.Text = selectedClient.Reference1PhoneNumber;
            textBox_Reference1RelationshipType.Text = selectedClient.Reference1RelationshipType;
            comboBox_Reference1RelationshipType.SelectedIndex = selectedClient.IdReference1RelationshipType;
            //-----------------------------------------------Reference contact 2 data
            textBox_Reference2FirstName.Text = selectedClient.Reference2FirstName;
            textBox_Reference2MiddleName.Text = selectedClient.Reference2MiddleName;
            textBox_Reference2LastName.Text = selectedClient.Reference2LastName;
            textBox_Reference2Email.Text = selectedClient.Reference2Email;
            textBox_Reference2PhoneNumber.Text = selectedClient.Reference2PhoneNumber;
            textBox_Reference2RelationshipType.Text = selectedClient.Reference2RelationshipType;
            comboBox_Reference2RelationshipType.SelectedIndex = selectedClient.IdReference2RelationshipType;
            //-----------------------------------------------Financial data
            textBox_CodeRFC.Text = selectedClient.CodeRFC;
            //-----------------------------------------------Bank Account 1 data
            textBox_BankAccount1Name.Text = selectedClient.BankAccount1Name;
            comboBox_BankAccount1Name.SelectedIndex = selectedClient.IdBankAccount1Name;
            textBox_BankAccount1CodeCLABE.Text = selectedClient.BankAccount1CLABE;
            textBox_BankAccount1CardNumber.Text = selectedClient.BankAccount1CardNumber;
            textBox_BankAccount1CardType.Text = selectedClient.BankAccount1CardType;
            comboBox_BankAccount1CardType.SelectedIndex = selectedClient.IdBankAccount1CardType;
            //-----------------------------------------------Bank Account 2 data
            textBox_BankAccount2Name.Text = selectedClient.BankAccount2Name;
            comboBox_BankAccount2Name.SelectedIndex = selectedClient.IdBankAccount2Name;
            textBox_BankAccount2CodeCLABE.Text = selectedClient.BankAccount2CLABE;
            textBox_BankAccount2CardNumber.Text = selectedClient.BankAccount2CardNumber;
            textBox_BankAccount2CardType.Text = selectedClient.BankAccount2CardType;
            comboBox_BankAccount2CardType.SelectedIndex = selectedClient.IdBankAccount1CardType;
            //-----------------------------------------------Status client
            label_StatusClient.Content = selectedClient.StatusActive;
        }

        private bool VerifyForm () {
            bool isFormCorrect = true;
            //-----------------------------------------------Personal data
            {
                if (string.IsNullOrEmpty (textBox_FirstName.Text) || !CheckFormat.IsValidNamePerson (textBox_FirstName.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_PersonalData, "errorFirstName") == false)
                        stackPanel_PersonalData.Children.Insert (
                            stackPanel_PersonalData.Children.IndexOf (textBox_FirstName) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorFirstName",
                                string.IsNullOrEmpty (textBox_FirstName.Text) ? "Campo requerido." : "Nombre(s) no válidos.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_PersonalData, "errorFirstName"))
                    ManageLabelsError.RemoveLabel (stackPanel_PersonalData, "errorFirstName");
                if (string.IsNullOrEmpty (textBox_MiddleName.Text) || !CheckFormat.IsValidNamePerson (textBox_MiddleName.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_PersonalData, "errorMiddleName") == false)
                        stackPanel_PersonalData.Children.Insert (
                            stackPanel_PersonalData.Children.IndexOf (textBox_MiddleName) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorMiddleName",
                                string.IsNullOrEmpty (textBox_MiddleName.Text) ? "Campo requerido." : "Apellido paterno no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_PersonalData, "errorMiddleName"))
                    ManageLabelsError.RemoveLabel (stackPanel_PersonalData, "errorMiddleName");
                if (string.IsNullOrEmpty (textBox_LastName.Text) || !CheckFormat.IsValidNamePerson (textBox_LastName.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_PersonalData, "errorLastName") == false)
                        stackPanel_PersonalData.Children.Insert (
                            stackPanel_PersonalData.Children.IndexOf (textBox_LastName) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorLastName",
                                string.IsNullOrEmpty (textBox_LastName.Text) ? "Campo requerido." : "Apellido materno no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_PersonalData, "errorLastName"))
                    ManageLabelsError.RemoveLabel (stackPanel_PersonalData, "errorLastName");
                if (string.IsNullOrEmpty (textBox_LastName.Text) || !CheckFormat.IsValidNamePerson (textBox_LastName.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_PersonalData, "errorLastName") == false)
                        stackPanel_PersonalData.Children.Insert (
                            stackPanel_PersonalData.Children.IndexOf (textBox_LastName) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorLastName",
                                string.IsNullOrEmpty (textBox_LastName.Text) ? "Campo requerido." : "Apellido materno no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_PersonalData, "errorLastName"))
                    ManageLabelsError.RemoveLabel (stackPanel_PersonalData, "errorLastName");
                //TODO Verificar fecha de nacimiento
                if (comboBox_Gender.SelectedIndex == 0) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_Gender, "errorGender") == false)
                        stackPanel_Gender.Children.Insert (
                            2,
                            ManageLabelsError.CreateNewLabel (
                                "errorGender",
                                "Seleccione una opción.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_Gender, "errorGender"))
                    ManageLabelsError.RemoveLabel (stackPanel_Gender, "errorGender");
                if (comboBox_MaritalStatus.SelectedIndex == 0) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_MaritalStatus, "errorMaritalStatus") == false)
                        stackPanel_MaritalStatus.Children.Insert (
                            2,
                            ManageLabelsError.CreateNewLabel (
                                "errorMaritalStatus",
                                "Seleccione una opción.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_MaritalStatus, "errorMaritalStatus"))
                    ManageLabelsError.RemoveLabel (stackPanel_MaritalStatus, "errorMaritalStatus");
                if (string.IsNullOrEmpty (textBox_CodeCurp.Text) || !CheckFormat.IsValidCURP (textBox_CodeCurp.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_PersonalData, "errorCURP") == false)
                        stackPanel_PersonalData.Children.Insert (
                            stackPanel_PersonalData.Children.IndexOf (textBox_CodeCurp) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorCURP",
                                string.IsNullOrEmpty (textBox_CodeCurp.Text) ? "Campo requerido." : "CURP no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_PersonalData, "errorCURP"))
                    ManageLabelsError.RemoveLabel (stackPanel_PersonalData, "errorCURP");
            }
            //-----------------------------------------------Address data
            {
                //TODO Selección de estado a ComboBox
                if (string.IsNullOrEmpty (textBox_State.Text) || !CheckFormat.IsValidNamePerson (textBox_State.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorState") == false)
                        stackPanel_AddressData.Children.Insert (
                            stackPanel_AddressData.Children.IndexOf (textBox_State) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorState",
                                string.IsNullOrEmpty (textBox_State.Text) ? "Campo requerido." : "",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorState"))
                    ManageLabelsError.RemoveLabel (stackPanel_AddressData, "errorState");
                //TODO Agregar municipios como ComboBox y en la base de datos
                if (string.IsNullOrEmpty (textBox_PostalCode.Text) || !CheckFormat.IsValidPostalCode (textBox_PostalCode.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorPostalCode") == false)
                        stackPanel_AddressData.Children.Insert (
                            stackPanel_AddressData.Children.IndexOf (textBox_PostalCode) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorPostalCode",
                                string.IsNullOrEmpty (textBox_PostalCode.Text) ? "Campo requerido." : "Código postal no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorPostalCode"))
                    ManageLabelsError.RemoveLabel (stackPanel_AddressData, "errorPostalCode");
                if (string.IsNullOrEmpty (textBox_Colony.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorColony") == false)
                        stackPanel_AddressData.Children.Insert (
                            stackPanel_AddressData.Children.IndexOf (textBox_Colony) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorColony",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorStreet"))
                    ManageLabelsError.RemoveLabel (stackPanel_AddressData, "errorStreet");
                if (string.IsNullOrEmpty (textBox_Street.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorStreet") == false)
                        stackPanel_AddressData.Children.Insert (
                            stackPanel_AddressData.Children.IndexOf (textBox_Street) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorStreet",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorStreet"))
                    ManageLabelsError.RemoveLabel (stackPanel_AddressData, "errorStreet");
                if (string.IsNullOrEmpty (textBox_ExteriorNumber.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorExteriorNumber") == false)
                        stackPanel_AddressData.Children.Insert (
                            stackPanel_AddressData.Children.IndexOf (textBox_ExteriorNumber) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorExteriorNumber",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorExteriorNumber"))
                    ManageLabelsError.RemoveLabel (stackPanel_AddressData, "errorExteriorNumber");
                if (string.IsNullOrEmpty (textBox_InteriorNumber.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorInteriorNumber") == false)
                        stackPanel_AddressData.Children.Insert (
                            stackPanel_AddressData.Children.IndexOf (textBox_InteriorNumber) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorInteriorNumber",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressData, "errorInteriorNumber"))
                    ManageLabelsError.RemoveLabel (stackPanel_AddressData, "errorInteriorNumber");
                if (comboBox_AddressType.SelectedIndex == 0) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressType, "errorAddressType") == false)
                        stackPanel_AddressType.Children.Insert (
                            2,
                            ManageLabelsError.CreateNewLabel (
                                "errorAddressType",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_AddressType, "errorAddressType"))
                    ManageLabelsError.RemoveLabel (stackPanel_AddressType, "errorAddressType");
            }
            //-----------------------------------------------Contact data
            {
                if (string.IsNullOrEmpty (textBox_Email1.Text) || !CheckFormat.IsValidEmail (textBox_Email1.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactData, "errorEmail1") == false)
                        stackPanel_ContactData.Children.Insert (
                            stackPanel_ContactData.Children.IndexOf (textBox_Email1) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorEmail1",
                                string.IsNullOrEmpty (textBox_Email1.Text) ? "Campo requerido." : "Email no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactData, "errorEmail1"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactData, "errorEmail1");
                if (string.IsNullOrEmpty (textBox_Email2.Text) && !CheckFormat.IsValidEmail (textBox_Email2.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactData, "errorEmail2") == false)
                        stackPanel_ContactData.Children.Insert (
                            stackPanel_ContactData.Children.IndexOf (textBox_Email2) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorEmail2",
                                "Email no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactData, "errorEmail2"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactData, "errorEmail2");
                if (string.IsNullOrEmpty (textBox_PhoneNumber1.Text) || !CheckFormat.IsValidPhoneNumber (textBox_PhoneNumber1.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactData, "errorPhoneNumber1") == false)
                        stackPanel_ContactData.Children.Insert (
                            stackPanel_ContactData.Children.IndexOf (textBox_PhoneNumber1) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorPhoneNumber1",
                                string.IsNullOrEmpty (textBox_PhoneNumber1.Text) ? "Campo requerido." : "Número no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactData, "errorPhoneNumber1"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactData, "errorPhoneNumber1");
                if (string.IsNullOrEmpty (textBox_PhoneNumber2.Text) && !CheckFormat.IsValidPhoneNumber (textBox_PhoneNumber2.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactData, "errorPhoneNumber2") == false)
                        stackPanel_ContactData.Children.Insert (
                            stackPanel_ContactData.Children.IndexOf (textBox_PhoneNumber2) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorPhoneNumber2",
                                "Número no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactData, "errorPhoneNumber2"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactData, "errorPhoneNumber2");
            }
            //-----------------------------------------------Work data
            {
                if (comboBox_WorkType.SelectedIndex == 0) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_WorkType, "errorWorkType") == false)
                        stackPanel_WorkType.Children.Insert (
                            stackPanel_WorkType.Children.IndexOf (comboBox_WorkType) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorWorkType",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_WorkType, "errorWorkType"))
                    ManageLabelsError.RemoveLabel (stackPanel_WorkType, "errorWorkType");
                if (string.IsNullOrEmpty (textBox_WorkArea.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_WorkAreaData, "errorWorkArea") == false)
                        stackPanel_WorkAreaData.Children.Insert (
                            stackPanel_WorkAreaData.Children.IndexOf (textBox_WorkArea) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorWorkArea",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_WorkAreaData, "errorWorkArea"))
                    ManageLabelsError.RemoveLabel (stackPanel_WorkAreaData, "errorWorkArea");
                if (string.IsNullOrEmpty (textBox_MonthlySalary.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_WorkAreaData, "errorMonthlySalary") == false)
                        stackPanel_WorkAreaData.Children.Insert (
                            2,
                            ManageLabelsError.CreateNewLabel (
                                "errorMonthlySalary",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_WorkAreaData, "errorMonthlySalary"))
                    ManageLabelsError.RemoveLabel (stackPanel_WorkAreaData, "errorMonthlySalary");
            }
            //-----------------------------------------------Contact reference 1
            {
                if (string.IsNullOrEmpty (textBox_Reference1FirstName.Text) || !CheckFormat.IsValidNamePerson (textBox_Reference1FirstName.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference1, "errorReference1FirstName") == false)
                        stackPanel_ContactReference1.Children.Insert (
                            stackPanel_ContactReference1.Children.IndexOf (textBox_Reference1FirstName) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference1FirstName",
                                string.IsNullOrEmpty (textBox_Reference1FirstName.Text) ? "Campo requerido." : "Nombre(s) no válidos.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference1, "errorReference1FirstName"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference1, "errorReference1FirstName");
                if (string.IsNullOrEmpty (textBox_Reference1MiddleName.Text) || !CheckFormat.IsValidNamePerson (textBox_Reference1MiddleName.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference1, "errorReference1MiddleName") == false)
                        stackPanel_ContactReference1.Children.Insert (
                            stackPanel_ContactReference1.Children.IndexOf (textBox_Reference1MiddleName) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference1MiddleName",
                                string.IsNullOrEmpty (textBox_Reference1MiddleName.Text) ? "Campo requerido." : "Apellido no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference1, "errorReference1MiddleName"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference1, "errorReference1MiddleName");
                if (string.IsNullOrEmpty (textBox_Reference1LastName.Text) || !CheckFormat.IsValidNamePerson (textBox_Reference1LastName.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference1, "errorReference1LastName") == false)
                        stackPanel_ContactReference1.Children.Insert (
                            stackPanel_ContactReference1.Children.IndexOf (textBox_Reference1LastName) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference1LastName",
                                string.IsNullOrEmpty (textBox_Reference1LastName.Text) ? "Campo requerido." : "Apellido no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference1, "errorReference1LastName"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference1, "errorReference1LastName");
                if (string.IsNullOrEmpty (textBox_Reference1Email.Text) || !CheckFormat.IsValidEmail (textBox_Reference1Email.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference1, "errorReference1Email") == false)
                        stackPanel_ContactReference1.Children.Insert (
                            stackPanel_ContactReference1.Children.IndexOf (textBox_Reference1Email) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference1Email",
                                string.IsNullOrEmpty (textBox_Reference1Email.Text) ? "Campo requerido." : "Apellido no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference1, "errorReference1Email"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference1, "errorReference1Email");
                if (string.IsNullOrEmpty (textBox_Reference1PhoneNumber.Text) || !CheckFormat.IsValidPhoneNumber (textBox_Reference1PhoneNumber.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference1, "errorReference1PhoneNumber") == false)
                        stackPanel_ContactReference1.Children.Insert (
                            stackPanel_ContactReference1.Children.IndexOf (textBox_Reference1PhoneNumber) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference1PhoneNumber",
                                string.IsNullOrEmpty (textBox_Reference1PhoneNumber.Text) ? "Campo requerido." : "Número no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference1, "errorReference1PhoneNumber"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference1, "errorReference1PhoneNumber");
                if (comboBox_Reference1RelationshipType.SelectedIndex == 0) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_RelationShipType1, "errorReference1RelationshipType") == false)
                        stackPanel_RelationShipType1.Children.Insert (
                            2,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference1RelationshipType",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_RelationShipType1, "errorReference1RelationshipType"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference1, "errorReference1RelationshipType");
            }
            //-----------------------------------------------Contact reference 2
            {
                if (string.IsNullOrEmpty (textBox_Reference2FirstName.Text) || !CheckFormat.IsValidNamePerson (textBox_Reference2FirstName.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference2, "errorReference2FirstName") == false)
                        stackPanel_ContactReference2.Children.Insert (
                            stackPanel_ContactReference2.Children.IndexOf (textBox_Reference2FirstName) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference2FirstName",
                                string.IsNullOrEmpty (textBox_Reference2FirstName.Text) ? "Campo requerido." : "Nombre(s) no válidos.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference2, "errorReference2FirstName"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference2, "errorReference2FirstName");
                if (string.IsNullOrEmpty (textBox_Reference2MiddleName.Text) || !CheckFormat.IsValidNamePerson (textBox_Reference2MiddleName.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference2, "errorReference2MiddleName") == false)
                        stackPanel_ContactReference2.Children.Insert (
                            stackPanel_ContactReference2.Children.IndexOf (textBox_Reference2MiddleName) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference2MiddleName",
                                string.IsNullOrEmpty (textBox_Reference2MiddleName.Text) ? "Campo requerido." : "Apellido no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference2, "errorReference2MiddleName"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference2, "errorReference2MiddleName");
                if (string.IsNullOrEmpty (textBox_Reference2LastName.Text) || !CheckFormat.IsValidNamePerson (textBox_Reference2LastName.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference2, "errorReference2LastName") == false)
                        stackPanel_ContactReference2.Children.Insert (
                            stackPanel_ContactReference2.Children.IndexOf (textBox_Reference2LastName) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference2LastName",
                                string.IsNullOrEmpty (textBox_Reference2LastName.Text) ? "Campo requerido." : "Apellido no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference2, "errorReference2LastName"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference2, "errorReference2LastName");
                if (string.IsNullOrEmpty (textBox_Reference2Email.Text) || !CheckFormat.IsValidEmail (textBox_Reference2Email.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference2, "errorReference2Email") == false)
                        stackPanel_ContactReference2.Children.Insert (
                            stackPanel_ContactReference2.Children.IndexOf (textBox_Reference2Email) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference2Email",
                                string.IsNullOrEmpty (textBox_Reference2Email.Text) ? "Campo requerido." : "Apellido no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference2, "errorReference2Email"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference2, "errorReference2Email");
                if (string.IsNullOrEmpty (textBox_Reference2PhoneNumber.Text) || !CheckFormat.IsValidPhoneNumber (textBox_Reference2PhoneNumber.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference2, "errorReference2PhoneNumber") == false)
                        stackPanel_ContactReference2.Children.Insert (
                            stackPanel_ContactReference2.Children.IndexOf (textBox_Reference2PhoneNumber) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference2PhoneNumber",
                                string.IsNullOrEmpty (textBox_Reference2PhoneNumber.Text) ? "Campo requerido." : "Número no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_ContactReference2, "errorReference2PhoneNumber"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference2, "errorReference2PhoneNumber");
                if (comboBox_Reference2RelationshipType.SelectedIndex == 0) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_RelationShipType2, "errorReference2RelationshipType") == false)
                        stackPanel_RelationShipType2.Children.Insert (
                            2,
                            ManageLabelsError.CreateNewLabel (
                                "errorReference2RelationshipType",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_RelationShipType2, "errorReference2RelationshipType"))
                    ManageLabelsError.RemoveLabel (stackPanel_ContactReference2, "errorReference2RelationshipType");
            }
            //-----------------------------------------------Financial data
            {
                if (string.IsNullOrEmpty (textBox_CodeRFC.Text) || !CheckFormat.IsValidRFC (textBox_CodeCurp.Text, textBox_CodeRFC.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_DataBank, "errorRFC") == false)
                        stackPanel_DataBank.Children.Insert (
                            stackPanel_DataBank.Children.IndexOf (textBox_CodeRFC) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorRFC",
                                string.IsNullOrEmpty (textBox_CodeRFC.Text) ? "Campo requerido." : "RFC no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_DataBank, "errorRFC"))
                    ManageLabelsError.RemoveLabel (stackPanel_DataBank, "errorRFC");
            }
            //-----------------------------------------------Banck account 1
            {
                if (comboBox_BankAccount1Name.SelectedIndex == 0) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount1Name, "errorBankAccount1Name") == false)
                        stackPanel_BankAccount1Name.Children.Insert (
                            2,
                            ManageLabelsError.CreateNewLabel (
                                "errorBankAccount1Name",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount1Name, "errorBankAccount1Name"))
                    ManageLabelsError.RemoveLabel (stackPanel_BankAccount1Name, "errorBankAccount1Name");
                if (string.IsNullOrEmpty (textBox_BankAccount1CodeCLABE.Text) || !CheckFormat.IsValidCLABE (textBox_BankAccount1CodeCLABE.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount1, "errorBankAccount1CLABE") == false)
                        stackPanel_BankAccount1.Children.Insert (
                            stackPanel_BankAccount1.Children.IndexOf (textBox_BankAccount1CodeCLABE) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorBankAccount1CLABE",
                                string.IsNullOrEmpty (textBox_BankAccount1CodeCLABE.Text) ? "Campo requerido." : "CLABE no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount1, "errorBankAccount1CLABE"))
                    ManageLabelsError.RemoveLabel (stackPanel_BankAccount1, "errorBankAccount1CLABE");
                if (string.IsNullOrEmpty (textBox_BankAccount1CardNumber.Text) || !CheckFormat.IsValidCardNumber (textBox_BankAccount1CardNumber.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount1, "errorBankAccount1CardNumber") == false)
                        stackPanel_BankAccount1.Children.Insert (
                            stackPanel_BankAccount1.Children.IndexOf (textBox_BankAccount1CardNumber) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorBankAccount1CardNumber",
                                string.IsNullOrEmpty (textBox_BankAccount1CardNumber.Text) ? "Campo requerido." : "Número de tarjeta no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount1, "errorBankAccount1CardNumber"))
                    ManageLabelsError.RemoveLabel (stackPanel_BankAccount1, "errorBankAccount1CardNumber");
                if (comboBox_BankAccount1CardType.SelectedIndex == 0) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount1CardType, "errorBankAccount1CardType") == false)
                        stackPanel_BankAccount1CardType.Children.Insert (
                            2,
                            ManageLabelsError.CreateNewLabel (
                                "errorBankAccount1CardType",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount1CardType, "errorBankAccount1CardType"))
                    ManageLabelsError.RemoveLabel (stackPanel_BankAccount1CardType, "errorBankAccount1CardType");
            }
            //-----------------------------------------------Banck account 2
            {
                if (comboBox_BankAccount2Name.SelectedIndex == 0) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount2Name, "errorBankAccount2Name") == false)
                        stackPanel_BankAccount2Name.Children.Insert (
                            2,
                            ManageLabelsError.CreateNewLabel (
                                "errorBankAccount2Name",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount2Name, "errorBankAccount2Name"))
                    ManageLabelsError.RemoveLabel (stackPanel_BankAccount2Name, "errorBankAccount2Name");
                if (string.IsNullOrEmpty (textBox_BankAccount2CodeCLABE.Text) || !CheckFormat.IsValidCLABE (textBox_BankAccount2CodeCLABE.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount2, "errorBankAccount2CLABE") == false)
                        stackPanel_BankAccount2.Children.Insert (
                            stackPanel_BankAccount2.Children.IndexOf (textBox_BankAccount2CodeCLABE) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorBankAccount2CLABE",
                                string.IsNullOrEmpty (textBox_BankAccount2CodeCLABE.Text) ? "Campo requerido." : "CLABE no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount2, "errorBankAccount2CLABE"))
                    ManageLabelsError.RemoveLabel (stackPanel_BankAccount2, "errorBankAccount2CLABE");
                if (string.IsNullOrEmpty (textBox_BankAccount2CardNumber.Text) || !CheckFormat.IsValidCardNumber (textBox_BankAccount2CardNumber.Text)) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount2, "errorBankAccount2CardNumber") == false)
                        stackPanel_BankAccount2.Children.Insert (
                            stackPanel_BankAccount2.Children.IndexOf (textBox_BankAccount2CardNumber) + 1,
                            ManageLabelsError.CreateNewLabel (
                                "errorBankAccount2CardNumber",
                                string.IsNullOrEmpty (textBox_BankAccount2CardNumber.Text) ? "Campo requerido." : "Número de tarjeta no válido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount2, "errorBankAccount2CardNumber"))
                    ManageLabelsError.RemoveLabel (stackPanel_BankAccount2, "errorBankAccount2CardNumber");
                if (comboBox_BankAccount2CardType.SelectedIndex == 0) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount2CardType, "errorBankAccount2CardType") == false)
                        stackPanel_BankAccount2CardType.Children.Insert (
                            2,
                            ManageLabelsError.CreateNewLabel (
                                "errorBankAccount2CardType",
                                "Campo requerido.",
                                11, 8));
                } else if (ManageLabelsError.ExistsLabelInStack (stackPanel_BankAccount2CardType, "errorBankAccount2CardType"))
                    ManageLabelsError.RemoveLabel (stackPanel_BankAccount2CardType, "errorBankAccount2CardType");
            }
            return isFormCorrect;
        }
    }
}