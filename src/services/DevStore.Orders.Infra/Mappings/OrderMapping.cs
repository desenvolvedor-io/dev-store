using DevStore.Core.Configurations;
using DevStore.Orders.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevStore.Orders.Infra.Mappings
{
    public class OrderMapping : BaseConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(p => p.Address, e =>
            {
                e.Property(pe => pe.StreetAddress)
                    .HasColumnName("StreetAddress");

                e.Property(pe => pe.BuildingNumber)
                    .HasColumnName("BuildingNumber");

                e.Property(pe => pe.SecondaryAddress)
                    .HasColumnName("SecondaryAddress");

                e.Property(pe => pe.Neighborhood)
                    .HasColumnName("Neighborhood");

                e.Property(pe => pe.ZipCode)
                    .HasColumnName("ZipCode");

                e.Property(pe => pe.City)
                    .HasColumnName("City");

                e.Property(pe => pe.State)
                    .HasColumnName("State");
            });

            builder.Property(c => c.Code)
                .HasDefaultValueSql("NEXT VALUE FOR MySequence");

            // 1 : N => Order : OrderItems
            builder.HasMany(c => c.OrderItems)
                .WithOne(c => c.Order)
                .HasForeignKey(c => c.OrderId);

            base.Configure(builder);
        }
    }
}