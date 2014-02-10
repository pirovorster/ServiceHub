//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceHub.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bid
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int ServiceProviderId { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public bool IsCancelled { get; set; }
    
        public virtual AcceptedBid AcceptedBid { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual Service Service { get; set; }
    }
}
