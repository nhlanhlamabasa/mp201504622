namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="Amenity" />
    /// </summary>
    public class Amenity : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Amenity"/> class.
        /// </summary>
        public Amenity() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Amenity"/> class.
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <param name="Description">The Description<see cref="string"/></param>
        /// <param name="ImageName">The ImageName<see cref="string"/></param>
        /// <param name="ImageID">The ImageID<see cref="string"/></param>
        public Amenity(Guid Id, string Description, string ImageName, string ImageID) : base()
        {
            this.Id = Id;
            this.Description = Description;
            this.ImageName = ImageName;
            this.ImageID = ImageID;
        }

        /// <summary>
        /// Gets or sets the Description
        /// Gets the Description
        /// </summary>
        [Required]
        public string Description { get; set; }

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
        /// Gets or sets the Rooms
        /// </summary>
        public ICollection<RoomAmenity> Rooms { get; set; }
    }
}
