namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;

    /// <summary>
    /// Defines the <see cref="RoomAmenity" />
    /// </summary>
    public class RoomAmenity : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoomAmenity"/> class.
        /// </summary>
        public RoomAmenity() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomAmenity"/> class.
        /// </summary>
        /// <param name="room">The room<see cref="Room"/></param>
        /// <param name="amenity">The amenity<see cref="Amenity"/></param>
        public RoomAmenity(Room room, Amenity amenity) : base()
        {
            this.Room = room;
            this.Amenity = amenity;
            this.RoomId = room.Id;
            this.AmenityId = amenity.Id;

        }

        /// <summary>
        /// Gets or sets the Amenity
        /// </summary>
        public Amenity Amenity { get; set; }

        /// <summary>
        /// Gets or sets the AmenityId
        /// </summary>
        public Guid? AmenityId { get; set; }

        /// <summary>
        /// Gets or sets the Room
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// Gets or sets the RoomId
        /// </summary>
        public Guid? RoomId { get; set; }
    }
}
