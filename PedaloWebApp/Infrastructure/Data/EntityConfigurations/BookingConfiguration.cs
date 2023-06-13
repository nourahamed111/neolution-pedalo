using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PedaloWebApp.Core.Domain.Entities;

namespace PedaloWebApp.Infrastructure.Data.EntityConfigurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.BookingId);
            builder.Property(x => x.PassengerNames).HasMaxLength(255).IsRequired(false);
        }
    }
}
