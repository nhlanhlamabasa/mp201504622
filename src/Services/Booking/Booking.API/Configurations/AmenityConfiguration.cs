namespace Booking.API.Configurations
{
#pragma warning disable
    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
    {

        public void Configure(EntityTypeBuilder<Amenity> builder)
        {
            builder.ToTable("Amenities");
            builder.HasKey(r => r.Id);
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.Property<string>(r => r.Description).HasColumnName("Description").IsRequired();
            builder.Property<string>(r => r.ImageName).HasColumnName("ImageName").IsRequired();
            builder.Property<string>(r => r.ImageID).HasColumnName("ImageID").IsRequired();
            builder.HasMany(r => r.Rooms).WithOne(r => r.Amenity).IsRequired(false).HasForeignKey(r => r.AmenityId);
        }
    }
}
