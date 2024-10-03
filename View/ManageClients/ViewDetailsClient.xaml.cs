using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace FinanciaRed.View.ManageClients {
    /// <summary>
    /// Interaction logic for ViewDetailsClient.xaml
    /// </summary>
    public partial class ViewDetailsClient : Window {
        private DTO_Client_DetailsClient selectedClient = null;

        public ViewDetailsClient (int idClient) {
            InitializeComponent ();

            _ = LoadDetailsClient (idClient);
        }

        private async Task RetrieveDataClientDB (int idClient) {
            MessageResponse<DTO_Client_DetailsClient> messageResponseDetailsClient = await DAO_Client.GetDetailsClient (idClient);
            selectedClient = messageResponseDetailsClient.DataRetrieved;
        }

        private async Task LoadDetailsClient (int idClient) {
            await RetrieveDataClientDB (idClient);
            ShowDataClientOverFields ();
        }

        private void ShowDataClientOverFields () {
            //-----------------------------------------------Personal data
            label_FirstName.Content = selectedClient.FirstName;
            label_MiddleName.Content = selectedClient.MiddleName;
            label_LastName.Content = selectedClient.LastName;
            label_DateBirth.Content = selectedClient.DateBirth.ToString ("d");
            label_Gender.Content = selectedClient.Gender.Equals ("M") ? "Masculino" : "Femenino";
            label_MaritalStatus.Content = selectedClient.MaritalStatus;
            label_CodeCURP.Content = selectedClient.CodeCurp;
            //-----------------------------------------------Address data
            label_State.Content = selectedClient.AddressClient.State;
            label_PostalCode.Content = selectedClient.AddressClient.PostalCode;
            label_Colony.Content = selectedClient.AddressClient.Colony;
            label_Street.Content = selectedClient.AddressClient.Street;
            label_ExteriorNumber.Content = selectedClient.AddressClient.ExteriorNumber;
            label_InteriorNumber.Content = selectedClient.AddressClient.InteriorNumber;
            label_AddressType.Content = selectedClient.AddressClient.AddressType;
            //-----------------------------------------------Contact data
            label_Email1.Content = selectedClient.Email1;
            label_Email2.Content = selectedClient.Email2;
            label_PhoneNumber1.Content = selectedClient.PhoneNumber1;
            label_PhoneNumber2.Content = selectedClient.PhoneNumber2;
            //-----------------------------------------------Work data
            label_WorkType.Content = selectedClient.WorkType;
            label_WorkArea.Content = selectedClient.WorkArea;
            label_MonthlySalary.Content = selectedClient.MonthlySalary.ToString ();
            //-----------------------------------------------Reference conatact 1 data
            label_Reference1FirstName.Content = selectedClient.Reference1FirstName;
            label_Reference1MiddleName.Content = selectedClient.Reference1MiddleName;
            label_Reference1LastName.Content = selectedClient.Reference1LastName;
            label_Reference1Email.Content = selectedClient.Reference1Email;
            label_Reference1PhoneNumber.Content = selectedClient.Reference1PhoneNumber;
            label_Reference1RelationshipType.Content = selectedClient.Reference1RelationshipType;
            //-----------------------------------------------Reference contact 2 data
            label_Reference2FirstName.Content = selectedClient.Reference2FirstName;
            label_Reference2MiddleName.Content = selectedClient.Reference2MiddleName;
            label_Reference2LastName.Content = selectedClient.Reference2LastName;
            label_Reference2Email.Content = selectedClient.Reference2Email;
            label_Reference2PhoneNumber.Content = selectedClient.Reference2PhoneNumber;
            label_Reference2RelationshipType.Content = selectedClient.Reference2RelationshipType;
            //-----------------------------------------------Financial data
            label_CodeRFC.Content = selectedClient.CodeRFC;
            //-----------------------------------------------Bank Account 1 data
            label_BankAccount1Name.Content = selectedClient.BankAccount1Name;
            label_BankAccount1CodeCLABE.Content = selectedClient.BankAccount1CLABE;
            label_BankAccount1CardNumber.Content = selectedClient.BankAccount1CardNumber;
            label_BankAccount1CardType.Content = selectedClient.BankAccount1CardType;
            //-----------------------------------------------Bank Account 2 data
            label_BankAccount2Name.Content = selectedClient.BankAccount2Name;
            label_BankAccount2CodeCLABE.Content = selectedClient.BankAccount2CLABE;
            label_BankAccount2CardNumber.Content = selectedClient.BankAccount2CardNumber;
            label_BankAccount2CardType.Content = selectedClient.BankAccount2CardType;
            //-----------------------------------------------Status client
            label_StatusClient.Content = selectedClient.StatusActive;
        }

        private void ClicModifyClient (object sender, EventArgs e) {
            ModifyClientData modifyClientDataWindow = new ModifyClientData (selectedClient.IdClient);
            modifyClientDataWindow.ShowDialog ();
        }
    }
}