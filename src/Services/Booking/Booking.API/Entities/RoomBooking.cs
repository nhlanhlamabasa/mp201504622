namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="RoomBooking" />
    /// </summary>
    public class RoomBooking : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoomBooking"/> class.
        /// </summary>
        /// <param name="room">The room<see cref="Room"/></param>
        /// <param name="booking">The booking<see cref="Booking"/></param>
        public RoomBooking(Room room, Booking booking) : base()
        {
            RoomId = room.Id;
            Room = room;
            BookingId = booking.Id;
            Booking = booking;
            Arrival = booking.Arrival;
            Departure = booking.Departure;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="RoomBooking"/> class from being created.
        /// </summary>
        public RoomBooking() : base()
        {
        }

        /// <summary>
        /// Gets the Arrival
        /// </summary>
        [Required]
        public DateTime Arrival { get; set; }

        /// <summary>
        /// Gets the Booking
        /// </summary>
        [Required]
        public Booking Booking { get; set; }

        /// <summary>
        /// Gets the BookingId
        /// </summary>
        [Required]
        public Guid BookingId { get; set; }

        /// <summary>
        /// Gets the Departure
        /// </summary>
        [Required]
        public DateTime Departure { get; set; }

        /// <summary>
        /// Gets the Room
        /// </summary>
        [Required]
        public Room Room { get; set; }

        /// <summary>
        /// Gets the RoomId
        /// </summary>
        [Required]
        public Guid RoomId { get;  set; }
    }
}
