using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Utils;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace FinanciaRed.View {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page {
        private bool isPasswordVisible = false;

        public Login () {
            InitializeComponent ();

            textBox_Email.Text = "foforfr007@gmail.com";
            passwordBox_Password.Password = "1234";
        }

        private void ClicShowPassword (object sender, RoutedEventArgs e) {
            isPasswordVisible = !isPasswordVisible;
            /* after form
            if (isPasswordVisible) {
                iconEyePassword.Source = new BitmapImage (new Uri ("Images/icon-eye-open.png", UriKind.Relative));
            } else {
                iconEyePassword.Source = new BitmapImage (new Uri ("Images/icon-eye-close.png", UriKind.Relative));
            }*/
        }

        private void ClickRecoverPassword (Object sender, RoutedEventArgs e) {
            MessageBox.Show ("Funcionalidad no disponible");
        }

        private bool VerifyFormLogin () {
            bool IsFormCorrect = true;
            
            if (textBox_Email.Text.Length == 0) {
                IsFormCorrect = false;
                if (ManageLabelsError.ExistsLabelInStack (stackPanel_FormLogIn, "errorEmailLabel") == false) {
                    stackPanel_FormLogIn.Children.Insert (
                        stackPanel_FormLogIn.Children.IndexOf (textBox_Email) + 1,
                        ManageLabelsError.CreateNewLabel ("errorEmailLabel", "Ingrese su correo.", 11, 55)
                    );
                    stackPanel_MainContainer.Height += 20;
                }
            } else {
                if (ManageLabelsError.ExistsLabelInStack(stackPanel_FormLogIn, "errorEmailLabel")) {
                    ManageLabelsError.RemoveLabel (stackPanel_FormLogIn, "errorEmailLabel");
                    stackPanel_MainContainer.Height -= 20;
                }
            }
            if (passwordBox_Password.Password.Length == 0) {
                IsFormCorrect = false;
                if (ManageLabelsError.ExistsLabelInStack (stackPanel_FormLogIn, "errorPasswordLabel") == false) {
                    stackPanel_FormLogIn.Children.Insert (
                        stackPanel_FormLogIn.Children.IndexOf (passwordBox_Password) + 1,
                        ManageLabelsError.CreateNewLabel ("errorPasswordLabel", "Ingrese su contraseña.", 11, 55)
                    );
                    stackPanel_MainContainer.Height += 20;
                }
            } else {
                if (ManageLabelsError.ExistsLabelInStack (stackPanel_FormLogIn, "errorPasswordLabel")) {
                    ManageLabelsError.RemoveLabel (stackPanel_FormLogIn, "errorPasswordLabel");
                    stackPanel_MainContainer.Height -= 20;
                }
            }

            return IsFormCorrect;
        }

        private async void ClickLogin (object sender, RoutedEventArgs e) {
            if (VerifyFormLogin () == true) {
                string emailLogin = textBox_Email.Text;
                string passwordLogin = passwordBox_Password.Password;
                MessageResponse<DTO_Employee_Login> messageResponseLogin = 
                    await DAO_Employee.GetLogin (emailLogin, passwordLogin);

                if (messageResponseLogin.DataRetrieved == null) {
                    MessageBox.Show (
                        "Correo electrónico o contraseña incorrectas.",
                        "Inicio de sesión incorrecto"
                    );
                } else {
                    NavigationService navService = NavigationService.GetNavigationService (this);
                    navService.Navigate (new MainWindow (messageResponseLogin.DataRetrieved));
                }
            }
        }
    }
}