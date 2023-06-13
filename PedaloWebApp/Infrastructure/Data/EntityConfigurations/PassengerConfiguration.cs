namespace PedaloWebApp.Infrastructure.Data.EntityConfigurations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using PedaloWebApp.Core.Domain.Entities;

    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.HasKey(x => x.PassengerId);
            builder.Property(x => x.PassengerFirstName).IsRequired().HasMaxLength(80);
            builder.Property(x => x.PassengerLastName).IsRequired().HasMaxLength(80);
        }
    }
}
