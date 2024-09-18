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
    
    public partial class Clients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clients()
        {
            this.Credits = new HashSet<Credits>();
        }
    
        public int IdClient { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public System.DateTime DateBirth { get; set; }
        public string Gender { get; set; }
        public int IdMaritalStatus { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string CodeRFC { get; set; }
        public string CodeCURP { get; set; }
        public int IdContactReference1 { get; set; }
        public Nullable<int> IdContactReference2 { get; set; }
        public int IdBankAccount1 { get; set; }
        public Nullable<int> IdBankAccount2 { get; set; }
        public int IdAddress { get; set; }
        public int IdWorkArea { get; set; }
    
        public virtual BankAccounts BankAccounts { get; set; }
        public virtual BankAccounts BankAccounts1 { get; set; }
        public virtual ClientsAddresses ClientsAddresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Credits> Credits { get; set; }
        public virtual ContactsReferencesClients ContactsReferencesClients { get; set; }
        public virtual ContactsReferencesClients ContactsReferencesClients1 { get; set; }
        public virtual MaritalStatuses MaritalStatuses { get; set; }
        public virtual WorkAreas WorkAreas { get; set; }
    }
}
