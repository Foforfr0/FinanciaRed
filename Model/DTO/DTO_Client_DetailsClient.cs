using System;

namespace FinanciaRed.Model.DTO {
    public class DTO_Client_DetailsClient {
        public int IdClient {
            get; set;
        }
        public string FirstName {
            get; set;
        }
        public string MiddleName {
            get; set;
        }
        public string LastName {
            get; set;
        }
        public string Gender {
            get; set;
        }
        public DateTime DateBirth {
            get; set;
        }
        public int IdMaritalStatus {
            get; set;
        }
        public string MaritalStatus {
            get; set;
        }
        public string CodeCURP {
            get; set;
        }
        public DTO_AddressClient AddressClient {
            get; set;
        }
        public string Email1 {
            get; set;
        }
        public string Email2 {
            get; set;
        }
        public string PhoneNumber1 {
            get; set;
        }
        public string PhoneNumber2 {
            get; set;
        }
        public DTO_WorkInfo Work {
            get; set;
        }
        public DTO_ReferenceClient Reference1 {
            get; set;
        }
        public DTO_ReferenceClient Reference2 {
            get; set;
        }
        public string CodeRFC {
            get; set;
        }
        public DTO_BankAccountClient BankAccount1 {
            get; set;
        }
        public DTO_BankAccountClient BankAccount2 {
            get; set;
        }
        public int IdStatusClient {
            get; set;
        }
        public string StatusClient {
            get; set;
        }
    }
}