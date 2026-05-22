using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FinanciaRed.View.ManageEmployees {
    /// <summary>
    /// Interaction logic for RegisterEmployee.xaml
    /// </summary>
    public partial class RegisterEmployee : Window {
        private byte[] TEMP_profileImageSelected = null;
        private bool[] _IsCorrectStage1 = new bool[11] { false, false, false, false, false, false, false, false, false, false, false };

        public RegisterEmployee () {
            InitializeComponent ();

            _ = LoadVariables ();
        }

        private async Task LoadVariables () {
            comboBox_Gender.Items.Clear ();
            comboBox_Gender.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            comboBox_Gender.Items.Add (new ComboBoxItem { Content = "Masculino" });
            comboBox_Gender.Items.Add (new ComboBoxItem { Content = "Femenino" });

            // Set the DisplayDateStart to 100 years ago
            datePicker_DateBirth.DisplayDateStart = DateTime.Now.AddYears (-100);
            // Set the DisplayDateEnd to the current date
            datePicker_DateBirth.DisplayDateEnd = DateTime.Now.AddYears (-18);

            comboBox_Rol.Items.Clear ();
            comboBox_Rol.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_EmployeeRol>> messageResponseER = await DAO_GeneralVariables.GetAllEmployeeRoles ();
            List<DTO_EmployeeRol> listER = messageResponseER.DataRetrieved;
            foreach (DTO_EmployeeRol rol in listER) {
                comboBox_Rol.Items.Add (new ComboBoxItem { Content = rol.Rol });
            }

            await Task.Delay (200);
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar registro.",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes)
                this.Close ();
        }

        private async void ClickFinishRegistration (object sender, RoutedEventArgs e) {
            VerifyStage1 ();

            MessageBox.Show (string.Concat (_IsCorrectStage1[0]) + " " + string.Concat (_IsCorrectStage1[1]) + " " + string.Concat (_IsCorrectStage1[2]) + " " +
                             string.Concat (_IsCorrectStage1[3]) + " " + string.Concat (_IsCorrectStage1[4]) + " " + string.Concat (_IsCorrectStage1[5]) + " " +
                             string.Concat (_IsCorrectStage1[6]) + " " + string.Concat (_IsCorrectStage1[7]) + " " + string.Concat (_IsCorrectStage1[8]) + " " +
                             string.Concat (_IsCorrectStage1[9]) + " " + string.Concat (_IsCorrectStage1[10]));

            if (_IsCorrectStage1.All (x => x == true)) {
                bool existsEmail = await DAO_Employee.VerifyExistenceEmail (textBox_Email.Text);
                bool existsCodeCURP = await DAO_Employee.VerifyExistenceCURP (textBox_CodeCURP.Text);
                bool existsCodeRFC = await DAO_Employee.VerifyExistenceRFC (textBox_CodeRFC.Text);
                await Task.Delay (200);

                if (existsEmail) {
                    label_ErrorEmail.Content = "Email ya existente en la base de datos.";
                    label_ErrorEmail.Visibility = Visibility.Visible;
                }
                if (existsCodeRFC) {
                    label_ErrorCodeRFC.Content = "RFC ya existente en la base de datos.";
                    label_ErrorCodeRFC.Visibility = Visibility.Visible;
                }
                if (existsCodeCURP) {
                    label_ErrorCodeCURP.Content = "CURP ya existente en la base de datos.";
                    label_ErrorCodeCURP.Visibility = Visibility.Visible;
                }
                if (!existsEmail && !existsCodeCURP && !existsCodeRFC) {
                    await SaveDataInDatabase ();
                }
            } else {
                MessageBox.Show ("Faltan datos por ingresar o algunos datos están incorrectos.", "Formulario incompleto.");
            }
        }

        private async Task SaveDataInDatabase () {
            DTO_Employee_DetailsEmployee newDataEmployee = new DTO_Employee_DetailsEmployee () {
                FirstName = textBox_FirstName.Text,
                MiddleName = textBox_MiddleName.Text,
                LastName = textBox_LastName.Text,
                DateBirth = DateTime.Parse (datePicker_DateBirth.Text),
                Gender = comboBox_Gender.SelectedIndex == 1 ? "M" : "F",
                CodeCURP = textBox_CodeCURP.Text,
                CodeRFC = textBox_CodeRFC.Text,
                Email = textBox_Email.Text,
                Password = textBox_Pasword.Text,
                IdRol = comboBox_Rol.SelectedIndex,
                ProfilePhoto = TEMP_profileImageSelected ?? null
            };

            MessageResponse<bool> responseModifyEmployee = await DAO_Employee.RegistryNewEmployee (newDataEmployee);
            if (responseModifyEmployee.IsError) {
                MessageBox.Show ("Ha ocurrido un error inesperado.\nIntente más tarde.", "Error inesperado.");
            } else {
                MessageBox.Show ($"Se ha modificado correctamente al\nEMPLEADO: \"{newDataEmployee.FirstName} {newDataEmployee.MiddleName} {newDataEmployee.LastName}\"", "Modificación completa.");
                this.Close ();
            }
        }

        private void ClickSelectPhoto (object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Filter = "Imagenes (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png;",
                Title = "Seleccionar imagen de perfil",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog () == true) {
                string filePath = openFileDialog.FileName;
                // Mostrar la imagen seleccionada
                image_ImageProfile.Source = new BitmapImage (new Uri (filePath));
                // Convertir la imagen a un arreglo de bytes
                BitmapImage bitmapImage = (BitmapImage)image_ImageProfile.Source;
                byte[] imageBytes;
                using (MemoryStream ms = new MemoryStream ()) {
                    BitmapEncoder encoder = new BmpBitmapEncoder ();
                    encoder.Frames.Add (BitmapFrame.Create (bitmapImage));
                    encoder.Save (ms);
                    imageBytes = ms.ToArray ();
                }
                TEMP_profileImageSelected = imageBytes;
                label_PhotoStatus.Content = "";
            }
        }

        //Stage 1 validations---------------------------------------------------------------------------
        private void TextChanged_FirstName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_FirstName.Text, true, false)) {
                label_ErrorFirstName.Content = "Nombre no válido.";
                label_ErrorFirstName.Visibility = Visibility.Visible;
                _IsCorrectStage1[0] = false;
            } else {
                label_ErrorFirstName.Content = "";
                label_ErrorFirstName.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[0] = true;
            }
        }

        private void TextChanged_MiddleName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_MiddleName.Text, false, false)) {
                label_ErrorMiddleName.Content = "Apellido paterno no válido.";
                label_ErrorMiddleName.Visibility = Visibility.Visible;
                _IsCorrectStage1[1] = false;
            } else {
                label_ErrorMiddleName.Content = "";
                label_ErrorMiddleName.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[1] = true;
            }
        }

        private void TextChanged_LastName (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidWord (textBox_LastName.Text, false, false)) {
                label_ErrorLastName.Content = "Apellido materno no válido.";
                label_ErrorLastName.Visibility = Visibility.Visible;
                _IsCorrectStage1[2] = false;
            } else {
                label_ErrorLastName.Content = "";
                label_ErrorLastName.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[2] = true;
            }
        }

        private void TextChanged_CodeCURP (object sender, TextChangedEventArgs e) {
            TextBox textbox = sender as TextBox;
            if (textbox.Text.Length > 18) {
                textbox.Text = textbox.Text.Substring (0, 18);
            }

            if (!CheckFormat.IsValidCURP (textBox_CodeCURP.Text)) {
                label_ErrorCodeCURP.Content = "CURP no válido.";
                label_ErrorCodeCURP.Visibility = Visibility.Visible;
                _IsCorrectStage1[5] = false;
            } else {
                label_ErrorCodeCURP.Content = "";
                label_ErrorCodeCURP.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[5] = true;
            }
        }

        private void TextChanged_CodeRFC (object sender, TextChangedEventArgs e) {
            TextBox textbox = sender as TextBox;
            if (textbox.Text.Length > 13) {
                textbox.Text = textbox.Text.Substring (0, 13);
            }

            if (!CheckFormat.IsValidRFC (textBox_CodeCURP.Text, textBox_CodeRFC.Text)) {
                label_ErrorCodeRFC.Content = "RFC no válido o no coincide con el CURP.";
                label_ErrorCodeRFC.Visibility = Visibility.Visible;
                _IsCorrectStage1[6] = false;
            } else {
                label_ErrorCodeRFC.Content = "";
                label_ErrorCodeRFC.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[6] = true;
            }
        }

        private void TextChanged_Email (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidEmail (textBox_Email.Text)) {
                label_ErrorEmail.Content = "Correo electrónico no válido.";
                label_ErrorEmail.Visibility = Visibility.Visible;
                _IsCorrectStage1[7] = false;
            } else {
                label_ErrorEmail.Content = "";
                label_ErrorEmail.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[7] = true;
            }
        }

        private void TextChanged_Password (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidPassword (textBox_Pasword.Text)) {
                label_ErrorPasword.Text = "Debe contener mayúsculas, minúsculas, signos y números.";
                label_ErrorPasword.Visibility = Visibility.Visible;
                _IsCorrectStage1[8] = false;
            } else {
                label_ErrorPasword.Text = "";
                label_ErrorPasword.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[8] = true;
            }
        }

        private void TextChanged_PasswordConfirmation (object sender, TextChangedEventArgs e) {
            if (!textBox_PaswordConfirmation.Text.Equals (textBox_Pasword.Text)) {
                label_ErrorPaswordConfirmation.Content = "No coincide.";
                label_ErrorPaswordConfirmation.Visibility = Visibility.Visible;
                _IsCorrectStage1[9] = false;
            } else {
                label_ErrorPaswordConfirmation.Content = "";
                label_ErrorPaswordConfirmation.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[9] = true;
            }
        }

        private void VerifyStage1 () {
            if (string.IsNullOrEmpty (textBox_FirstName.Text)) {
                label_ErrorFirstName.Content = "Campo necesario.";
                label_ErrorFirstName.Visibility = Visibility.Visible;
                _IsCorrectStage1[0] = false;
            }

            if (string.IsNullOrEmpty (textBox_MiddleName.Text)) {
                label_ErrorMiddleName.Content = "Campo necesario.";
                label_ErrorMiddleName.Visibility = Visibility.Visible;
                _IsCorrectStage1[1] = false;
            }

            if (string.IsNullOrEmpty (textBox_LastName.Text)) {
                label_ErrorLastName.Content = "Campo necesario.";
                label_ErrorLastName.Visibility = Visibility.Visible;
                _IsCorrectStage1[2] = false;
            }

            if (datePicker_DateBirth.SelectedDate == null) {
                label_ErrorDateBirth.Content = "Campo necesario.";
                label_ErrorDateBirth.Visibility = Visibility.Visible;
                _IsCorrectStage1[3] = false;
            } else {
                label_ErrorDateBirth.Content = "";
                label_ErrorDateBirth.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[3] = true;
            }

            if (comboBox_Gender.SelectedIndex == 0) {
                label_ErrorGender.Content = "Campo necesario.";
                label_ErrorGender.Visibility = Visibility.Visible;
                _IsCorrectStage1[4] = false;
            } else {
                label_ErrorGender.Content = "";
                label_ErrorGender.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[4] = true;
            }

            if (string.IsNullOrEmpty(textBox_CodeCURP.Text)) {
                label_ErrorCodeCURP.Content = "Campo necesario.";
                label_ErrorCodeCURP.Visibility = Visibility.Visible;
                _IsCorrectStage1[5] = false;
            }

            if (string.IsNullOrEmpty (textBox_CodeRFC.Text)) {
                label_ErrorCodeRFC.Content = "Campo necesario.";
                label_ErrorCodeRFC.Visibility = Visibility.Visible;
                _IsCorrectStage1[6] = false;
            }

            if (string.IsNullOrEmpty (textBox_Email.Text)) {
                label_ErrorEmail.Content = "Campo necesario.";
                label_ErrorEmail.Visibility = Visibility.Visible;
                _IsCorrectStage1[7] = false;
            }

            if (string.IsNullOrEmpty (textBox_Pasword.Text)) {
                label_ErrorPasword.Text = "Campo necesario.";
                label_ErrorPasword.Visibility = Visibility.Visible;
                _IsCorrectStage1[8] = false;
            }

            if (string.IsNullOrEmpty (textBox_PaswordConfirmation.Text)) {
                label_ErrorPaswordConfirmation.Content = "Campo necesario.";
                label_ErrorPaswordConfirmation.Visibility = Visibility.Visible;
                _IsCorrectStage1[9] = false;
            }

            if (comboBox_Rol.SelectedIndex == 0) {
                label_ErrorRol.Content = "Campo necesario.";
                label_ErrorRol.Visibility = Visibility.Visible;
                _IsCorrectStage1[10] = false;
            } else {
                label_ErrorRol.Content = "";
                label_ErrorRol.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[10] = true;
            }
        }
    }
}