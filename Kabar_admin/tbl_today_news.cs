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
    
    public partial class tbl_today_news
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_today_news()
        {
            this.tbl_news_cat = new HashSet<tbl_news_cat>();
        }
    
        public int NewsPK { get; set; }
        public System.Guid guid { get; set; }
        public int SourceFK { get; set; }
        public string title { get; set; }
        public string sContent { get; set; }
        public string image_url { get; set; }
        public string video_url { get; set; }
        public string external_url { get; set; }
        public int authorisedby { get; set; }
        public System.DateTime indate { get; set; }
        public Nullable<System.DateTime> edit_date { get; set; }
        public int addedby { get; set; }
        public Nullable<int> editedby { get; set; }
        public byte kind { get; set; }
        public Nullable<System.DateTime> pubdate { get; set; }
        public string spubdate { get; set; }
        public bool isArgent { get; set; }
        public long seencount { get; set; }
        public string CatList { get; set; }
        public string kindlist { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_news_cat> tbl_news_cat { get; set; }
        public virtual tbl_source tbl_source { get; set; }
    }
}
