using System.Text.RegularExpressions;

namespace FinanciaRed.Utils {
    internal class CheckFormat {
        public static bool IsValidEmail (string email) {
            if (string.IsNullOrEmpty (email))
                return false;

            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            Regex regex = new Regex (pattern);

            return regex.IsMatch (email);
        }

        public static bool IsValidPhoneNumber (string phoneNumber) {
            if (string.IsNullOrEmpty (phoneNumber))
                return false;

            string pattern = @"^\d{3}-\d{3}-\d{4}$";
            Regex regex = new Regex (pattern);

            return regex.IsMatch (phoneNumber);
        }

        public static bool IsValidPassword (string password) {
            if (string.IsNullOrEmpty (password))
                return false;

            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            Regex regex = new Regex (pattern);

            return regex.IsMatch (password);
        }

        public static bool IsValidNamePerson (string name) {
            if (string.IsNullOrEmpty (name))
                return false;

            string pattern = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ'’-]+(?:[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ'’-]+)*$";
            Regex regex = new Regex (pattern);

            return regex.IsMatch (name);
        }

        public static bool IsValidCURP (string curp) {
            if (string.IsNullOrEmpty (curp))
                return false;

            string pattern = @"^[A-Z]{4}[0-9]{6}[HM]{1}(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[A-Z]{4}[0-9]{1}$";
            Regex regex = new Regex (pattern);

            return regex.IsMatch (curp);
        }

        public static bool IsValidRFC (string curp, string rfc) {
            if (string.IsNullOrEmpty (rfc) || string.IsNullOrEmpty (curp))
                return false;

            string curpSubString = curp.Substring (0, 10);
            string rfcPattern = @"^"+curpSubString+"[A-Z,0-9]{3}$";
            Regex rfcRegex = new Regex (rfcPattern);

            return rfcRegex.IsMatch (rfc);
        }

        public static bool IsValidCLABE (string clabe) {
            if (string.IsNullOrEmpty (clabe))
                return false;

            string pattern = @"^\d{3}-\d{3}-\d{3}-\d{3}-\d{3}-\d{3}$";
            Regex regex = new Regex (pattern);

            return regex.IsMatch (clabe);
        }

        public static bool IsValidCardNumber (string cardNumber) {
            if (string.IsNullOrEmpty (cardNumber))
                return false;

            string patternVisa = @"^4[0-9]{3}-[0-9]{4}-[0-9]{4}-[0-9]{1}(?:[0-9]{3})?$";
            //TODO VALITDATE FROM WHERE IS THE CARD NUMBER
            /*string patternMasterCard = @"^5[1-5][0-9]{14}$";
            string patternAmericanExpress = @"^3[47][0-9]{13}$";
            string patternDinnersClub = @"^30[0-5][0-9]{11}$";
            string patternDiscover = @"^6(?:011|5[0-9]{2})[0-9]{12}$";
            string patternJCB = @"^(?:2131|1800|35\d{3})\d{11}$";*/
            
            Regex regex = new Regex (patternVisa);

            return regex.IsMatch (cardNumber);
        }

        public static bool IsValidPostalCode (string postalCode) {
            if (string.IsNullOrEmpty (postalCode))
                return false;

            string pattern = @"^\d{5}$";
            Regex regex = new Regex (pattern);

            return regex.IsMatch (postalCode);
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
