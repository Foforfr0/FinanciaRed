using System;

namespace FinanciaRed.Model.DTO.Client {
    internal class DTO_CreditApplication_Create {
        public int IdClient {
            get; set;
        }
        public int IdPromotion {
            get; set;
        }
        public float InterestRate {
            get; set;
        }
        public int NumberFortNigths {
            get; set;
        }
        public int AmountSolicited {
            get; set;
        }
        public byte[] ProofINE {
            get; set;
        }
        public byte[] ProofAddress {
            get; set;
        }
        public byte[] ProofLastPayStub {
            get; set;
        }
        public DateTime DateApplication {
            get; set;
        }
        public int IdEmployee {
            get; set;
        }
    }
}
