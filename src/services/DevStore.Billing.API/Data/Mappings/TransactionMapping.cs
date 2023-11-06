using DevStore.Billing.API.Models;
using DevStore.Core.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevStore.Billing.API.Data.Mappings
{
    public class TransactionMapping : BaseConfiguration<Transaction>
    {
        public override void Configure(EntityTypeBuilder<Transaction> builder)
        {
            // 1 : N => Payment : Transaction
            builder.HasOne(c => c.Payment)
                .WithMany(c => c.Transactions);

            base.Configure(builder);
        }
    }
}