namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    /// <summary>
    /// Defines the <see cref="RoomAmenityViewModel" />
    /// </summary>
    public class RoomAmenityViewModel
    {
        /// <summary>
        /// Gets or sets the Amenity
        /// </summary>
        public AmenityViewModel Amenity { get; set; }

        /// <summary>
        /// Gets or sets the AmenityId
        /// </summary>
        public Guid AmenityId { get; set; }

        /// <summary>
        /// Gets or sets the Room
        /// </summary>
        public RoomViewModel Room { get; set; }

        /// <summary>
        /// Gets or sets the RoomId
        /// </summary>
        public Guid RoomId { get; set; }
    }
}
