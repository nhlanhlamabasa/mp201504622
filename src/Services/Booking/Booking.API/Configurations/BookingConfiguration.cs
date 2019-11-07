namespace Booking.API.Configurations
{
#pragma warning disable
    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class BookingConfiguration : IEntityTypeConfiguration<Entities.Booking>
    {
        public void Configure(EntityTypeBuilder<Entities.Booking> builder)
        {
            builder.ToTable("Bookings");
            builder.HasKey(b => b.Id);
            builder.Property<int>(b => b.NumberOfGuests).HasColumnName("NumberOfGuests").IsRequired();
            builder.Property<DateTime>(r => r.Arrival).HasColumnName("Arrival").IsRequired();
            builder.Property<DateTime>(r => r.Departure).HasColumnName("Departure").IsRequired();
            builder.Property<string>(r => r.Status).HasColumnName("Status").IsRequired();
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.Property<Guid>(b => b.BookerId).HasColumnName("BookerId").IsRequired();
            builder.Property<string>(b => b.HotelName).HasColumnName("HotelName").IsRequired();
            builder.Property<Guid>(b => b.HotelId).HasColumnName("HotelId").IsRequired();
            builder.HasMany(r => r.RoomBookings).WithOne();
            builder.HasOne(r => r.Invoice).WithOne(r => r.Booking).HasForeignKey<Invoice>(r => r.BookingId);
            builder.Property<Guid>(r => r.CheckInPerson).HasColumnName("ChekInPerson").IsRequired().HasDefaultValue(Guid.Empty);
            builder.Property<Guid>(r => r.CheckOutPerson).HasColumnName("CheckOutPerson").IsRequired().HasDefaultValue(Guid.Empty);
            builder.HasOne(x => x.CheckInCheckList).WithOne(x => x.Booking).HasForeignKey<CheckInCheckList>(x => x.BookingId);
            builder.HasMany(r => r.Complaints).WithOne(r => r.Booking).HasForeignKey(r => r.BookingId);
        }
    }
}
