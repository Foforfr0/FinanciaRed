using System;

namespace FinanciaRed.Model.DTO {
    internal class DTO_Credit_Consult {
        public int IdCredit {
            get; set;
        }
        public float AmountTotal {
            get; set;
        }
        public string AmountTotalS {
            get; set;
        }
        public float? AmountLeft {
            get; set;
        }
        public string AmountLeftS {
            get; set;
        }
        public float InterestRate {
            get; set;
        }
        public string InterestPercentaje {
            get; set;
        }
        public int NumberFortnigths {
            get; set;
        }
        public DateTime? DateStart {
            get; set;
        }
        public DateTime? DateEnd {
            get; set;
        }
        public int IdStatus {
            get; set;
        }
        public string Status {
            get; set;
        }
    }
}