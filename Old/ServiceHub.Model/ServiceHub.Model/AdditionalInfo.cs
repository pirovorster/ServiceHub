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
    
    public partial class AdditionalInfo
    {
        public System.Guid Id { get; set; }
        public int OrderId { get; set; }
        public System.Guid ServiceId { get; set; }
        public string Comment { get; set; }
        public System.DateTime TimeStamp { get; set; }
    
        public virtual Service Service { get; set; }
    }
}
