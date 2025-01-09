using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Utils;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FinanciaRed.View.ManageEmployees {
    /// <summary>
    /// Interaction logic for CheckProfile.xaml
    /// </summary>
    public partial class CheckProfile : Page {
        private DTO_Employee_Details currentEmployee = null;
        private byte[] TEMP_profileImageSelected = null;

        public CheckProfile (int idEmployee) {
            InitializeComponent ();

            _ = RetrieveDetailsAccount (idEmployee, false);
        }

        private async Task RetrieveDetailsAccount (int idEmployee, bool withPassword) {
            MessageResponse<DTO_Employee_Details> messageResponseDetailsAccountEmployee =
                await DAO_Employee.GetAsync (idEmployee, withPassword);

            if (!messageResponseDetailsAccountEmployee.IsError) {
                currentEmployee = messageResponseDetailsAccountEmployee.DataRetrieved;
                textBox_FirstName.Text = currentEmployee.FirstName;
                textBox_MiddleName.Text = currentEmployee.MiddleName;
                textBox_LastName.Text = currentEmployee.LastName;
                textBox_Email.Text = currentEmployee.Email;
                textBox_Password.Text = withPassword ? currentEmployee.Password : "***********";
                textBox_Rol.Text = currentEmployee.Rol;
                if (currentEmployee.ProfilePhoto == null) {
                    image_ImageProfile.Source = new BitmapImage (new Uri ("../Images/icon-user.png", UriKind.Relative));
                    label_PhotoStatus.Content = "Sin foto";
                } else {
                    image_ImageProfile.Source = Converters.ConvertByteToBitmapImage (currentEmployee.ProfilePhoto);
                    label_PhotoStatus.Content = "";
                }
            } else {
                MessageBox.Show (
                    "No se pudo obtener los datos.", 
                    "Error inesperado",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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

        private async void ClickModifyAccount (object sender, RoutedEventArgs e) {
            button_Modify.Visibility = Visibility.Collapsed;
            button_UploadImage.Visibility = Visibility.Visible;
            button_Accept.Visibility = Visibility.Visible;
            button_Cancel.Visibility = Visibility.Visible;

            label_PasswordConfirmation.Visibility = Visibility.Visible;
            textBox_PasswordConfirmation.Visibility = Visibility.Visible;

            await RetrieveDetailsAccount (currentEmployee.IdEmployee, true);

            textBox_Password.Text = "";

            textBox_Email.IsReadOnly = false;
            textBox_Password.IsReadOnly = false;
            textBox_PasswordConfirmation.IsReadOnly = false;
            textBox_Email.BorderBrush = new SolidColorBrush (Colors.LightGray);
            textBox_Password.BorderBrush = new SolidColorBrush (Colors.LightGray);
            textBox_PasswordConfirmation.BorderBrush = new SolidColorBrush (Colors.LightGray);
        }

        private async void ClickAcceptModification (object sender, RoutedEventArgs e) {
            if (VerifyForm ()) {
                DTO_Employee_Details newDataEmployee = new DTO_Employee_Details {
                    IdEmployee = currentEmployee.IdEmployee,
                    FirstName = currentEmployee.FirstName,
                    MiddleName = currentEmployee.MiddleName,
                    LastName = currentEmployee.LastName,
                    Gender = currentEmployee.Gender,
                    CodeCURP = currentEmployee.CodeCURP,
                    CodeRFC = currentEmployee.CodeRFC,
                    Email = textBox_Email.Text,
                    Password = textBox_PasswordConfirmation.Text,
                    IdRol = currentEmployee.IdRol,
                    Rol = currentEmployee.Rol,
                    ProfilePhoto = TEMP_profileImageSelected
                };

                MessageResponse<bool> responseModify;
                if (string.IsNullOrEmpty (textBox_Password.Text) && string.IsNullOrEmpty (textBox_PasswordConfirmation.Text)) {
                    responseModify = DAO_Employee.PutAsync (newDataEmployee, false);
                } else {
                    responseModify = DAO_Employee.PutAsync (newDataEmployee, true);
                }

                if (responseModify.IsError) {
                    MessageBox.Show (
                        "Error al guardar los cambios, intente más tarde.",
                        "Error inesperado",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                } else {
                    CURRENT_USER.Instance.ProfilePhoto = TEMP_profileImageSelected;
                    Frame parentFrame = Utils.GetElementVisualTree.GetParent<Frame> (this);
                    MainWindow mainWindow = Utils.GetElementVisualTree.GetParent<Page> (parentFrame) as MainWindow;
                    mainWindow.InitializeDataEmployee ();

                    await SetFormNoEditable ();
                }
            }
        }

        private async void ClickCancelModification (object sender, RoutedEventArgs e) {
            await SetFormNoEditable ();
        }

        private async Task SetFormNoEditable () {
            button_Modify.Visibility = Visibility.Visible;
            button_UploadImage.Visibility = Visibility.Collapsed;
            button_Accept.Visibility = Visibility.Collapsed;
            button_Cancel.Visibility = Visibility.Collapsed;

            label_PasswordConfirmation.Visibility = Visibility.Collapsed;
            textBox_PasswordConfirmation.Visibility = Visibility.Collapsed;

            await RetrieveDetailsAccount (currentEmployee.IdEmployee, false);

            textBox_Email.IsReadOnly = true;
            textBox_Password.IsReadOnly = true;
            textBox_PasswordConfirmation.IsReadOnly = true;
            textBox_Email.BorderBrush = new SolidColorBrush (Colors.Transparent);
            textBox_Password.BorderBrush = new SolidColorBrush (Colors.Transparent);
            textBox_PasswordConfirmation.BorderBrush = new SolidColorBrush (Colors.Transparent);
            if (currentEmployee.ProfilePhoto == null) {
                image_ImageProfile.Source = new BitmapImage (new Uri ("../Images/icon-user.png", UriKind.Relative));
                label_PhotoStatus.Content = "Sin foto";
            } else {
                image_ImageProfile.Source = Converters.ConvertByteToBitmapImage (currentEmployee.ProfilePhoto);
                label_PhotoStatus.Content = "";
            }
            label_PhotoStatus.Content = currentEmployee.ProfilePhoto == null ? "Sin foto" : "";
        }

        private bool VerifyForm () {
            bool isFormCorrect = true;

            if (CheckFormat.IsValidEmail (textBox_Email.Text) == false) {
                isFormCorrect = false;
                if (ManageLabelsError.ExistsLabelInStack (stackPanel_Form, "errorEmailLabel") == false) {
                    stackPanel_Form.Children.Insert (
                        stackPanel_Form.Children.IndexOf (textBox_Email) + 1,
                        ManageLabelsError.CreateNewLabel ("errorEmailLabel", "Ingrese su correo.", 11, 0)
                    );
                }
            } else {
                if (ManageLabelsError.ExistsLabelInStack (stackPanel_Form, "errorEmailLabel")) {
                    ManageLabelsError.RemoveLabel (stackPanel_Form, "errorEmailLabel");
                }
            }
            if (string.IsNullOrEmpty (textBox_Password.Text) && string.IsNullOrEmpty (textBox_PasswordConfirmation.Text)) {

            } else {
                if (CheckFormat.IsValidPassword (textBox_Password.Text) == false) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_Form, "errorPasswordLabel") == false) {
                        stackPanel_Form.Children.Insert (
                            stackPanel_Form.Children.IndexOf (textBox_Password) + 1,
                            ManageLabelsError.CreateNewLabel ("errorPasswordLabel", "8 caracteres, incluya mayúsculas, minúsculas, números y signos.", 11, 0)
                        );
                    }
                } else {
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_Form, "errorPasswordLabel")) {
                        ManageLabelsError.RemoveLabel (stackPanel_Form, "errorPasswordLabel");
                    }
                }
                if (textBox_Password.Text.Equals (textBox_PasswordConfirmation.Text) == false) {
                    isFormCorrect = false;
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_Form, "errorPasswordConfirmationLabel") == false) {
                        stackPanel_Form.Children.Insert (
                            stackPanel_Form.Children.IndexOf (textBox_PasswordConfirmation) + 1,
                            ManageLabelsError.CreateNewLabel ("errorPasswordConfirmationLabel", "Confirmación de contraseña incorrecta.", 11, 0)
                        );
                    }
                } else {
                    if (ManageLabelsError.ExistsLabelInStack (stackPanel_Form, "errorPasswordConfirmationLabel")) {
                        ManageLabelsError.RemoveLabel (stackPanel_Form, "errorPasswordConfirmationLabel");
                    }
                }
            }

            return isFormCorrect;
        }
    }
}