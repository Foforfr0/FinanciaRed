using FinanciaRed.Model.DTO;
using System.Threading.Tasks;
using System.Windows;

namespace FinanciaRed.View.ManageClients {
    /// <summary>
    /// Interaction logic for ModifyClientData.xaml
    /// </summary>
    public partial class ModifyClientData : Window {
        private DTO_Client_DetailsClient selectedClient = null;

        public ModifyClientData (int idClient) {
            InitializeComponent ();

            _ = RetrieveClientDetails (idClient);
        }

        private async Task RetrieveClientDetails (int idClient) {
            
        }
    }
}
