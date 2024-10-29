using FinanciaRed.Model.DAO;
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
        public MainWindow () {
            InitializeComponent ();

            InitializeDataEmployee ();
        }

        public void InitializeDataEmployee () {
            if (CurrentUser.Instance.ProfilePhoto == null) {
                image_ImageProfile.Source = new BitmapImage (new Uri ("./Images/icon-user.png", UriKind.Relative));
            } else {
                image_ImageProfile.Source = Converters.ConvertByteToBitmapImage (CurrentUser.Instance.ProfilePhoto);
            }
            label_NameEmployee.Content = CurrentUser.Instance.FirstName + " " + CurrentUser.Instance.MiddleName + " " + CurrentUser.Instance.LastName + " ";
            label_RolEmployee.Content = CurrentUser.Instance.Rol;
            ShowAvailableOptionsUser ();
        }

        //TODO
        //Refinar los accesos de cada tipo de usuario
        private void ShowAvailableOptionsUser () {
            if (CurrentUser.Instance.IdRol == 1) {       //Ases@r de crédito
                label_GlobalOptionClients.Visibility = Visibility.Visible;
                label_GlobalOptionCredits.Visibility = Visibility.Visible;
                label_GlobalOptionEmployees.Visibility = Visibility.Collapsed;
            }
            if (CurrentUser.Instance.IdRol == 2) {       //Analista de crédito
                label_GlobalOptionClients.Visibility = Visibility.Visible;
                label_GlobalOptionCredits.Visibility = Visibility.Visible;
                label_GlobalOptionEmployees.Visibility = Visibility.Collapsed;
            }
            if (CurrentUser.Instance.IdRol == 3) {       //Administrador
                label_GlobalOptionClients.Visibility = Visibility.Visible;
                label_GlobalOptionCredits.Visibility = Visibility.Visible;
                label_GlobalOptionEmployees.Visibility = Visibility.Visible;
            }
        }

        private void ClickCheckAccount (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new CheckProfile (CurrentUser.Instance.IdEmployee));
        }
        private void ClickShowManagementClientsFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewClients ());
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