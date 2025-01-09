using FinanciaRed.Model.DAO;
using FinanciaRed.Model.DTO;
using FinanciaRed.Model.DTO.CreditPromotion;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinanciaRed.View.ManageCreditPromotions {
    /// <summary>
    /// Interaction logic for RegisterCreditPromotion.xaml
    /// </summary>
    public partial class RegisterCreditPromotion : Window {
        public RegisterCreditPromotion () {
            InitializeComponent ();
        }

        private void ClickCancel (object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show (
                "¿Está seguro de cancelar?\nNo se podrán recuperar los datos.",
                "Cancelar modificación.",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                this.Close ();
        }

        private async void ClickFinishRegistration (object sender, RoutedEventArgs e) {
            if (ValidateForm ()) {
                await SaveDataDatabase ();
            }
        }

        private bool ValidateForm () {
            bool isCorrect = true;

            if (string.IsNullOrEmpty (textBox_Name.Text)) {
                label_ErrorName.Content = "Campo necesario.";
                label_ErrorName.Visibility = Visibility.Visible;
                isCorrect = false;
            } else if (textBox_Name.Text.Length <= 5) {
                label_ErrorName.Content = "Nombre muy corto.";
                label_ErrorName.Visibility = Visibility.Visible;
                isCorrect = false;
            } else {
                label_ErrorName.Content = "";
                label_ErrorName.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty (textBox_NumberFortNigths.Text)) {
                label_ErrorNumberFortNigths.Content = "Campo necesario.";
                label_ErrorNumberFortNigths.Visibility = Visibility.Visible;
                isCorrect = false;
            } else if (!int.TryParse (textBox_NumberFortNigths.Text, out int result)) {
                label_ErrorNumberFortNigths.Content = "Número de quincena no válido.";
                label_ErrorNumberFortNigths.Visibility = Visibility.Visible;
                isCorrect = false;
            } else {
                label_ErrorNumberFortNigths.Content = "";
                label_ErrorNumberFortNigths.Visibility = Visibility.Collapsed;
            }
            if (string.IsNullOrEmpty (textBox_InterestRate.Text)) {
                label_ErrorNumberFortNigths.Content = "Campo necesario.";
                label_ErrorNumberFortNigths.Visibility = Visibility.Visible;
                isCorrect = false;
            } else {
                label_ErrorNumberFortNigths.Content = "";
                label_ErrorNumberFortNigths.Visibility = Visibility.Collapsed;
            }
            if (datePicker_DateStart.SelectedDate.Value == null) {
                label_ErrorDateStart.Content = "Campo necesario.";
                label_ErrorDateStart.Visibility = Visibility.Visible;
                isCorrect = false;
            } else if (datePicker_DateStart.SelectedDate.Value > datePicker_DateEnd.SelectedDate.Value) {
                label_ErrorDateStart.Content = "La fecha de inicio debe ser antes que la fecha de fin.";
                label_ErrorDateStart.Visibility = Visibility.Visible;
                isCorrect = false;
            } else {
                label_ErrorDateStart.Content = "";
                label_ErrorDateStart.Visibility = Visibility.Collapsed;
            }
            if (datePicker_DateEnd.SelectedDate.Value == null) {
                label_ErrorDateEnd.Content = "Campo necesario.";
                label_ErrorDateEnd.Visibility = Visibility.Visible;
                isCorrect = false;
            } else if (datePicker_DateEnd.SelectedDate.Value > datePicker_DateEnd.SelectedDate.Value) {
                label_ErrorDateEnd.Content = "La fecha de fin debe ser después que la fecha de fin.";
                label_ErrorDateEnd.Visibility = Visibility.Visible;
                isCorrect = false;
            } else {
                label_ErrorDateEnd.Content = "";
                label_ErrorDateEnd.Visibility = Visibility.Collapsed;
            }

            return isCorrect;
        }

        private async Task SaveDataDatabase () {
            DTO_CreditPromotion_Details newCreditProm = new DTO_CreditPromotion_Details {
                Name = textBox_Name.Text,
                NumberFortNigths = int.Parse (textBox_NumberFortNigths.Text),
                InterestRate = float.Parse (textBox_InterestRate.Text),
                DateStart = datePicker_DateStart.SelectedDate.Value,
                DateEnd = datePicker_DateEnd.SelectedDate.Value
            };

            MessageResponse<bool> reponseRegistryProm = await DAO_CreditPromotion.PostAsync (newCreditProm);
            if (reponseRegistryProm.IsError) {
                MessageBox.Show (
                    "No se logró guardar los cambios realizados.", 
                    "Error inesperado.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
                MessageBox.Show (
                    $"Se ha registrado correctamente la\nPromoción: \"{newCreditProm.Name}\"", 
                    "Modificación completa.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close ();
            }
        }

        // Number fortnigths
        private void NumericTextBox_PreviewTextInput (object sender, TextCompositionEventArgs e) {
            OnPreviewTextInput (e);
        }

        private void NumericTextBox_Pasting (object sender, DataObjectPastingEventArgs e) {
            OnPasting (e);
        }

        public static bool IsTextAllowed (string text) {
            return !string.IsNullOrEmpty (text) && text.All (char.IsDigit);
        }

        public static void OnPreviewTextInput (TextCompositionEventArgs e) {
            e.Handled = !IsTextAllowed (e.Text);
        }

        public static void OnPasting (DataObjectPastingEventArgs e) {
            if (e.DataObject.GetDataPresent (typeof (string))) {
                string text = (string)e.DataObject.GetData (typeof (string));
                if (!IsTextAllowed (text)) {
                    e.CancelCommand ();
                }
            } else {
                e.CancelCommand ();
            }
        }

        // Interest rate
        public static bool IsTextAllowed (string text, string currentText, int caretIndex) {
            // Si el texto está vacío, permitir
            if (string.IsNullOrEmpty (text))
                return true;

            // Verificar si cada caracter es un dígito o un punto decimal
            foreach (char c in text) {
                if (!char.IsDigit (c) && c != '.')
                    return false;
            }

            // Construir el texto resultante para validación
            string proposedText = currentText.Insert (caretIndex, text);

            // No permitir múltiples puntos decimales
            if (proposedText.Count (c => c == '.') > 1)
                return false;

            // Verificar si el resultado es un número decimal válido
            return decimal.TryParse (proposedText, out _);
        }

        public static void OnPreviewTextInput (TextCompositionEventArgs e, TextBox textBox) {
            e.Handled = !IsTextAllowed (e.Text, textBox.Text, textBox.CaretIndex);
        }

        public static void OnPasting (DataObjectPastingEventArgs e, TextBox textBox) {
            if (e.DataObject.GetDataPresent (typeof (string))) {
                string text = (string)e.DataObject.GetData (typeof (string));
                if (!IsTextAllowed (text, textBox.Text, textBox.CaretIndex)) {
                    e.CancelCommand ();
                }
            } else {
                e.CancelCommand ();
            }
        }

        private void DecimalTextBox_PreviewTextInput (object sender, TextCompositionEventArgs e) {
            OnPreviewTextInput (e, sender as TextBox);
        }

        private void DecimalTextBox_Pasting (object sender, DataObjectPastingEventArgs e) {
            OnPasting (e, sender as TextBox);
        }

        // Opcional: Para prevenir espacios
        private void DecimalTextBox_PreviewKeyDown (object sender, KeyEventArgs e) {
            if (e.Key == Key.Space) {
                e.Handled = true;
            }
        }
    }
}
