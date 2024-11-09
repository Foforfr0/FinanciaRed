//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinanciaRed.Model.Model_Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employees
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employees()
        {
            this.CreditApplications = new HashSet<CreditApplications>();
        }
    
        public int IdEmployee { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public System.DateTime DateBirth { get; set; }
        public string Gender { get; set; }
        public string CodeCURP { get; set; }
        public string CodeRFC { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdRole { get; set; }
        public int IdStatusEmployee { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditApplications> CreditApplications { get; set; }
        public virtual RolesEmployees RolesEmployees { get; set; }
        public virtual StatusesEmployee StatusesEmployee { get; set; }
    }
}
