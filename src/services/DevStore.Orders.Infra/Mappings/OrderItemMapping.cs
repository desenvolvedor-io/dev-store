using DevStore.Core.Configurations;
using DevStore.Orders.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevStore.Orders.Infra.Mappings
{
    public class OrderItemMapping : BaseConfiguration<OrderItem>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(c => c.ProductName)
                .IsRequired()
                .HasColumnType("varchar(250)");

            // 1 : N => Order : Pagamento
            builder.HasOne(c => c.Order)
                .WithMany(c => c.OrderItems);

            base.Configure(builder);
        }
    }
}