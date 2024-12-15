using System;

namespace FinanciaRed.Model.DTO {
    internal class DTO_CreditApplication_Consult {
        public int IdCreditApplication {
            get; set;
        }
        public int AmountTotal {
            get; set;
        }
        public string AmountTotalS {
            get; set;
        }
        public float InteresRate {
            get; set;
        }
        public string InterestPercentaje {
            get; set;
        }
        public int NumberFortNights {
            get; set;
        }
        public DateTime DateRequest {
            get; set;
        }
        public int IdStatus {
            get; set;
        }
        public string Status {
            get; set;
        }
        public string ClientName {
            get; set;
        }
    }
}
