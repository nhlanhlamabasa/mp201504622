namespace Booking.API.Configurations
{
#pragma warning disable
    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.ToTable("Hotels");
            builder.HasKey(h => h.Id);
            builder.Property<string>(h => h.Name).HasColumnName("Name").IsRequired();
            builder.Property<int>(h => h.NumberOfRooms).HasColumnName("NumberOfRooms").IsRequired().HasDefaultValue(0);
            builder.Property(h => h.Rating).HasColumnName("Rating").IsRequired();
            builder.Property<string>(h => h.PostalCode).HasColumnName("PostalCode").IsRequired();
            builder.Property<string>(p => p.Country).HasColumnName("Country").IsRequired();
            builder.Property<string>(p => p.City).HasColumnName("City").IsRequired();
            builder.Property<string>(p => p.Province).HasColumnName("Province").IsRequired();
            builder.Property<string>(p => p.Street).HasColumnName("Street").IsRequired();
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.Property<string>(r => r.ImageID).HasColumnName("ImageID").IsRequired();
            builder.Property<int>(r => r.NumberOfBookings).HasColumnName("NumberOfBookings").IsRequired().HasDefaultValue(0);
            builder.Property<string>(r => r.ImageName).HasColumnName("ImageName").IsRequired();
            builder.HasMany(h => h.Rooms).WithOne().IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
