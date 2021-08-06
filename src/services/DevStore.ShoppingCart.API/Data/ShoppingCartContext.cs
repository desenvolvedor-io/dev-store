using DevStore.ShoppingCart.API.Model;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DevStore.ShoppingCart.API.Data
{
    public sealed class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ShoppingCartClient> ShoppingCartClient { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.Entity<ShoppingCartClient>()
                .HasIndex(c => c.ClientId)
                .HasName("IDX_Cliente");

            modelBuilder.Entity<ShoppingCartClient>()
                .Ignore(c => c.Voucher)
                .OwnsOne(c => c.Voucher, v =>
                {
                    v.Property(vc => vc.Code)
                        .HasColumnType("varchar(50)");

                    v.Property(vc => vc.DiscountType);

                    v.Property(vc => vc.Percentage);

                    v.Property(vc => vc.Discount);
                });

            modelBuilder.Entity<ShoppingCartClient>()
                .HasMany(c => c.Items)
                .WithOne(i => i.ShoppingCartClient)
                .HasForeignKey(c => c.ShoppingCartId);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Cascade;
        }
    }
}