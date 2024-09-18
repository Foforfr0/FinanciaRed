using FinanciaRed.Model.DTO;
using FinanciaRed.View.ManageClients;
using FinanciaRed.View.ManageCredits;
using FinanciaRed.View.ManageEmployees;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FinanciaRed.View {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page {
        private DTO_Employee_Login currentEmployee;

        public MainWindow () {
            InitializeComponent ();
        }

        public MainWindow (DTO_Employee_Login currentEmployee) {
            InitializeComponent ();

            InitializeDataEmployee (currentEmployee);
        }

        private void InitializeDataEmployee (DTO_Employee_Login currentEmployee) {
            this.currentEmployee = currentEmployee;

            label_NameEmployee.Content = currentEmployee.FirstName + " " + currentEmployee.MiddleName + " " + currentEmployee.LastName + " ";
            label_RolEmployee.Content = currentEmployee.Rol;
        }

        private void ClickCheckAccount (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new CheckProfile (currentEmployee.IdEmployee));
        }
        private void ClickShowManagementClientsFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewClients());
        }
        private void ClickShowManagementCreditsFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewCredits ());
        }
        private void ClickLogOut (object sender, RoutedEventArgs e) {
            NavigationService navService = NavigationService.GetNavigationService (this);
            navService.Navigate (new Uri ("View/Login.xaml", UriKind.Relative));
        }
    }
}
