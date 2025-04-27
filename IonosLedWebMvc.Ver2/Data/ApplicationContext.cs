using System;
using System.Collections.Generic;
using IonosLedWebMvc.Ver2.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace IonosLedWebMvc.Ver2.Data;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ModelsLedLight> ModelsLedLights { get; set; }

    public virtual DbSet<ProductsLedLight> ProductsLedLights { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserEvent> UserEvents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        /*optionsBuilder.UseMySql("server=127.0.0.1;user=root;database=ionosprod_ver2;port=3306;password=admin", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.4.4-mysql"));*/
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<ModelsLedLight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("models_led_lights");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssemblyPrice)
                .HasColumnType("decimal(8,2) unsigned")
                .HasColumnName("assembly_price");
            entity.Property(e => e.CheckPrice)
                .HasColumnType("decimal(8,2) unsigned")
                .HasColumnName("check_price");
            entity.Property(e => e.CutPrice)
                .HasColumnType("decimal(8,2) unsigned")
                .HasColumnName("cut_price");
            entity.Property(e => e.DrillPrice)
                .HasColumnType("decimal(8,2) unsigned")
                .HasColumnName("drill_price");
            entity.Property(e => e.ModelName)
                .HasMaxLength(64)
                .HasColumnName("model_name");
            entity.Property(e => e.MountPrice)
                .HasColumnType("decimal(8,2) unsigned")
                .HasColumnName("mount_price");
            entity.Property(e => e.Sections)
                .HasDefaultValueSql("'1'")
                .HasColumnName("sections");
            entity.Property(e => e.SolderPrice)
                .HasColumnType("decimal(8,2) unsigned")
                .HasColumnName("solder_price");
        });

        modelBuilder.Entity<ProductsLedLight>(entity =>
        {
            entity.HasKey(e => e.Serial).HasName("PRIMARY");

            entity.ToTable("products_led_lights");

            entity.HasIndex(e => e.AlProfileCutUser, "al_profile_cut_user");

            entity.HasIndex(e => e.AlProfileDrillUser, "al_profile_drill_user");

            entity.HasIndex(e => e.LabelPrintUser, "label_print_user");

            entity.HasIndex(e => e.LedModuleMountingUser, "led_module_mounting_user");

            entity.HasIndex(e => e.LightAssemblingUser, "light_assembling_user");

            entity.HasIndex(e => e.LightCheckingPackagingUser, "light_checking_packaging_user");

            entity.HasIndex(e => e.LightSolderingUser, "light_soldering_user");

            entity.HasIndex(e => e.ModelId, "model_id");

            entity.Property(e => e.Serial).HasColumnName("serial");
            entity.Property(e => e.AlProfileCutTs)
                .HasColumnType("timestamp")
                .HasColumnName("al_profile_cut_ts");
            entity.Property(e => e.AlProfileCutUser).HasColumnName("al_profile_cut_user");
            entity.Property(e => e.AlProfileDrillTs)
                .HasColumnType("timestamp")
                .HasColumnName("al_profile_drill_ts");
            entity.Property(e => e.AlProfileDrillUser).HasColumnName("al_profile_drill_user");
            entity.Property(e => e.BitrixOrder).HasColumnName("bitrix_order");
            entity.Property(e => e.Comment)
                .HasMaxLength(128)
                .HasColumnName("comment");
            entity.Property(e => e.LabelPrintTs)
                .HasColumnType("timestamp")
                .HasColumnName("label_print_ts");
            entity.Property(e => e.LabelPrintUser).HasColumnName("label_print_user");
            entity.Property(e => e.LedModuleMountingTs)
                .HasColumnType("timestamp")
                .HasColumnName("led_module_mounting_ts");
            entity.Property(e => e.LedModuleMountingUser).HasColumnName("led_module_mounting_user");
            entity.Property(e => e.LightAssemblingTs)
                .HasColumnType("timestamp")
                .HasColumnName("light_assembling_ts");
            entity.Property(e => e.LightAssemblingUser).HasColumnName("light_assembling_user");
            entity.Property(e => e.LightCheckingPackagingTs)
                .HasColumnType("timestamp")
                .HasColumnName("light_checking_packaging_ts");
            entity.Property(e => e.LightCheckingPackagingUser).HasColumnName("light_checking_packaging_user");
            entity.Property(e => e.LightSolderingTs)
                .HasColumnType("timestamp")
                .HasColumnName("light_soldering_ts");
            entity.Property(e => e.LightSolderingUser).HasColumnName("light_soldering_user");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.Spec)
                .HasMaxLength(32)
                .HasColumnName("spec");

            entity.HasOne(d => d.AlProfileCutUserNavigation).WithMany(p => p.ProductsLedLightAlProfileCutUserNavigations)
                .HasForeignKey(d => d.AlProfileCutUser)
                .HasConstraintName("al_profile_cut_user");

            entity.HasOne(d => d.AlProfileDrillUserNavigation).WithMany(p => p.ProductsLedLightAlProfileDrillUserNavigations)
                .HasForeignKey(d => d.AlProfileDrillUser)
                .HasConstraintName("al_profile_drill_user");

            entity.HasOne(d => d.LabelPrintUserNavigation).WithMany(p => p.ProductsLedLightLabelPrintUserNavigations)
                .HasForeignKey(d => d.LabelPrintUser)
                .HasConstraintName("label_print_user");

            entity.HasOne(d => d.LedModuleMountingUserNavigation).WithMany(p => p.ProductsLedLightLedModuleMountingUserNavigations)
                .HasForeignKey(d => d.LedModuleMountingUser)
                .HasConstraintName("led_module_mounting_user");

            entity.HasOne(d => d.LightAssemblingUserNavigation).WithMany(p => p.ProductsLedLightLightAssemblingUserNavigations)
                .HasForeignKey(d => d.LightAssemblingUser)
                .HasConstraintName("light_assembling_user");

            entity.HasOne(d => d.LightCheckingPackagingUserNavigation).WithMany(p => p.ProductsLedLightLightCheckingPackagingUserNavigations)
                .HasForeignKey(d => d.LightCheckingPackagingUser)
                .HasConstraintName("light_checking_packaging_user");

            entity.HasOne(d => d.LightSolderingUserNavigation).WithMany(p => p.ProductsLedLightLightSolderingUserNavigations)
                .HasForeignKey(d => d.LightSolderingUser)
                .HasConstraintName("light_soldering_user");

            entity.HasOne(d => d.Model).WithMany(p => p.ProductsLedLights)
                .HasForeignKey(d => d.ModelId)
                .HasConstraintName("model_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PermissionAlProfileCut).HasColumnName("permission_al_profile_cut");
            entity.Property(e => e.PermissionAlProfileDrill).HasColumnName("permission_al_profile_drill");
            entity.Property(e => e.PermissionChiefLightProduction).HasColumnName("permission_chief_light_production");
            entity.Property(e => e.PermissionLedModuleMounting).HasColumnName("permission_led_module_mounting");
            entity.Property(e => e.PermissionLightAssembling).HasColumnName("permission_light_assembling");
            entity.Property(e => e.PermissionLightCheckingPackaging).HasColumnName("permission_light_checking_packaging");
            entity.Property(e => e.PermissionLightSoldering).HasColumnName("permission_light_soldering");
            entity.Property(e => e.PermissionSettings).HasColumnName("permission_settings");
            entity.Property(e => e.RoleName)
                .HasMaxLength(64)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Role, "role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
            entity.Property(e => e.Pin).HasColumnName("pin");
            entity.Property(e => e.Role).HasColumnName("role");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("role");
        });

        modelBuilder.Entity<UserEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_events");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Event)
                .HasMaxLength(16)
                .HasColumnName("event");
            entity.Property(e => e.Ts)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("ts");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserEvents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
