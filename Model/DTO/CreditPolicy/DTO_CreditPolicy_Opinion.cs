namespace FinanciaRed.Model.DTO.CreditPolicy {
    internal class DTO_CreditPolicy_Opinion {
        public int IdCreditApplicationPolicy {
            get; set;
        }
        public int IdCreditApplication {
            get; set;
        }
        public int IdCreditPolicy {
            get; set;
        }
        public string CreditPolicy {
            get; set;
        }
        public bool IsApplied {
            get; set;
        }
    }
}
