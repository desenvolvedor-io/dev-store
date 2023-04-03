using DevStore.Core.Configurations;
using DevStore.Customers.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevStore.Customers.API.Data.Mappings
{
    public class AddressMapping : BaseConfiguration<Address>
    {
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(c => c.StreetAddress)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(c => c.BuildingNumber)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(c => c.ZipCode)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(c => c.SecondaryAddress)
                .HasColumnType("varchar(250)");

            builder.Property(c => c.Neighborhood)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.City)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.State)
                .IsRequired()
                .HasColumnType("varchar(50)");

            base.Configure(builder);
        }
    }
}