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
    
    public partial class WorkAreas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkAreas()
        {
            this.Clients = new HashSet<Clients>();
        }
    
        public int IdWorkArea { get; set; }
        public string WorkArea { get; set; }
        public int IdWorkAreaType { get; set; }
        public double MonthlySalary { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clients> Clients { get; set; }
        public virtual WorkAreaTypes WorkAreaTypes { get; set; }
    }
}
