using System;

namespace FinanciaRed.Model.DTO {
    public class DTO_CreditPolicy_Consult {
        public int IdCreditPolicy {
            get; set;
        }
        public string Name {
            get; set;
        }
        public string Description {
            get; set;
        }
        public DateTime DateStart {
            get; set;
        }
        public DateTime? DateEnd {
            get; set;
        }
        public string DateEndS {
            get; set;
        }
    }
}
