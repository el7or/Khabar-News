//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Khabar_Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Registered_Users
    {
        public int FUserPK { get; set; }
        public System.Guid guid { get; set; }
        public string displayname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string following_sources { get; set; }
        public Nullable<byte> Kind { get; set; }
        public System.DateTime indate { get; set; }
        public Nullable<System.DateTime> edit_date { get; set; }
        public int addedby { get; set; }
        public Nullable<int> editedby { get; set; }
        public System.Guid verifcationcode { get; set; }
    }
}
