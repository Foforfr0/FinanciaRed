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
    
    public partial class Credits
    {
        public int IdCredit { get; set; }
        public long Amount { get; set; }
        public Nullable<long> AmountLeft { get; set; }
        public int IdClient { get; set; }
        public int IdStateCredit { get; set; }
        public byte[] SignedDocument { get; set; }
        public double InterestRate { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int IdEmployee { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual Employees Employees { get; set; }
        public virtual StatesCredits StatesCredits { get; set; }
    }
}
