//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kabar_admin
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_category()
        {
            this.tbl_news_cat = new HashSet<tbl_news_cat>();
        }
    
        public int CatPK { get; set; }
        public System.Guid guid { get; set; }
        public string title { get; set; }
        public string info { get; set; }
        public string icon_url { get; set; }
        public Nullable<byte> bdeleted { get; set; }
        public System.DateTime indate { get; set; }
        public Nullable<System.DateTime> edit_date { get; set; }
        public int addedby { get; set; }
        public Nullable<int> editedby { get; set; }
        public bool isCountry { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_news_cat> tbl_news_cat { get; set; }
    }
}