using System;

namespace FinanciaRed.Model.DTO {
    internal class DTO_Credit_Consult {
        public int IdCredit {
            get; set;
        }
        public long Amount {
            get; set;
        }
        public long? AmountLeft {
            get; set;
        }
        public int IdClient {
            get; set;
        }
        public int IdStateCredit {
            get; set;
        }
        public byte[] SignedDocument {
            get; set;
        }
        public double InterestRate {
            get; set;
        }
        public DateTime StartDate {
            get; set;
        }
        public DateTime EndDate {
            get; set;
        }
        public int IdEmployee {
            get; set;
        }
    }
}