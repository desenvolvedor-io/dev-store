using DevStore.Core.Configurations;
using DevStore.Orders.Domain.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevStore.Orders.Infra.Mappings
{
    public class VoucherMapping : BaseConfiguration<Voucher>
    {
        public override void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.Property(c => c.Code)
                .IsRequired()
                .HasColumnType("varchar(100)");

            base.Configure(builder);
        }
    }
}