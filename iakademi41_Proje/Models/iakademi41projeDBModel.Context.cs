﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iakademi41_Proje.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class iakademi41proje_DBEntities : DbContext
    {
        public iakademi41proje_DBEntities()
            : base("name=iakademi41proje_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_Categories> tbl_Categories { get; set; }
        public virtual DbSet<tbl_Products> tbl_Products { get; set; }
        public virtual DbSet<tbl_Suppliers> tbl_Suppliers { get; set; }
        public virtual DbSet<tbl_Users> tbl_Users { get; set; }
        public virtual DbSet<tbl_Settings> tbl_Settings { get; set; }
        public virtual DbSet<tbl_Messages> tbl_Messages { get; set; }
        public virtual DbSet<tbl_Orders> tbl_Orders { get; set; }
        public virtual DbSet<vw_orders> vw_orders { get; set; }
        public virtual DbSet<vw_arama> vw_arama { get; set; }
        public virtual DbSet<vw_products> vw_products { get; set; }
        public virtual DbSet<vw_kategoriler> vw_kategoriler { get; set; }
        public virtual DbSet<vw_markalar> vw_markalar { get; set; }
    }
}
