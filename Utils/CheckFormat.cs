using System.Text.RegularExpressions;

namespace FinanciaRed.Utils {
    internal class CheckFormat {
        public static bool IsValidEmail (string email) {
            if (string.IsNullOrEmpty(email)) return false;

            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            Regex regex = new Regex (pattern);

            return regex.IsMatch (email);
        }

        public static bool IsValidPassword (string password) {
            if (string.IsNullOrEmpty (password)) return false;

            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            Regex regex = new Regex (pattern);

            return regex.IsMatch (password);
        }

        public static string FormatPhoneNumber (string unformattedNumber) {
            string formattedNumber = unformattedNumber.Replace ("-", "");

            if (formattedNumber.Length > 0 && char.IsDigit (formattedNumber[formattedNumber.Length - 1]) == false) {
                formattedNumber = formattedNumber.Remove (formattedNumber.Length - 1);
                return formattedNumber;
            }
            if (formattedNumber.Length > 10) {
                formattedNumber = formattedNumber.Remove (formattedNumber.Length - 1);
                return formattedNumber;
            }

            if (6 < formattedNumber.Length && formattedNumber.Length < 11) {
                formattedNumber = string.Format ("{0}-{1}-{2}",
                                                  formattedNumber.Substring (0, 3),
                                                  formattedNumber.Substring (3, 3),
                                                  formattedNumber.Substring (6));
                return formattedNumber;
            } else if (3 < formattedNumber.Length && formattedNumber.Length < 7) {
                formattedNumber = string.Format ("{0}-{1}",
                                                  formattedNumber.Substring (0, 3),
                                                  formattedNumber.Substring (3));
                return formattedNumber;
            } else if (0 < formattedNumber.Length && formattedNumber.Length < 4) {
                formattedNumber = string.Format ("{0}",
                                                  formattedNumber.Substring (0));
                return formattedNumber;
            }
            return formattedNumber;
        }
    }
}
