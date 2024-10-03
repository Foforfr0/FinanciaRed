using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View.ManageClients {
    /// <summary>
    /// Interaction logic for RegisterClient.xaml
    /// </summary>
    public partial class RegisterClient : Window {
        public RegisterClient () {
            InitializeComponent ();

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

            comboBox_AddressType.Items.Clear ();
            comboBox_AddressType.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_AddressType>> messageResponseAT = await DAO_GeneralVariables.GetAllAdressesTypes ();
            List<DTO_AddressType> listAT = messageResponseAT.DataRetrieved;
            foreach (DTO_AddressType type in listAT) {
                comboBox_AddressType.Items.Add (new ComboBoxItem { Content = type.Type });
            }

            comboBox_State.Items.Clear ();
            comboBox_State.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_AddressState>> messageResponseSA = await DAO_GeneralVariables.GetAllAddressStates();
            List<DTO_AddressState> listSA = messageResponseSA.DataRetrieved;
            foreach (DTO_AddressState state in listSA) {
                comboBox_State.Items.Add (new ComboBoxItem { Content = state.Name});
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

        private void ClickFinishRegistry (object sender, RoutedEventArgs e) {

        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar registro.",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes) {
                this.Close ();
            }

        }

        private void ClickShowStage1 (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Visible;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
        }

        private void ClickShowStage2 (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Visible;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
        }

        private void ClickShowStage3 (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Visible;
            stackPanel_Stage4.Visibility = Visibility.Collapsed;
        }

        private void ClickShowStage4 (object sender, RoutedEventArgs e) {
            stackPanel_Stage1.Visibility = Visibility.Collapsed;
            stackPanel_Stage2.Visibility = Visibility.Collapsed;
            stackPanel_Stage3.Visibility = Visibility.Collapsed;
            stackPanel_Stage4.Visibility = Visibility.Visible;
        }
    }
}
