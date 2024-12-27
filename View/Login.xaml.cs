using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
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

            textBox_Email.Text = "administrador@gmail.com";         //Administrador
            //textBox_Email.Text = "cobrador@gmail.com";            //Gestor de cobranza
            //textBox_Email.Text = "analista@gmail.com";            //Analista de crédito
            //textBox_Email.Text = "asesor@gmail.com";                //Asesor de cobranza

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

        private bool VerifyFormLogin () {
            bool isFormCorrect = true;

            if (string.IsNullOrEmpty (textBox_Email.Text)) {
                isFormCorrect = false;
                label_ErrorEmailLogin.Content = "Ingrese su correo.";
                label_ErrorEmailLogin.Visibility = Visibility.Visible;
            } else {
                isFormCorrect = true;
                label_ErrorEmailLogin.Content = "";
                label_ErrorEmailLogin.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty (passwordBox_Password.Password)) {
                isFormCorrect = false;
                label_ErrorPasswordLogin.Content = "Ingrese su contraseña.";
                label_ErrorPasswordLogin.Visibility = Visibility.Visible;
            } else {
                isFormCorrect = true;
                label_ErrorPasswordLogin.Content = "";
                label_ErrorPasswordLogin.Visibility = Visibility.Collapsed;
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
                    {
                        CURRENT_USER.Instance.IdEmployee = messageResponseLogin.DataRetrieved.IdEmployee;
                        CURRENT_USER.Instance.FirstName = messageResponseLogin.DataRetrieved.FirstName;
                        CURRENT_USER.Instance.MiddleName = messageResponseLogin.DataRetrieved.MiddleName;
                        CURRENT_USER.Instance.LastName = messageResponseLogin.DataRetrieved.LastName;
                        CURRENT_USER.Instance.ProfilePhoto = messageResponseLogin.DataRetrieved.ProfilePhoto;
                        CURRENT_USER.Instance.IdRol = messageResponseLogin.DataRetrieved.IdRol;
                        CURRENT_USER.Instance.Rol = messageResponseLogin.DataRetrieved.Rol;
                    }
                    navService.Navigate (new MainWindow ());
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