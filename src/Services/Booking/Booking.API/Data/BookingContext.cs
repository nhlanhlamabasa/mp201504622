namespace Booking.API.Data
{
#pragma warning disable
    using Booking.API.Configurations;
    using Booking.API.Entities;
    using Booking.API.Extensions;
    using Microsoft.EntityFrameworkCore;

    public class BookingContext : DbContext
    {
        public BookingContext() { }

        public BookingContext(DbContextOptions<BookingContext> options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<RoomBooking> RoomBookings { get; set; }

        public DbSet<Amenity> Amenities { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Line> Lines { get; set; }

        public DbSet<RoomTypeEnum> RoomTypeEnums { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<RoomAmenity> RoomAmenities { get; set; }

        public DbSet<CheckInCheckList> CheckInCheckLists { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new HotelConfiguration());
            builder.ApplyConfiguration(new BookingConfiguration());
            builder.ApplyConfiguration(new RoomConfiguration());
            builder.ApplyConfiguration(new RoomBookingConfiguration());
            builder.ApplyConfiguration(new AmenityConfiguration());
            builder.ApplyConfiguration(new LineConfiguration());
            builder.ApplyConfiguration(new InvoiceConfiguration());
            builder.ApplyConfiguration(new RoomTypeEnumConfiguration());
            builder.ApplyConfiguration(new PaymentConfiguration());
            builder.ApplyConfiguration(new RoomAmenityConfiguration());
            builder.ApplyConfiguration(new CheckInCheckListConfiguration());
            builder.ApplyConfiguration(new ComplaintConfiguration());
            builder.Seed();
        }
    }
}
