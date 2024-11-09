namespace FinanciaRed.Model.DAO {
    public class CURRENT_USER {
        private static CURRENT_USER _instance = new CURRENT_USER();

        public static CURRENT_USER Instance => _instance;

        private CURRENT_USER() { }

        public int IdEmployee {
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
        public int IdRol {
            get; set;
        }
        public string Rol {
            get; set;
        }
        public string Email {
            get; set;
        }
        public byte[] ProfilePhoto {
            get; set;
        }
    }
}
