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
    
    public partial class Rating
    {
        public System.Guid AcceptedBidId { get; set; }
        public int OrderId { get; set; }
        public double Score { get; set; }
        public string Comment { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public System.Guid UserId { get; set; }
    
        public virtual AcceptedBid AcceptedBid { get; set; }
        public virtual User User { get; set; }
    }
}
