using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ENSEK.WebApi.Infrastucture.Repository.Entities.Configurations;

public class MeterReadConfiguration : IEntityTypeConfiguration<MeterRead>
{
    public void Configure(EntityTypeBuilder<MeterRead> builder)
    {
        builder.HasIndex(x => x.AccountId)
               .IsUnique();

        builder.Property(x => x.MeterReadingDateTime)
               .IsRequired();

        builder.Property(x => x.MeterReadValue)
               .IsRequired();
    }
}