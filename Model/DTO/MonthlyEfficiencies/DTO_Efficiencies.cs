using System;

namespace FinanciaRed.Model.DTO.MonthlyEfficiencies {
    internal class DTO_Efficiencies {
        public DateTime DateStart {
            get; set;
        }
        public DateTime DateEnd {
            get; set;
        }
        public int CreditsApplicatedNumb {
            get; set;
        }
        public float CreditsApplicatedPercent {
            get; set;
        }
        public int CreditsAcceptedNumb {
            get; set;
        }
        public float CreditsAcceptedPercent {
            get; set;
        }
        public int CreditsDeclinedNumb {
            get; set;
        }
        public float CreditsDeclinedPercent {
            get; set;
        }
        public int CreditsAwaitingNumb {
            get; set;
        }
        public float CreditsAwaitingPercent {
            get; set;
        }

        public float TotalAmountCredits {
            get; set;
        }
        public float AverageAmountCredit {
            get; set;
        }
        public float MaximumAmountCredit {
            get; set;
        }

        public void AllZero () {
            CreditsApplicatedNumb = 0;
            CreditsApplicatedPercent = 0;
            CreditsAcceptedNumb = 0;
            CreditsAcceptedPercent = 0;
            CreditsDeclinedNumb = 0;
            CreditsDeclinedPercent = 0;
            CreditsAwaitingNumb = 0;
            CreditsAwaitingPercent = 0;
            TotalAmountCredits = 0;
            AverageAmountCredit = 0;
            MaximumAmountCredit = 0;
        }
    }
}
