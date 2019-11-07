namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="Room" />
    /// </summary>
    public class Room : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        public Room() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <param name="RoomNumber">The RoomNumber<see cref="int"/></param>
        /// <param name="type">The type<see cref="RoomTypeEnum"/></param>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="RoomBookings">The RoomBookings<see cref="ICollection{RoomBooking}"/></param>
        /// <param name="Amenities">The Amenities<see cref="ICollection{RoomAmenity}"/></param>
        /// <param name="HotelName">The HotelName<see cref="string"/></param>
        public Room(Guid Id, int RoomNumber, RoomTypeEnum type, Guid HotelId, ICollection<RoomBooking> RoomBookings, ICollection<RoomAmenity> Amenities, string HotelName) : base()
        {
            this.Id = Id;
            this.HotelId = HotelId;
            this.RoomNumber = RoomNumber;
            this.RoomBookings = RoomBookings;
            this.Amenities = Amenities;
            this.RoomType = type.Type;
            this.Capacity = type.Capacity;
            this.Cost = type.Cost;
            this.HotelName = HotelName;
            this.ImageName = type.ImageName;
            this.ImageID = type.ImageID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <param name="RoomNumber">The RoomNumber<see cref="int"/></param>
        /// <param name="type">The type<see cref="RoomTypeEnum"/></param>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="HotelName">The HotelName<see cref="string"/></param>
        public Room(Guid Id, int RoomNumber, RoomTypeEnum type, Guid HotelId, string HotelName) : base()
        {
            this.Id = Id;
            this.HotelId = HotelId;
            this.RoomNumber = RoomNumber;
            this.RoomType = type.Type;
            this.Capacity = type.Capacity;
            this.Cost = type.Cost;
            this.HotelName = HotelName;
            this.ImageName = type.ImageName;
            this.ImageID = type.ImageID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <param name="RoomNumber">The RoomNumber<see cref="int"/></param>
        /// <param name="type">The type<see cref="RoomTypeEnum"/></param>
        /// <param name="Hotel">The Hotel<see cref="Hotel"/></param>
        public Room(Guid Id, int RoomNumber, RoomTypeEnum type, Hotel Hotel) : base()
        {
            this.Id = Id;
            this.RoomNumber = RoomNumber;
            this.RoomType = type.Type;
            this.HotelId = Hotel.Id;
            this.RoomType = type.Type;
            this.Capacity = type.Capacity;
            this.Cost = type.Cost;
            this.HotelName = Hotel.Name;
            this.ImageName = type.ImageName;
            this.ImageID = type.ImageID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        /// <param name="room">The room<see cref="Room"/></param>
        public Room(Room room)
        {
            Amenities = room.Amenities;
            Capacity = room.Capacity;
            Cost = room.Cost;
            Hotel = room.Hotel;
            HotelId = room.HotelId;
            RoomBookings = room.RoomBookings;
            RoomNumber = room.RoomNumber;
            RoomType = room.RoomType;
            HotelName = room.HotelName;
            ImageName = room.ImageName;
            ImageID = room.ImageID;
            CreationTime = room.CreationTime;
            ModifiedDate = room.ModifiedDate;
        }

        /// <summary>
        /// Gets or sets the Amenities
        /// </summary>
        public ICollection<RoomAmenity> Amenities { get; set; }

        /// <summary>
        /// Gets or sets the Capacity
        /// </summary>
        [Required]
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the Cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the Hotel
        /// </summary>
        [Required]
        public Hotel Hotel { get; set; }

        /// <summary>
        /// Gets or sets the HotelId
        /// </summary>
        [Required]
        public Guid HotelId { get; set; }

        /// <summary>
        /// Gets or sets the HotelName
        /// </summary>
        [Required]
        public string HotelName { get; set; }

        /// <summary>
        /// Gets or sets the ImageID
        /// </summary>
        [Required]
        public string ImageID { get; set; }

        /// <summary>
        /// Gets or sets the ImageName
        /// </summary>
        [Required]
        public string ImageName { get; set; }

        /// <summary>
        /// Gets or sets the RoomBookings
        /// </summary>
        public ICollection<RoomBooking> RoomBookings { get; set; }

        /// <summary>
        /// Gets or sets the RoomNumber
        /// </summary>
        [Required]
        public int RoomNumber { get; set; }

        /// <summary>
        /// Gets or sets the RoomType
        /// </summary>
        [Required]
        public string RoomType { get; set; }
    }
}
