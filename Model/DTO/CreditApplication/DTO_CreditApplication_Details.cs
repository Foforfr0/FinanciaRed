using System;

namespace FinanciaRed.Model.DTO {
    internal class DTO_CreditApplication_Details {
        public int IdCreditApplication {
            get; set;
        }
        public string NameAdviser {
            get; set;
        }
        public int AmountSolicited {
            get; set;
        }
        public int AmountWithInteres {
            get; set;
        }
        public float InterestRate {
            get; set;
        }
        public int NumberFortNights {
            get; set;
        }
        public DateTime DateSolicited {
            get; set;
        }
        public DateTime DateAccepted {
            get; set;
        }
        public int IdClient {
            get; set;
        }
        public string ClientFirstName {
            get; set;
        }
        public string ClientMiddleName {
            get; set;
        }
        public string ClientLastName {
            get; set;
        }
        public string CodeCURP {
            get; set;
        }
        public string CodeRFC {
            get; set;
        }
        public DateTime ClientDateBirth {
            get; set;
        }
        public string ClientGender {
            get; set;
        }
        public string ClientEmail1 {
            get; set;
        }
        public string ClientEmail2 {
            get; set;
        }
        public string ClientPhoneNumber1 {
            get; set;
        }
        public string ClientPhoneNumber2 {
            get; set;
        }
        public string ClientAddress {
            get; set;
        }
        public string ClientWorkType {
            get; set;
        }
        public string ClientWork {
            get; set;
        }
        public double ClientMonthlySalary {
            get; set;
        }
        public DTO_ClientReference ClientReference1 {
            get; set;
        }
        public DTO_ClientReference ClientReference2 {
            get; set;
        }
        public DTO_BankAccountClient ClientBankAcount1 {
            get; set;
        }
        public DTO_BankAccountClient ClientBankAcount2 {
            get; set;
        }
        public byte[] ProofINE {
            get; set;
        }
        public byte[] ProofLastVoucher {
            get; set;
        }
        public byte[] ProofAddress {
            get; set;
        }
        public string Valoration {
            get; set;
        }
        public string Status {
            get; set;
        }
    }
}
