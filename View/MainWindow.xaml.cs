using FinanciaRed.Model.DTO;
using FinanciaRed.Utils;
using FinanciaRed.View.ManageClients;
using FinanciaRed.View.ManageCreditRequests;
using FinanciaRed.View.ManageCredits;
using FinanciaRed.View.ManageEmployees;
using FinanciaRed.View.ManageUsers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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

            if (currentEmployee.ProfilePhoto == null) {
                image_ImageProfile.Source = new BitmapImage (new Uri ("./Images/icon-user.png", UriKind.Relative));
            } else {
                image_ImageProfile.Source = Converters.ConvertByteToBitmapImage (currentEmployee.ProfilePhoto);
            }
            label_NameEmployee.Content = currentEmployee.FirstName + " " + currentEmployee.MiddleName + " " + currentEmployee.LastName + " ";
            label_RolEmployee.Content = currentEmployee.Rol;
            ShowAvailableOptionsUser ();
        }

        //TODO
        //Refinar los accesos de cada tipo de usuario
        private void ShowAvailableOptionsUser () {
            if (currentEmployee.IdRol == 1) {       //Ases@r de crédito
                label_GlobalOptionClients.Visibility = Visibility.Visible;
                label_GlobalOptionCredits.Visibility = Visibility.Visible;
                label_GlobalOptionEmployees.Visibility = Visibility.Collapsed;
            }
            if (currentEmployee.IdRol == 2) {       //Analista de crédito
                label_GlobalOptionClients.Visibility = Visibility.Visible;
                label_GlobalOptionCredits.Visibility = Visibility.Visible;
                label_GlobalOptionEmployees.Visibility = Visibility.Collapsed;
            }
            if (currentEmployee.IdRol == 3) {       //Administrador
                label_GlobalOptionClients.Visibility = Visibility.Visible;
                label_GlobalOptionCredits.Visibility = Visibility.Visible;
                label_GlobalOptionEmployees.Visibility = Visibility.Visible;
            }
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

        private void ClickShowManagementCreditRequestsFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewCreditRequests ());
        }

        private void ClickShowManagementEmployeesFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewEmployees ());
        }

        private void ClickLogOut (object sender, RoutedEventArgs e) {
            NavigationService navService = NavigationService.GetNavigationService (this);
            navService.Navigate (new Uri ("View/Login.xaml", UriKind.Relative));
        }
    }
}
