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
    
    public partial class ClientsAddresses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClientsAddresses()
        {
            this.Clients = new HashSet<Clients>();
        }
    
        public int IdClientAddress { get; set; }
        public string ExteriorNumber { get; set; }
        public string InteriorNumber { get; set; }
        public string Street { get; set; }
        public string Colony { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public int IdAddressType { get; set; }
    
        public virtual AddressesTypes AddressesTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clients> Clients { get; set; }
    }
}
