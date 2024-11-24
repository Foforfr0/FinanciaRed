using System;

namespace FinanciaRed.Model.DTO {
    internal class DTO_Credit_Consult {
        public int IdCredit {
            get; set;
        }
        public long AmountTotal {
            get; set;
        }
        public long? AmountLeft {
            get; set;
        }
        public double InteresRate {
            get; set;
        }
        public int NumberFortnigths {
            get; set;
        }
        public DateTime DateStart {
            get; set;
        }
        public DateTime DateEnd {
            get; set;
        }
    }
}