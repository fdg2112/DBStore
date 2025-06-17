using System;
using System.Collections.Generic;
using DBStore.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace DBStore.Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<audit_log> audit_logs { get; set; }

    public virtual DbSet<cart> carts { get; set; }

    public virtual DbSet<cart_item> cart_items { get; set; }

    public virtual DbSet<favorite> favorites { get; set; }

    public virtual DbSet<order> orders { get; set; }

    public virtual DbSet<order_item> order_items { get; set; }

    public virtual DbSet<product> products { get; set; }

    public virtual DbSet<role> roles { get; set; }

    public virtual DbSet<shipping_address> shipping_addresses { get; set; }

    public virtual DbSet<user> users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=aws-0-sa-east-1.pooler.supabase.com;Port=5432;Database=postgres;User Id=postgres.ffgemqzyzytivikktofs;Password=Chupame1huevo!;SSL Mode=Require;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<audit_log>(entity =>
        {
            entity.HasKey(e => e.id).HasName("audit_logs_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.changed_data).HasColumnType("jsonb");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.admin).WithMany(p => p.audit_logs)
                .HasForeignKey(d => d.admin_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("audit_logs_admin_id_fkey");
        });

        modelBuilder.Entity<cart>(entity =>
        {
            entity.HasKey(e => e.id).HasName("carts_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.user).WithMany(p => p.carts)
                .HasForeignKey(d => d.user_id)
                .HasConstraintName("carts_user_id_fkey");
        });

        modelBuilder.Entity<cart_item>(entity =>
        {
            entity.HasKey(e => e.id).HasName("cart_items_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.quantity).HasDefaultValue(1);

            entity.HasOne(d => d.cart).WithMany(p => p.cart_items)
                .HasForeignKey(d => d.cart_id)
                .HasConstraintName("cart_items_cart_id_fkey");

            entity.HasOne(d => d.product).WithMany(p => p.cart_items)
                .HasForeignKey(d => d.product_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cart_items_product_id_fkey");
        });

        modelBuilder.Entity<favorite>(entity =>
        {
            entity.HasKey(e => e.id).HasName("favorites_pkey");

            entity.HasIndex(e => new { e.user_id, e.product_id }, "favorites_user_id_product_id_key").IsUnique();

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.product).WithMany(p => p.favorites)
                .HasForeignKey(d => d.product_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favorites_product_id_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.favorites)
                .HasForeignKey(d => d.user_id)
                .HasConstraintName("favorites_user_id_fkey");
        });

        modelBuilder.Entity<order>(entity =>
        {
            entity.HasKey(e => e.id).HasName("orders_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.payment_status).HasDefaultValueSql("'pending'::text");
            entity.Property(e => e.shipping_status).HasDefaultValueSql("'pending'::text");
            entity.Property(e => e.total).HasPrecision(10, 2);
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.billing_address).WithMany(p => p.orders)
                .HasForeignKey(d => d.billing_address_id)
                .HasConstraintName("orders_billing_address_id_fkey");

            entity.HasOne(d => d.created_byNavigation).WithMany(p => p.ordercreated_byNavigations)
                .HasForeignKey(d => d.created_by)
                .HasConstraintName("orders_created_by_fkey");

            entity.HasOne(d => d.updated_byNavigation).WithMany(p => p.orderupdated_byNavigations)
                .HasForeignKey(d => d.updated_by)
                .HasConstraintName("orders_updated_by_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.orderusers)
                .HasForeignKey(d => d.user_id)
                .HasConstraintName("orders_user_id_fkey");
        });

        modelBuilder.Entity<order_item>(entity =>
        {
            entity.HasKey(e => e.id).HasName("order_items_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.price).HasPrecision(10, 2);
            entity.Property(e => e.quantity).HasDefaultValue(1);

            entity.HasOne(d => d.order).WithMany(p => p.order_items)
                .HasForeignKey(d => d.order_id)
                .HasConstraintName("order_items_order_id_fkey");

            entity.HasOne(d => d.product).WithMany(p => p.order_items)
                .HasForeignKey(d => d.product_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_items_product_id_fkey");
        });

        modelBuilder.Entity<product>(entity =>
        {
            entity.HasKey(e => e.id).HasName("products_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.price).HasPrecision(10, 2);
            entity.Property(e => e.stock).HasDefaultValue(0);
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.created_byNavigation).WithMany(p => p.productcreated_byNavigations)
                .HasForeignKey(d => d.created_by)
                .HasConstraintName("products_created_by_fkey");

            entity.HasOne(d => d.updated_byNavigation).WithMany(p => p.productupdated_byNavigations)
                .HasForeignKey(d => d.updated_by)
                .HasConstraintName("products_updated_by_fkey");
        });

        modelBuilder.Entity<role>(entity =>
        {
            entity.HasKey(e => e.id).HasName("roles_pkey");

            entity.HasIndex(e => e.name, "roles_name_key").IsUnique();

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<shipping_address>(entity =>
        {
            entity.HasKey(e => e.id).HasName("shipping_addresses_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.cost).HasPrecision(10, 2);
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.user).WithMany(p => p.shipping_addresses)
                .HasForeignKey(d => d.user_id)
                .HasConstraintName("shipping_addresses_user_id_fkey");
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.id).HasName("users_pkey");

            entity.HasIndex(e => e.email, "users_email_key").IsUnique();

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.email_verified).HasDefaultValue(false);
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");

            entity.HasMany(d => d.roles).WithMany(p => p.users)
                .UsingEntity<Dictionary<string, object>>(
                    "user_role",
                    r => r.HasOne<role>().WithMany()
                        .HasForeignKey("role_id")
                        .HasConstraintName("user_roles_role_id_fkey"),
                    l => l.HasOne<user>().WithMany()
                        .HasForeignKey("user_id")
                        .HasConstraintName("user_roles_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("user_id", "role_id").HasName("user_roles_pkey");
                        j.ToTable("user_roles");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
