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
    /// Interaction logic for ModifyEmployeeData.xaml
    /// </summary>
    public partial class ModifyEmployeeData : Window {
        private DTO_Employee_DetailsEmployee selectedEmployee = null;
        private byte[] TEMP_profileImageSelected = null;
        private bool[] _IsCorrectStage1 = new bool[5] { false, false, false, false, false };

        public ModifyEmployeeData (DTO_Employee_DetailsEmployee selectedEmployee) {
            InitializeComponent ();

            this.selectedEmployee = selectedEmployee;
            _ = LoadVariables ();
        }

        private async Task LoadVariables () {
            comboBox_Rol.Items.Clear ();
            comboBox_Rol.Items.Add (new ComboBoxItem { Content = "Seleccione una opción", IsSelected = true, Tag = 0 });
            MessageResponse<List<DTO_EmployeeRol>> messageResponseER = await DAO_GeneralVariables.GetAllEmployeeRoles ();
            List<DTO_EmployeeRol> listER = messageResponseER.DataRetrieved;
            foreach (DTO_EmployeeRol rol in listER) {
                comboBox_Rol.Items.Add (new ComboBoxItem { Content = rol.Rol });
            }

            await Task.Delay (200);
            ShowEmployeeClient ();
        }

        private void ShowEmployeeClient () {
            textBox_FirstName.Text = selectedEmployee.FirstName;
            textBox_MiddleName.Text = selectedEmployee.MiddleName;
            textBox_LastName.Text = selectedEmployee.LastName;
            textBox_Email.Text = selectedEmployee.Email;
            comboBox_Rol.SelectedIndex = selectedEmployee.IdRol;
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar modificación.",
                MessageBoxButton.YesNo
            );
            if (result == MessageBoxResult.Yes)
                this.Close ();
        }

        private async void ClickFinishModification (object sender, RoutedEventArgs e) {
            VerifyStage1 ();

            if (_IsCorrectStage1.All (x => x == true)) {
                bool exitsEmail;
                if (textBox_Email.Text.Equals (selectedEmployee.Email)) {
                    exitsEmail = false;
                } else {
                    exitsEmail = await DAO_Employee.VerifyExistenceEmail (textBox_Email.Text);
                }
                await Task.Delay (200);

                if (exitsEmail) {
                    label_ErrorEmail.Content = "Correo ya existente en la base de datos.";
                    label_ErrorEmail.Visibility = Visibility.Visible;
                    _IsCorrectStage1[3] = false;
                } else {
                    SaveDataInDatabase ();
                }
            } else {
                MessageBox.Show ("Faltan datos por ingresar o algunos datos están incorrectos.", "Formulario incompleto.");
            }
        }

        private void SaveDataInDatabase () {
            DTO_Employee_DetailsEmployee newDataEmployee = new DTO_Employee_DetailsEmployee {
                IdEmployee = selectedEmployee.IdEmployee,
                FirstName = textBox_FirstName.Text,
                MiddleName = textBox_MiddleName.Text,
                LastName = textBox_LastName.Text,
                Email = textBox_Email.Text,
                IdRol = comboBox_Rol.SelectedIndex,
                ProfilePhoto = TEMP_profileImageSelected
            };

            MessageResponse<bool> responseModifyEmployee = DAO_Employee.SaveChangesDataEmployee (newDataEmployee);
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

        private void TextChanged_Email (object sender, TextChangedEventArgs e) {
            if (!CheckFormat.IsValidEmail (textBox_Email.Text)) {
                label_ErrorEmail.Content = "Apellido materno no válido.";
                label_ErrorEmail.Visibility = Visibility.Visible;
                _IsCorrectStage1[3] = false;
            } else {
                label_ErrorEmail.Content = "";
                label_ErrorEmail.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[3] = true;
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

            if (string.IsNullOrEmpty (textBox_Email.Text)) {
                label_ErrorEmail.Content = "Campo necesario.";
                label_ErrorEmail.Visibility = Visibility.Visible;
                _IsCorrectStage1[3] = false;
            }

            if (comboBox_Rol.SelectedIndex == 0) {
                label_ErrorRol.Content = "Campo necesario.";
                label_ErrorRol.Visibility = Visibility.Visible;
                _IsCorrectStage1[4] = false;
            } else {
                label_ErrorRol.Content = "";
                label_ErrorRol.Visibility = Visibility.Collapsed;
                _IsCorrectStage1[4] = true;
            }
        }
    }
}
