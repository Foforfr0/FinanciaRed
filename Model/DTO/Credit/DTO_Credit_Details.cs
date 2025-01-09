using System;

namespace FinanciaRed.Model.DTO.Credit {
    internal class DTO_Credit_Details {
        public int IdCredit {
            get; set;
        }
        public int AmountLeft {
            get; set;
        }
        public int IdStatus {
            get; set;
        }
        public string Status {
            get; set;
        }
        public DateTime? DateStart {
            get; set;
        }
        public DateTime? DateEnd {
            get; set;
        }
        public byte[] PaymentLayout {
            get; set;
        }
        public byte[] SignedDocument {
            get; set;
        }
        public DTO_CreditApplication_Details CreditApplication {
            get; set;
        }
    }
}
