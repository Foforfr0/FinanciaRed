using System;

namespace FinanciaRed.Model.DTO {
    internal class DTO_Credit_Consult {
        public int IdCredit {
            get; set;
        }
        public string CodeRFC {
            get; set;
        }
        public long AmountTotal {
            get; set;
        }
        public long? AmountLeft {
            get; set;
        }
        public int IdStatusCredit {
            get; set;
        }
        public string StatusCredit {
            get; set;
        }
        public DateTime StartDate {
            get; set;
        }
        public DateTime EndDate {
            get; set;
        }
    }
}