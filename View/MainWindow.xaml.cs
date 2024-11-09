using FinanciaRed.Model.DAO;
using FinanciaRed.Utils;
using FinanciaRed.View.ManageClients;
using FinanciaRed.View.ManageCreditApplications;
using FinanciaRed.View.ManageCreditPolicies;
using FinanciaRed.View.ManageCredits;
using FinanciaRed.View.ManageEmployees;
using FinanciaRed.View.ManageMonthlyEfficiencies;
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
            if (CURRENT_USER.Instance.ProfilePhoto == null) {
                image_ImageProfile.Source = new BitmapImage (new Uri ("./Images/icon-user.png", UriKind.Relative));
            } else {
                image_ImageProfile.Source = Converters.ConvertByteToBitmapImage (CURRENT_USER.Instance.ProfilePhoto);
            }
            label_NameEmployee.Content = CURRENT_USER.Instance.FirstName + " " + CURRENT_USER.Instance.MiddleName + " " + CURRENT_USER.Instance.LastName + " ";
            label_RolEmployee.Content = CURRENT_USER.Instance.Rol;
            ShowAvailableOptionsUser ();
        }

        private void ShowAvailableOptionsUser () {
            if (CURRENT_USER.Instance.IdRol == 1) {       //Administrador
                label_GlobalOptionEmployees.Visibility = Visibility.Visible;
                label_GlobalOptionCreditPolicies.Visibility = Visibility.Visible;
                label_GlobalOptionMonthlyEfficiencies.Visibility = Visibility.Visible;
            }
            if (CURRENT_USER.Instance.IdRol == 2) {       //Gestor de cobranza
                label_GlobalOptionCredits.Visibility = Visibility;
                label_GlobalOptionMonthlyEfficiencies.Visibility = Visibility.Visible;
            }
            if (CURRENT_USER.Instance.IdRol == 3) {       //Analista de crédito
                label_GlobalOptionCreditApplications.Visibility = Visibility.Visible;
                label_GlobalOptionCreditPolicies.Visibility = Visibility.Visible;
            }
            if (CURRENT_USER.Instance.IdRol == 4) {      //Aasesor de crédito
                label_GlobalOptionClients.Visibility = Visibility.Visible;
                label_GlobalOptionCreditApplications.Visibility = Visibility.Visible;
            }
        }

        private void ClickCheckAccount (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new CheckProfile (CURRENT_USER.Instance.IdEmployee));
        }
        private void ClickShowManagementClientsFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewClients ());
        }
        private void ClickShowManagementCreditsFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewCredits ());
        }

        private void ClickShowManagementCreditRequestsFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewCreditApplications ());
        }

        private void ClickShowManagementEmployeesFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewEmployees ());
        }

        private void ClickShowManagementPoliciesFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewCreditPolicies ());
        }

        private void ClickShowManagementMonthlyEfficienciesFrame (object sender, RoutedEventArgs e) {
            innerFrameContainer.Navigate (new ViewMonthlyEfficiencies ());
        }

        private void ClickLogOut (object sender, RoutedEventArgs e) {
            NavigationService navService = NavigationService.GetNavigationService (this);
            navService.Navigate (new Uri ("View/Login.xaml", UriKind.Relative));
        }
    }
}