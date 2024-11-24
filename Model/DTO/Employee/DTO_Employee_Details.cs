using System;

namespace FinanciaRed.Model.DTO {
    public class DTO_Employee_Details {
        public int IdEmployee {
            get; set;
        }
        public byte[] ProfilePhoto {
            get; set;
        }
        public string FirstName {
            get; set;
        }
        public string MiddleName {
            get; set;
        }
        public string LastName {
            get; set;
        }
        public DateTime DateBirth {
            get; set;
        }
        public string Gender {
            get; set;
        }
        public string CodeCURP {
            get; set;
        }
        public string CodeRFC {
            get; set;
        }
        public string Email {
            get; set;
        }
        public string Password {
            get; set;
        }
        public int IdRol {
            get; set;
        }
        public string Rol {
            get; set;
        }
    }
}
