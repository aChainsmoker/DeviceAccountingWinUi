using System;
using System.Collections.Generic;
using DeviceAccountingWinUi.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceAccountingWinUi.DatabaseControl;

public partial class OrganisationContext : DbContext
{
    public OrganisationContext()
    {
    }

    public OrganisationContext(DbContextOptions<OrganisationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<DeviceTransfer> DeviceTransfers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("User ID=postgres;Password=pk1Gwb98;Host=localhost;Port=5432;Database=organisation;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Departments_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .HasColumnName("short_name");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("Devices_pkey");

            entity.Property(e => e.InventoryId)
                .HasMaxLength(100)
                .HasColumnName("inventory_id");
            entity.Property(e => e.Cost)
                .HasPrecision(10, 2)
                .HasColumnName("cost");
            entity.Property(e => e.DateOfObtaining).HasColumnName("date_of_obtaining");
            entity.Property(e => e.Model)
                .HasMaxLength(255)
                .HasColumnName("model");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(50)
                .HasColumnName("room_number");
            entity.Property(e => e.WorkerId).HasColumnName("worker_id");

            entity.HasOne(d => d.RoomNumberNavigation).WithMany(p => p.Devices)
                .HasForeignKey(d => d.RoomNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Devices_room_number_fkey");

            entity.HasOne(d => d.Worker).WithMany(p => p.Devices)
                .HasForeignKey(d => d.WorkerId)
                .HasConstraintName("Devices_worker_id_fkey");
        });

        modelBuilder.Entity<DeviceTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DeviceTransfers_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOfTransfer).HasColumnName("date_of_transfer");
            entity.Property(e => e.DeviceId)
                .HasMaxLength(100)
                .HasColumnName("device_id");
            entity.Property(e => e.FromWorkerId).HasColumnName("from_worker_id");
            entity.Property(e => e.ToWorkerId).HasColumnName("to_worker_id");

            entity.HasOne(d => d.Device).WithMany(p => p.DeviceTransfers)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DeviceTransfers_device_id_fkey");

            entity.HasOne(d => d.FromWorker).WithMany(p => p.DeviceTransferFromWorkers)
                .HasForeignKey(d => d.FromWorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DeviceTransfers_from_worker_id_fkey");

            entity.HasOne(d => d.ToWorker).WithMany(p => p.DeviceTransferToWorkers)
                .HasForeignKey(d => d.ToWorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DeviceTransfers_to_worker_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Number).HasName("Rooms_pkey");

            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .HasColumnName("number");
            entity.Property(e => e.Area)
                .HasPrecision(10, 2)
                .HasColumnName("area");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");

            entity.HasOne(d => d.Department).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("Rooms_department_id_fkey");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Workers_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Department).WithMany(p => p.Workers)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("Workers_department_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Workers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("Workers_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
