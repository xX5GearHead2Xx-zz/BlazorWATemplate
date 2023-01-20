using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorWATemplate.Shared.DBModels
{
    public partial class BlazorTemplateContext : DbContext
    {
        public BlazorTemplateContext()
        {
        }

        public BlazorTemplateContext(DbContextOptions<BlazorTemplateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LinkUserRole> LinkUserRoles { get; set; } = null!;
        public virtual DbSet<PasswordRecovery> PasswordRecoveries { get; set; } = null!;
        public virtual DbSet<ReferenceUserAccessType> ReferenceUserAccessTypes { get; set; } = null!;
        public virtual DbSet<ReferenceUserRole> ReferenceUserRoles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LinkUserRole>(entity =>
            {
                entity.ToTable("LinkUserRole");

                entity.Property(e => e.LinkUserRoleId).HasColumnName("LinkUserRole_ID");

                entity.Property(e => e.AccessTypeId).HasColumnName("AccessType_ID");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.AccessType)
                    .WithMany(p => p.LinkUserRoles)
                    .HasForeignKey(d => d.AccessTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LinkUserR__Acces__2D27B809");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.LinkUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LinkUserR__Role___2C3393D0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LinkUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LinkUserR__User___2B3F6F97");
            });

            modelBuilder.Entity<PasswordRecovery>(entity =>
            {
                entity.ToTable("PasswordRecovery");

                entity.Property(e => e.PasswordRecoveryId).HasColumnName("PasswordRecovery_ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Expiry).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PasswordRecoveries)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PasswordR__User___300424B4");
            });

            modelBuilder.Entity<ReferenceUserAccessType>(entity =>
            {
                entity.HasKey(e => e.AccessTypeId)
                    .HasName("PK__Referenc__D621E23FF766768C");

                entity.ToTable("ReferenceUserAccessType");

                entity.Property(e => e.AccessTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccessType_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ReferenceUserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__Referenc__D80AB49B816B8247");

                entity.ToTable("ReferenceUserRole");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("Role_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastActive)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(1000);

                entity.Property(e => e.Salt).HasMaxLength(1000);

                entity.Property(e => e.Surname)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
