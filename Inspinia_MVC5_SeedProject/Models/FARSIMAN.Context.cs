﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_GMEDINA.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FARSIMANEntities : DbContext
    {
        public FARSIMANEntities()
            : base("name=FARSIMANEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbAccess> tbAccesses { get; set; }
        public virtual DbSet<tbEmployee> tbEmployees { get; set; }
        public virtual DbSet<tbEmployeesSubsidiary> tbEmployeesSubsidiaries { get; set; }
        public virtual DbSet<tbObject> tbObjects { get; set; }
        public virtual DbSet<tbPosition> tbPositions { get; set; }
        public virtual DbSet<tbSubsidiary> tbSubsidiaries { get; set; }
        public virtual DbSet<tbTransporter> tbTransporters { get; set; }
        public virtual DbSet<tbUser> tbUsers { get; set; }
        public virtual DbSet<tbTravel> tbTravelHistories { get; set; }
        public virtual DbSet<tbTravelDetail> tbTravelDetails { get; set; }
        public virtual DbSet<tbTravel> tbTravels { get; set; }
    }
}
