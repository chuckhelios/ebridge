﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

public partial class ebridgeEntities : DbContext
{
    public ebridgeEntities()
        : base("name=ebridgeEntities")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }

    public virtual DbSet<ACTIVITY_LOG> ACTIVITY_LOG { get; set; }
    public virtual DbSet<AMAZON> AMAZON { get; set; }
    public virtual DbSet<CHAT_NOTICE> CHAT_NOTICE { get; set; }
    public virtual DbSet<CODEBOOK> CODEBOOK { get; set; }
    public virtual DbSet<COUNSELOR> COUNSELOR { get; set; }
    public virtual DbSet<FOLLOWUP_RESPONSE> FOLLOWUP_RESPONSE { get; set; }
    public virtual DbSet<FOLLOWUP_RESPONSE_B> FOLLOWUP_RESPONSE_B { get; set; }
    public virtual DbSet<GRID_REF> GRID_REF { get; set; }
    public virtual DbSet<MESSAGE> MESSAGE { get; set; }
    public virtual DbSet<MESSAGE_ALT> MESSAGE_ALT { get; set; }
    public virtual DbSet<ONLINE_STATUS> ONLINE_STATUS { get; set; }
    public virtual DbSet<PAGE_REF> PAGE_REF { get; set; }
    public virtual DbSet<PARTICIPANT> PARTICIPANT { get; set; }
    public virtual DbSet<QUESTION_REF> QUESTION_REF { get; set; }
    public virtual DbSet<RESOURCE_LINK> RESOURCE_LINK { get; set; }
    public virtual DbSet<RESPONSE_REF> RESPONSE_REF { get; set; }
    public virtual DbSet<RESPONSE_X_QUESTION> RESPONSE_X_QUESTION { get; set; }
    public virtual DbSet<SCHEDULE> SCHEDULE { get; set; }
    public virtual DbSet<SCREENING_RESPONSE> SCREENING_RESPONSE { get; set; }
    public virtual DbSet<SITE> SITE { get; set; }
    public virtual DbSet<STATUS> STATUS { get; set; }
}
