//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FetchService
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_frontend_users
    {
        public int FUserPK { get; set; }
        public System.Guid guid { get; set; }
        public string displayname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public Nullable<byte> Kind { get; set; }
        public bool viewsources { get; set; }
        public bool addsources { get; set; }
        public bool editsources { get; set; }
        public bool delsources { get; set; }
        public bool viewnews { get; set; }
        public bool addnews { get; set; }
        public bool editnews { get; set; }
        public bool delnews { get; set; }
        public bool viewcats { get; set; }
        public bool addcats { get; set; }
        public bool editcats { get; set; }
        public bool delcats { get; set; }
        public bool viewclassification { get; set; }
        public bool addclassification { get; set; }
        public bool editclassification { get; set; }
        public bool delclassification { get; set; }
        public bool viewusers { get; set; }
        public bool addusers { get; set; }
        public bool editusers { get; set; }
        public bool delusers { get; set; }
        public System.DateTime indate { get; set; }
        public Nullable<System.DateTime> edit_date { get; set; }
        public int addedby { get; set; }
        public Nullable<int> editedby { get; set; }
        public System.Guid verifcationcode { get; set; }
        public string following_sources { get; set; }
        public bool isAdmin { get; set; }
    }
}
