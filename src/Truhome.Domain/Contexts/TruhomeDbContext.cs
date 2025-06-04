using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Truhome.Domain.Entities;

namespace Truhome.Domain.Contexts;

public partial class TruhomeDbContext : DbContext
{
    public TruhomeDbContext()
    {
    }

    public TruhomeDbContext(DbContextOptions<TruhomeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customeraudit> Customeraudits { get; set; }

    public virtual DbSet<Customermapping> Customermappings { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_address_id");

            entity.ToTable("address");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Addline1)
                .HasMaxLength(200)
                .HasColumnName("addline1");
            entity.Property(e => e.Addline2)
                .HasMaxLength(200)
                .HasColumnName("addline2");
            entity.Property(e => e.Addresstype)
                .HasMaxLength(20)
                .HasColumnName("addresstype");
            entity.Property(e => e.AreaOfLocality)
                .HasMaxLength(50)
                .HasColumnName("areaOfLocality");
            entity.Property(e => e.City)
                .HasMaxLength(25)
                .HasColumnName("city");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Landmark)
                .HasMaxLength(50)
                .HasColumnName("landmark");
            entity.Property(e => e.Pincode).HasColumnName("pincode");
            entity.Property(e => e.State)
                .HasMaxLength(25)
                .HasColumnName("state");
            entity.Property(e => e.Unitno)
                .HasMaxLength(50)
                .HasColumnName("unitno");

            entity.HasOne(d => d.Customer).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("fk_customer_id");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_customer_id");

            entity.ToTable("customer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Aadharnumber)
                .HasMaxLength(20)
                .HasColumnName("aadharnumber");
            entity.Property(e => e.Alternatemobilenumber).HasColumnName("alternatemobilenumber");
            entity.Property(e => e.Cin)
                .HasMaxLength(25)
                .HasColumnName("cin");
            entity.Property(e => e.Ckycid)
                .HasMaxLength(20)
                .HasColumnName("ckycid");
            entity.Property(e => e.Companyname)
                .HasMaxLength(100)
                .HasColumnName("companyname");
            entity.Property(e => e.Customertype).HasColumnName("customertype");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Drivinglicensenumber)
                .HasMaxLength(50)
                .HasColumnName("drivinglicensenumber");
            entity.Property(e => e.Emailid)
                .HasMaxLength(100)
                .HasColumnName("emailid");
            entity.Property(e => e.Fatherfirstname)
                .HasMaxLength(100)
                .HasColumnName("fatherfirstname");
            entity.Property(e => e.Fatherlastname)
                .HasMaxLength(100)
                .HasColumnName("fatherlastname");
            entity.Property(e => e.Fathermiddlename)
                .HasMaxLength(100)
                .HasColumnName("fathermiddlename");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Middlename)
                .HasMaxLength(100)
                .HasColumnName("middlename");
            entity.Property(e => e.Mobilenumber).HasColumnName("mobilenumber");
            entity.Property(e => e.Mothermaidenname)
                .HasMaxLength(100)
                .HasColumnName("mothermaidenname");
            entity.Property(e => e.Pannumber)
                .HasMaxLength(20)
                .HasColumnName("pannumber");
            entity.Property(e => e.Passportnumber)
                .HasMaxLength(50)
                .HasColumnName("passportnumber");
            entity.Property(e => e.Sourcesystem)
                .HasMaxLength(25)
                .HasColumnName("sourcesystem");
            entity.Property(e => e.Spousefirstname)
                .HasMaxLength(100)
                .HasColumnName("spousefirstname");
            entity.Property(e => e.Spouselastname)
                .HasMaxLength(100)
                .HasColumnName("spouselastname");
            entity.Property(e => e.Spousemiddlename)
                .HasMaxLength(100)
                .HasColumnName("spousemiddlename");
            entity.Property(e => e.Voterid)
                .HasMaxLength(50)
                .HasColumnName("voterid");
        });

        modelBuilder.Entity<Customeraudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customerrequestlog_pkey");

            entity.ToTable("customeraudit");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Aadharnumber)
                .HasMaxLength(20)
                .HasColumnName("aadharnumber");
            entity.Property(e => e.Alternatemobilenumber).HasColumnName("alternatemobilenumber");
            entity.Property(e => e.Cin)
                .HasMaxLength(25)
                .HasColumnName("cin");
            entity.Property(e => e.Ckycid)
                .HasMaxLength(20)
                .HasColumnName("ckycid");
            entity.Property(e => e.Companyname)
                .HasMaxLength(100)
                .HasColumnName("companyname");
            entity.Property(e => e.Correlationid)
                .HasMaxLength(50)
                .HasColumnName("correlationid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Customertype).HasColumnName("customertype");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Drivinglicensenumber)
                .HasMaxLength(50)
                .HasColumnName("drivinglicensenumber");
            entity.Property(e => e.Emailid)
                .HasMaxLength(100)
                .HasColumnName("emailid");
            entity.Property(e => e.Fatherfirstname)
                .HasMaxLength(100)
                .HasColumnName("fatherfirstname");
            entity.Property(e => e.Fatherlastname)
                .HasMaxLength(100)
                .HasColumnName("fatherlastname");
            entity.Property(e => e.Fathermiddlename)
                .HasMaxLength(100)
                .HasColumnName("fathermiddlename");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Middlename)
                .HasMaxLength(100)
                .HasColumnName("middlename");
            entity.Property(e => e.Mobilenumber).HasColumnName("mobilenumber");
            entity.Property(e => e.Mothermaidenname)
                .HasMaxLength(100)
                .HasColumnName("mothermaidenname");
            entity.Property(e => e.Pannumber)
                .HasMaxLength(20)
                .HasColumnName("pannumber");
            entity.Property(e => e.Passportnumber)
                .HasMaxLength(50)
                .HasColumnName("passportnumber");
            entity.Property(e => e.Sourcesystem)
                .HasMaxLength(25)
                .HasColumnName("sourcesystem");
            entity.Property(e => e.Spousefirstname)
                .HasMaxLength(100)
                .HasColumnName("spousefirstname");
            entity.Property(e => e.Spouselastname)
                .HasMaxLength(100)
                .HasColumnName("spouselastname");
            entity.Property(e => e.Spousemiddlename)
                .HasMaxLength(100)
                .HasColumnName("spousemiddlename");
            entity.Property(e => e.Voterid)
                .HasMaxLength(50)
                .HasColumnName("voterid");
        });

        modelBuilder.Entity<Customermapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customermapping_pkey");

            entity.ToTable("customermapping");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Externalcustomerid)
                .HasMaxLength(100)
                .HasColumnName("externalcustomerid");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("logs_pkey");

            entity.ToTable("logs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Correlationid).HasColumnName("correlationid");
            entity.Property(e => e.Level)
                .HasMaxLength(50)
                .HasColumnName("level");
            entity.Property(e => e.Loggedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("loggedat");
            entity.Property(e => e.Message).HasColumnName("message");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
