namespace Booking.API.Configurations
{
#pragma warning disable

    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class RoomAmenityConfiguration : IEntityTypeConfiguration<RoomAmenity>
    {
        public void Configure(EntityTypeBuilder<RoomAmenity> builder)
        {
            builder.ToTable("RoomAmenities");
            builder.HasKey(rb => new { rb.RoomId, rb.AmenityId });
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.HasOne(r => r.Amenity).WithMany(r => r.Rooms).HasForeignKey(k => k.AmenityId);
            builder.HasOne(r => r.Room).WithMany(r => r.Amenities).HasForeignKey(k => k.RoomId);
        }
    }
}
