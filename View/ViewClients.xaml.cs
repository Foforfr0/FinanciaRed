using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FinanciaRed.View {
    /// <summary>
    /// Interaction logic for ViewClients.xaml
    /// </summary>

    public partial class ViewClients : Page {
        public ViewClients () {
            InitializeComponent ();

            List<Client> clients = new List<Client> ()
            {
                new Client{ Nombre = "Juan Fernández", Credito = 10000, CreditoRestante = 8000 },
                new Client{ Nombre = "Ana Vianey J. M.", Credito = 10000, CreditoRestante = 10000 },
                new Client{ Nombre = "Juan Fernández", Credito = 10000, CreditoRestante = 8000 }
            };

            // Asignar la lista como el origen de datos para el DataGrid
            dataGridClients.ItemsSource = clients;
        }

        private void ClicShowDetailsClient (object sender, RoutedEventArgs e) {
        
        }
    }

    public class Client {
        public string Nombre {
            get; set;
        }

        public int Credito {
            get; set;
        }

        public int CreditoRestante {
            get; set;
        }
    }
}
