using DevStore.Billing.API.Models;
using DevStore.Core.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevStore.Billing.API.Data.Mappings
{
    public class PaymentMapping : BaseConfiguration<Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Ignore(c => c.CreditCard);

            // 1 : N => Payment : Transaction
            builder.HasMany(c => c.Transactions)
                .WithOne(c => c.Payment)
                .HasForeignKey(c => c.PaymentId);

            base.Configure(builder);
        }
    }
}