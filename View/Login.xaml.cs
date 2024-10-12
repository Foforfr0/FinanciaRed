using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

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
            if (isPasswordVisible) {
                passwordBox_Password.Visibility = Visibility.Collapsed;
                iconEyePassword.Source = new BitmapImage (new Uri ("Images/icon-eye-open.png", UriKind.Relative));
            } else {
                passwordBox_Password.Visibility = Visibility.Visible;
                iconEyePassword.Source = new BitmapImage (new Uri ("Images/icon-eye-close.png", UriKind.Relative));
            }
        }

        private void ClickRecoverPassword (Object sender, RoutedEventArgs e) {
            MessageBox.Show ("Funcionalidad no disponible");
        }

        private bool VerifyFormLogin () {
            bool isFormCorrect = true;

            if (string.IsNullOrEmpty (textBox_Email.Text)) {
                isFormCorrect = false;
                if (ManageLabelsError.ExistsLabelInStack (stackPanel_FormLogIn, "errorEmailLabel") == false) {
                    stackPanel_FormLogIn.Children.Insert (
                        stackPanel_FormLogIn.Children.IndexOf (textBox_Email) + 1,
                        ManageLabelsError.CreateNewLabel ("errorEmailLabel", "Ingrese su correo.", 11, 55)
                    );
                    stackPanel_MainContainer.Height += 20;
                }
            } else {
                if (ManageLabelsError.ExistsLabelInStack (stackPanel_FormLogIn, "errorEmailLabel")) {
                    ManageLabelsError.RemoveLabel (stackPanel_FormLogIn, "errorEmailLabel");
                    stackPanel_MainContainer.Height -= 20;
                }
            }
            if (string.IsNullOrEmpty (passwordBox_Password.Password)) {
                isFormCorrect = false;
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

            return isFormCorrect;
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

        private void ChangedTextBoxPassword (object sender, EventArgs e) {
            passwordBox_Password.Password = textBox_Password.Text;
        }

        private void ChangedPasswordBoxPassword (object sender, EventArgs e) {
            textBox_Password.Text = passwordBox_Password.Password;
        }
    }
}