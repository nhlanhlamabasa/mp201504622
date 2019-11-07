namespace Booking.API.Configurations
{
#pragma warning disable
    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {

        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Rooms");
            builder.HasKey(r => r.Id);
            builder.Property<decimal>(r => r.Cost).HasColumnName("Cost").IsRequired().HasColumnType("money");
            builder.Property<int>(r => r.RoomNumber).HasColumnName("RoomNumber").IsRequired();
            builder.Property<string>(r => r.RoomType).HasColumnName("RoomType").IsRequired();
            builder.Property<int>(r => r.Capacity).HasColumnName("Capacity").IsRequired();
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.Property<string>(x => x.HotelName).HasColumnName("HotelName").IsRequired();
            builder.Property<string>(r => r.ImageName).HasColumnName("ImageName").IsRequired();
            builder.HasOne(r => r.Hotel).WithMany(h => h.Rooms).HasForeignKey(r => r.HotelId);
            builder.Property<string>(r => r.ImageID).HasColumnName("ImageID").IsRequired();
            builder.HasMany(r => r.RoomBookings).WithOne();
            builder.HasMany(r => r.Amenities).WithOne(r => r.Room).IsRequired(false).HasForeignKey(r => r.RoomId);
        }
    }
}
