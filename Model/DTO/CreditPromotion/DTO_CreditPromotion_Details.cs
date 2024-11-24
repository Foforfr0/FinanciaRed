using System;

namespace FinanciaRed.Model.DTO.CreditPromotion {
    public class DTO_CreditPromotion_Details {
        public int IdCreditPromotion {
            get; set;
        }
        public string Name {
            get; set;
        }
        public float InterestRate {
            get; set;
        }
        public int NumberFortNigths {
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
