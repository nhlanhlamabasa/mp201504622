namespace Booking.API.Configurations
{
#pragma warning disable
    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class RoomBookingConfiguration : IEntityTypeConfiguration<RoomBooking>
    {
        public void Configure(EntityTypeBuilder<RoomBooking> builder)
        {
            builder.ToTable("RoomBookings");
            builder.HasKey(rb => new { rb.RoomId, rb.BookingId });
            builder.Property<DateTime>(r => r.Arrival).HasColumnName("Arrival").IsRequired();
            builder.Property<DateTime>(r => r.Departure).HasColumnName("Departure").IsRequired();
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.Ignore(x => x.Id);
            builder.HasOne(r => r.Booking).WithMany(r => r.RoomBookings).HasForeignKey(k => k.BookingId);
            builder.HasOne(r => r.Room).WithMany(r => r.RoomBookings).HasForeignKey(k => k.RoomId);
        }
    }
}
