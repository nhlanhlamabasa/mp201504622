namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="Hotel" />
    /// </summary>
    public class Hotel : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hotel"/> class.
        /// </summary>
        public Hotel() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hotel"/> class.
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <param name="Name">The Name<see cref="string"/></param>
        /// <param name="Rating">The Rating<see cref="Ratings"/></param>
        /// <param name="ImageName">The ImageName<see cref="string"/></param>
        /// <param name="ImageID">The ImageID<see cref="string"/></param>
        /// <param name="ContactDetails">The ContactDetails<see cref="ContactDetails"/></param>
        /// <param name="Address">The Address<see cref="Address"/></param>
        public Hotel(Guid Id, string Name, Ratings Rating, string ImageName, string ImageID, ContactDetails ContactDetails, Address Address) : base()
        {
            this.Id = Id;
            this.Name = Name;
            this.Rating = Rating.ToString();
            this.ImageName = ImageName;
            NumberOfRooms = 0;
            NumberOfBookings = 0;
            PostalCode = ContactDetails.PostalCode;
            Country = Address.Country;
            City = Address.City;
            Province = Address.Province;
            Street = Address.Street;
            this.ImageID = ImageID;
        }

        /// <summary>
        /// Gets or sets the City
        /// Gets the City
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the Country
        /// Gets the Country
        /// </summary>
        [Required]
        public string Country { get; set; }

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
        /// Gets or sets the ManagerId
        /// </summary>
        public Guid? ManagerId { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// Gets the Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the NumberOfBookings
        /// </summary>
        public int NumberOfBookings { get; set; }

        /// <summary>
        /// Gets or sets the NumberOfRooms
        /// Gets the NumberOfRooms
        /// </summary>
        public int NumberOfRooms { get; set; }

        /// <summary>
        /// Gets or sets the PostalCode
        /// Gets the PostalCode
        /// </summary>
        [Required]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the Province
        /// Gets the Province
        /// </summary>
        [Required]
        public string Province { get; set; }

        /// <summary>
        /// Gets or sets the Rating
        /// Gets the Rating
        /// </summary>
        [Required]
        public string Rating { get; set; }

        /// <summary>
        /// Gets or sets the Rooms
        /// Gets the Rooms
        /// </summary>
        [Required]
        public ICollection<Room> Rooms { get; set; }

        /// <summary>
        /// Gets the Street
        /// </summary>
        [Required]
        public string Street { get; private set; }

        /// <summary>
        /// The CalculateNumberOfBookings
        /// </summary>
        public void CalculateNumberOfBookings()
        {
            if (Rooms != null)
            {
                foreach (Room room in Rooms)
                {
                    if (room.RoomBookings != null)
                    {
                        NumberOfBookings += room.RoomBookings.Count;
                    }
                }
            }
        }

        /// <summary>
        /// The CalculateNumberOfRooms
        /// </summary>
        public void CalculateNumberOfRooms()
        {
            if (Rooms != null)
            {
                NumberOfRooms = Rooms.Count;
            }
        }
    }
}
