﻿namespace FinanciaRed.Model.DTO {
    internal class DTO_AddressClient {
        public int IdAddressClient {
            get; set;
        }
        public string ExteriorNumber {
            get; set;
        }
        public string InteriorNumber {
            get; set;
        }
        public string Street {
            get; set;
        }
        public string Colony {
            get; set;
        }
        public string PostalCode {
            get; set;
        }
        public string State {
            get; set;
        }
        public int IdAddressType {
            get; set;
        }
        public string AddressType {
            get; set;
        }
    }
}