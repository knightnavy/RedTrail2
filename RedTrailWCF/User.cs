//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RedTrailWCF
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.LoginHistories = new HashSet<LoginHistory>();
        }
    
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public short Gender { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int Age { get; set; }
        public string ContactNo { get; set; }
        public int BloodGroup { get; set; }
        public bool IsDonor { get; set; }
        public int Location { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    
        public virtual Location Location1 { get; set; }
        public virtual ICollection<LoginHistory> LoginHistories { get; set; }
    }
}
