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
    
    public partial class Client
    {
        public Client()
        {
            this.Services = new HashSet<Service>();
        }
    
        public System.Guid Id { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
    
        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
