﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KhabarDBContext : DbContext
    {
        public KhabarDBContext()
            : base("name=KhabarDBContext")
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 120;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_category> tbl_category { get; set; }
        public virtual DbSet<tbl_frontend_users> tbl_frontend_users { get; set; }
        public virtual DbSet<tbl_news_cat> tbl_news_cat { get; set; }
        public virtual DbSet<tbl_older_news> tbl_older_news { get; set; }
        public virtual DbSet<tbl_source> tbl_source { get; set; }
        public virtual DbSet<tbl_today_news> tbl_today_news { get; set; }
        public virtual DbSet<tbl_kinds> tbl_kinds { get; set; }
        public virtual DbSet<tbl_Registered_Users> tbl_Registered_Users { get; set; }
    }
}
