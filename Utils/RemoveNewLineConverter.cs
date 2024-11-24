using System.Globalization;
using System.Windows.Data;
using System;

namespace FinanciaRed.Utils {
    public class RemoveNewLineConverter : IValueConverter {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is string text) {
                // Reemplaza saltos de línea por espacios o los elimina según necesites.
                return text.Replace (Environment.NewLine, " ").Replace ("\n", " ").Replace ("\r", " ");
            }
            return value;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }
    }
}
