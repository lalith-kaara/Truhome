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

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customerrequestlog> Customerrequestlogs { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_customer_id");

            entity.ToTable("customer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Aadharnumber)
                .HasMaxLength(20)
                .HasColumnName("aadharnumber");
            entity.Property(e => e.Ckycnumber)
                .HasMaxLength(20)
                .HasColumnName("ckycnumber");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Drivinglicensenumber)
                .HasMaxLength(50)
                .HasColumnName("drivinglicensenumber");
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
            entity.Property(e => e.Husbandfirstname)
                .HasMaxLength(100)
                .HasColumnName("husbandfirstname");
            entity.Property(e => e.Husbandlastname)
                .HasMaxLength(100)
                .HasColumnName("husbandlastname");
            entity.Property(e => e.Husbandmiddlename)
                .HasMaxLength(100)
                .HasColumnName("husbandmiddlename");
            entity.Property(e => e.Lastname)
                .HasMaxLength(100)
                .HasColumnName("lastname");
            entity.Property(e => e.Middlename)
                .HasMaxLength(100)
                .HasColumnName("middlename");
            entity.Property(e => e.Mobilenumber).HasColumnName("mobilenumber");
            entity.Property(e => e.Pannumber)
                .HasMaxLength(20)
                .HasColumnName("pannumber");
            entity.Property(e => e.Passportnumber)
                .HasMaxLength(50)
                .HasColumnName("passportnumber");
            entity.Property(e => e.Voterid)
                .HasMaxLength(50)
                .HasColumnName("voterid");
        });

        modelBuilder.Entity<Customerrequestlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customerrequestlog_pkey");

            entity.ToTable("customerrequestlog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Aadharnumber)
                .HasMaxLength(100)
                .HasColumnName("aadharnumber");
            entity.Property(e => e.Ckycnumber)
                .HasMaxLength(100)
                .HasColumnName("ckycnumber");
            entity.Property(e => e.Correlationid)
                .HasMaxLength(50)
                .HasColumnName("correlationid");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Drivinglicensenumber)
                .HasMaxLength(100)
                .HasColumnName("drivinglicensenumber");
            entity.Property(e => e.Fatherfirstname)
                .HasMaxLength(255)
                .HasColumnName("fatherfirstname");
            entity.Property(e => e.Fatherlastname)
                .HasMaxLength(255)
                .HasColumnName("fatherlastname");
            entity.Property(e => e.Fathermiddlename)
                .HasMaxLength(255)
                .HasColumnName("fathermiddlename");
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Husbandfirstname)
                .HasMaxLength(255)
                .HasColumnName("husbandfirstname");
            entity.Property(e => e.Husbandlastname)
                .HasMaxLength(255)
                .HasColumnName("husbandlastname");
            entity.Property(e => e.Husbandmiddlename)
                .HasMaxLength(255)
                .HasColumnName("husbandmiddlename");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasColumnName("lastname");
            entity.Property(e => e.Middlename)
                .HasMaxLength(255)
                .HasColumnName("middlename");
            entity.Property(e => e.Mobilenumber).HasColumnName("mobilenumber");
            entity.Property(e => e.Pannumber)
                .HasMaxLength(50)
                .HasColumnName("pannumber");
            entity.Property(e => e.Passportnumber)
                .HasMaxLength(100)
                .HasColumnName("passportnumber");
            entity.Property(e => e.Systemorigin)
                .HasMaxLength(300)
                .HasColumnName("systemorigin");
            entity.Property(e => e.Voterid)
                .HasMaxLength(100)
                .HasColumnName("voterid");
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
