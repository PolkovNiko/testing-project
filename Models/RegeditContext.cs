using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace staff_register.Models;

public partial class RegeditContext : DbContext
{
    public RegeditContext()
    {
    }

    public RegeditContext(DbContextOptions<RegeditContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Departmen> Departmens { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:regedit");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departmen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC071E16B505");

            entity.Property(e => e.About)
                .HasColumnType("text")
                .HasColumnName("about");
            entity.Property(e => e.IdBoss).HasColumnName("id_boss");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");

            entity.HasOne(d => d.IdBossNavigation).WithMany(p => p.Departmen)
                .HasForeignKey(d => d.IdBoss)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Departmens_ToStaff");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Staff__3214EC075C245E5B");

            entity.HasIndex(e => e.Department, "AK_Staff_department").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Adress)
                .HasColumnType("text")
                .HasColumnName("adress");
            entity.Property(e => e.Birthday)
                .HasColumnType("date")
                .HasColumnName("birthday");
            entity.Property(e => e.Department).HasColumnName("department");
            entity.Property(e => e.FamilyStatus)
                .HasColumnType("text")
                .HasColumnName("family_status");
            entity.Property(e => e.Fio)
                .HasColumnType("text")
                .HasColumnName("fio");
            entity.Property(e => e.Number)
                .HasMaxLength(13)
                .HasColumnName("number");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.Wage)
                .HasColumnType("smallmoney")
                .HasColumnName("wage");

            entity.HasOne(d => d.DepartmentNavigation).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.Department)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_ToDepartment");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_ToUsers");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07AB0E32D5");

            entity.HasIndex(e => e.Login, "UQ__tmp_ms_x__7838F27227A4762C").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.Login)
                .HasMaxLength(20)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(18)
                .HasColumnName("password");
            entity.Property(e => e.Rank)
                .HasColumnType("text")
                .HasColumnName("rank");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
