using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System.Windows;

namespace FinanciaRed.View.ManageClients {
    /// <summary>
    /// Interaction logic for DetailsClient.xaml
    /// </summary>
    public partial class DetailsClient : Window {
        private DTO_Client_DetailsClient selectedClient = null;

        public DetailsClient (int idClient) {
            InitializeComponent ();

            ShowDetailsClient (idClient);
        }

        private async void ShowDetailsClient (int idClient) {
            MessageResponse<DTO_Client_DetailsClient> messageResponseDetailsClient = await DAO_Client.GetDetailsClient (idClient);
            selectedClient = messageResponseDetailsClient.DataRetrieved;
            textBox_FirstName.Text = selectedClient.FirstName;
            textBox_MiddleName.Text = selectedClient.MiddleName;
            textBox_LastName.Text = selectedClient.LastName;
            textBox_DateBirth.Text = selectedClient.DateBirth;
            textBox_Gender.Text = selectedClient.Gender;
            textBox_MaritalStatus.Text = selectedClient.MaritalStatus;
            textBox_CodeCurp.Text = selectedClient.CodeCurp;
            textBox_Address.Text = selectedClient.PersonalAddress;
            textBox_Email1.Text = selectedClient.Email1;
            textBox_Email2.Text = selectedClient.Email2;
            textBox_PhoneNumber1.Text = selectedClient.PhoneNumber1;
            textBox_PhoneNumber2.Text = selectedClient.PhoneNumber2;
            textBox_WorkType.Text = selectedClient.WorkType;
            textBox_WorkArea.Text = selectedClient.WorkArea;
            textBox_MonthlySalary.Text = selectedClient.MonthlySalary.ToString();
            textBox_Reference1FirstName.Text = selectedClient.Reference1FirstName;
            textBox_Reference1MiddleName.Text = selectedClient.Reference1MiddleName;
            textBox_Reference1LastName.Text = selectedClient.Reference1LastName;
            textBox_Reference1Email.Text = selectedClient.Reference1Email;
            textBox_Reference1PhoneNumber.Text = selectedClient.Reference1PhoneNumber;
            textBox_Reference1RelationshipType.Text = selectedClient.Reference1RelationshipType;
            textBox_Reference2FirstName.Text = selectedClient.Reference2FirstName;
            textBox_Reference2MiddleName.Text = selectedClient.Reference2MiddleName;
            textBox_Reference2LastName.Text = selectedClient.Reference2LastName;
            textBox_Reference2Email.Text = selectedClient.Reference2Email;
            textBox_Reference2PhoneNumber.Text = selectedClient.Reference2PhoneNumber;
            textBox_Reference2RelationshipType.Text = selectedClient.Reference2RelationshipType;
            textBox_CodeRFC.Text = selectedClient.CodeRFC;
            textBox_BankAccount1Name.Text = selectedClient.BankAccount1Name;
            textBox_BankAccount1CodeCLABE.Text = selectedClient.BankAccount1CLABE;
            textBox_BankAccount1CardNumber.Text = selectedClient.BankAccount1CardNumber;
            textBox_BankAccount1CardType.Text = selectedClient.BankAccount1CardType;
            textBox_BankAccount2Name.Text = selectedClient.BankAccount2Name;
            textBox_BankAccount2CodeCLABE.Text = selectedClient.BankAccount2CLABE;
            textBox_BankAccount2CardNumber.Text = selectedClient.BankAccount2CardNumber;
            textBox_BankAccount2CardType.Text = selectedClient.BankAccount2CardType;
        }

        private void ClicAcceptModification (object sender, RoutedEventArgs e) {

        }

        private void ClicCancelModification (object sender, RoutedEventArgs e) {
            button_Modify.Visibility = Visibility.Visible;
            button_Accept.Visibility = Visibility.Collapsed;
            button_Cancel.Visibility = Visibility.Collapsed;

            textBox_MaritalStatus.Text = selectedClient.MaritalStatus;
            textBox_Address.Text = selectedClient.PersonalAddress;
            textBox_Email1.Text = selectedClient.Email1;
            textBox_Email2.Text = selectedClient.Email2;
            textBox_PhoneNumber1.Text = selectedClient.PhoneNumber1;
            textBox_PhoneNumber2.Text = selectedClient.PhoneNumber2;
            textBox_WorkType.Text = selectedClient.WorkType;
            textBox_WorkArea.Text = selectedClient.WorkArea;
            textBox_MonthlySalary.Text = selectedClient.MonthlySalary.ToString ();
            textBox_Reference1FirstName.Text = selectedClient.Reference1FirstName;
            textBox_Reference1MiddleName.Text = selectedClient.Reference1MiddleName;
            textBox_Reference1LastName.Text = selectedClient.Reference1LastName;
            textBox_Reference1Email.Text = selectedClient.Reference1Email;
            textBox_Reference1PhoneNumber.Text = selectedClient.Reference1PhoneNumber;
            textBox_Reference1RelationshipType.Text = selectedClient.Reference1RelationshipType;
            textBox_Reference2FirstName.Text = selectedClient.Reference2FirstName;
            textBox_Reference2MiddleName.Text = selectedClient.Reference2MiddleName;
            textBox_Reference2LastName.Text = selectedClient.Reference2LastName;
            textBox_Reference2Email.Text = selectedClient.Reference2Email;
            textBox_Reference2PhoneNumber.Text = selectedClient.Reference2PhoneNumber;
            textBox_Reference2RelationshipType.Text = selectedClient.Reference2RelationshipType;
            textBox_BankAccount1Name.Text = selectedClient.BankAccount1Name;
            textBox_BankAccount1CodeCLABE.Text = selectedClient.BankAccount1CLABE;
            textBox_BankAccount1CardNumber.Text = selectedClient.BankAccount1CardNumber;
            textBox_BankAccount1CardType.Text = selectedClient.BankAccount1CardType;
            textBox_BankAccount2Name.Text = selectedClient.BankAccount2Name;
            textBox_BankAccount2CodeCLABE.Text = selectedClient.BankAccount2CLABE;
            textBox_BankAccount2CardNumber.Text = selectedClient.BankAccount2CardNumber;
            textBox_BankAccount2CardType.Text = selectedClient.BankAccount2CardType;

            textBox_MaritalStatus.IsReadOnly = true;
            textBox_Address.IsReadOnly = true;
            textBox_Email1.IsReadOnly = true;
            textBox_Email2.IsReadOnly = true;
            textBox_PhoneNumber1.IsReadOnly = true;
            textBox_PhoneNumber2.IsReadOnly = true;
            textBox_Reference1FirstName.IsReadOnly = true;
            textBox_Reference1MiddleName.IsReadOnly = true;
            textBox_Reference1LastName.IsReadOnly = true;
            textBox_Reference1Email.IsReadOnly = true;
            textBox_Reference1PhoneNumber.IsReadOnly = true;
            textBox_Reference1RelationshipType.IsReadOnly = true;
            textBox_Reference2FirstName.IsReadOnly = true;
            textBox_Reference2MiddleName.IsReadOnly = true;
            textBox_Reference2LastName.IsReadOnly = true;
            textBox_Reference2Email.IsReadOnly = true;
            textBox_Reference2PhoneNumber.IsReadOnly = true;
            textBox_Reference2RelationshipType.IsReadOnly = true;
            textBox_BankAccount1Name.IsReadOnly = true;
            textBox_BankAccount1CodeCLABE.IsReadOnly = true;
            textBox_BankAccount1CardNumber.IsReadOnly = true;
            textBox_BankAccount1CardType.IsReadOnly = true;
            textBox_BankAccount2Name.IsReadOnly = true;
            textBox_BankAccount2CodeCLABE.IsReadOnly = true;
            textBox_BankAccount2CardNumber.IsReadOnly = true;
            textBox_BankAccount2CardType.IsReadOnly = true;
        }

        private void ClicModifyClient (object sender, RoutedEventArgs e) {
            button_Modify.Visibility = Visibility.Collapsed;
            button_Accept.Visibility = Visibility.Visible;
            button_Cancel.Visibility = Visibility.Visible;

            textBox_MaritalStatus.IsReadOnly = false;
            textBox_Address.IsReadOnly = false;
            textBox_Email1.IsReadOnly = false;
            textBox_Email2.IsReadOnly = false;
            textBox_PhoneNumber1.IsReadOnly = false;
            textBox_PhoneNumber2.IsReadOnly = false;
            textBox_Reference1FirstName.IsReadOnly = false;
            textBox_Reference1MiddleName.IsReadOnly = false;
            textBox_Reference1LastName.IsReadOnly = false;
            textBox_Reference1Email.IsReadOnly = false;
            textBox_Reference1PhoneNumber.IsReadOnly = false;
            textBox_Reference1RelationshipType.IsReadOnly = false;
            textBox_Reference2FirstName.IsReadOnly = false;
            textBox_Reference2MiddleName.IsReadOnly = false;
            textBox_Reference2LastName.IsReadOnly = false;
            textBox_Reference2Email.IsReadOnly = false;
            textBox_Reference2PhoneNumber.IsReadOnly = false;
            textBox_Reference2RelationshipType.IsReadOnly = false;
            textBox_BankAccount1Name.IsReadOnly = false;
            textBox_BankAccount1CodeCLABE.IsReadOnly = false;
            textBox_BankAccount1CardNumber.IsReadOnly = false;
            textBox_BankAccount1CardType.IsReadOnly = false;
            textBox_BankAccount2Name.IsReadOnly = false;
            textBox_BankAccount2CodeCLABE.IsReadOnly = false;
            textBox_BankAccount2CardNumber.IsReadOnly = false;
            textBox_BankAccount2CardType.IsReadOnly = false;
        }
    }
}
