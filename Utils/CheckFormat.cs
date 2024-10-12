using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace FinanciaRed.Utils {
    internal class CheckFormat {
        public static bool IsValidEmail (string email) {
            if (string.IsNullOrEmpty (email))
                return false;

            Regex regex = new Regex (@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            return regex.IsMatch (email);
        }

        public static bool IsValidPhoneNumber (string phoneNumber) {
            if (string.IsNullOrEmpty (phoneNumber))
                return false;

            Regex regex = new Regex (@"^\d{3}-\d{3}-\d{4}$");

            return regex.IsMatch (phoneNumber);
        }

        public static bool IsValidPassword (string password) {
            if (string.IsNullOrEmpty (password))
                return false;

            Regex regex = new Regex (@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

            return regex.IsMatch (password);
        }

        public static bool IsValidWord (string word, bool withSpaces, bool withNumbers) {
            if (string.IsNullOrEmpty (word))
                return false;
            Regex regex = new Regex (@"^$");
            if (withSpaces && !withNumbers) {
                regex = new Regex (@"^(?!.*\s{2,})([A-ZÁÉÍÚÓÑ][a-záéíóúñ]*(?:\s[A-ZÁÉÍÚÓÑ][a-záéíóúñ]*)*)$");
            }
            if (withSpaces && withNumbers) {
                regex = new Regex (@"^(?!.*\s{2,})([A-ZÁÉÍÚÓÑ0-9][a-záéíóúñ0-9]*(?:\s[A-ZÁÉÍÚÓÑ0-9][a-záéíóúñ0-9]*)*)$$");
            }
            if (!withSpaces && !withNumbers) {
                regex = new Regex (@"^[A-ZÁÉÍÚÓÑ][a-záéíóúñ0-9]*$");
            }

            return regex.IsMatch (word);
        }

        public static bool IsValidCURP (string curp) {
            if (string.IsNullOrEmpty (curp))
                return false;

            Regex regex = new Regex (@"^[A-Z]{4}[0-9]{6}[HM]{1}(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[A-Z]{4}[0-9]{1}$");

            return regex.IsMatch (curp);
        }

        public static bool IsValidRFC (string curp, string rfc) {
            if (string.IsNullOrEmpty (rfc) || string.IsNullOrEmpty (curp))
                return false;

            string subStringCURP = curp.Substring (0, 10);
            Regex rfcRegex = new Regex (@"^(" + subStringCURP + ")[A-Z0-9]{3}$");

            return rfcRegex.IsMatch (rfc);
        }

        public static bool IsValidCLABE (string clabe) {
            if (string.IsNullOrEmpty (clabe))
                return false;
            ;
            Regex regex = new Regex (@"^\d{3}-\d{3}-\d{3}-\d{3}-\d{3}-\d{3}$");

            return regex.IsMatch (clabe);
        }

        public static bool IsValidCardNumber (string cardNumber) {
            if (string.IsNullOrEmpty (cardNumber))
                return false;

            string pattern = @"^[0-9]{4}-[0-9]{4}-[0-9]{4}-[0-9]{4}$";
            //TODO VALITDATE FROM WHERE IS THE CARD NUMBER
            /* string patternVisa = @"^4[0-9]{3}-[0-9]{4}-[0-9]{4}-[0-9]{1}(?:[0-9]{3})?$";
             * string patternMasterCard = @"^5[1-5][0-9]{14}$";
             * string patternAmericanExpress = @"^3[47][0-9]{13}$";
             * string patternDinnersClub = @"^30[0-5][0-9]{11}$";
             * string patternDiscover = @"^6(?:011|5[0-9]{2})[0-9]{12}$";
             * string patternJCB = @"^(?:2131|1800|35\d{3})\d{11}$";*/

            Regex regex = new Regex (pattern);

            return regex.IsMatch (cardNumber);
        }

        public static bool IsValidPostalCode (string postalCode) {
            if (string.IsNullOrEmpty (postalCode))
                return false;

            Regex regex = new Regex (@"^\d{5}$");

            return regex.IsMatch (postalCode);
        }

        public static string SeparateNumberByGroups (string unformattedNumber, int maxLength, int groupBy) {
            // Eliminar cualquier guion existente
            string formattedNumber = unformattedNumber.Replace ("-", "");
            int lengthFormattedNumber = formattedNumber.Length;

            // Eliminar último carácter si no es un dígito
            if (formattedNumber.Length > 0 && !char.IsDigit (formattedNumber[lengthFormattedNumber - 1])) {
                formattedNumber = formattedNumber.Remove (lengthFormattedNumber - 1);
            }

            //No deja ingresar más de 18 caracteres
            if (formattedNumber.Length > maxLength) {
                formattedNumber = formattedNumber.Remove (lengthFormattedNumber - 1);
                return formattedNumber;
            }

            // Construir el nuevo número formateado con guiones cada 3 caracteres
            StringBuilder sb = new StringBuilder ();
            for (int i = 0; i < lengthFormattedNumber; i++) {
                // Agregar un guion cada 3 caracteres, pero no al inicio
                if (i > 0 && i % groupBy == 0) {
                    sb.Append ("-");
                }
                sb.Append (formattedNumber[i]);
            }

            return sb.ToString ();

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

            //012-345-6789
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
