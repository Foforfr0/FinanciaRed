using System;
using System.Globalization;
using System.Windows.Data;

namespace FinanciaRed.Utils {
    public class BoolToNullableConverter : IValueConverter {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture) {
            // Convierte el valor a un bool?
            bool? nullableBool = value as bool?;

            if (nullableBool == null)
                return false; // No seleccionado

            // Convierte el parámetro a un bool
            bool paramBool;
            if (bool.TryParse (parameter as string, out paramBool)) {
                return nullableBool == paramBool;
            }

            return false; // Devuelve falso si no se cumplen las condiciones
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool isChecked && parameter is string paramString) {
                bool paramBool;
                if (bool.TryParse (paramString, out paramBool)) {
                    return isChecked ? (bool?)paramBool : null;
                }
            }
            return null; // Devuelve nulo si no está seleccionado
        }
    }
}