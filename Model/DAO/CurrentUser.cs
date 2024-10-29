using System;
using System.Data.Entity.Infrastructure.DependencyResolution;

namespace FinanciaRed.Model.DAO {
    public class CurrentUser {
        private static CurrentUser _instance = new CurrentUser();

        public static CurrentUser Instance => _instance;

        private CurrentUser() { }

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
