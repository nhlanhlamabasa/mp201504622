namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;

    /// <summary>
    /// Defines the <see cref="Complaint" />
    /// </summary>
    public class Complaint : Entity<Guid>
    {
        /// <summary>
        /// Gets or sets the Booking
        /// </summary>
        public Booking Booking { get; set; }

        /// <summary>
        /// Gets or sets the BookingId
        /// </summary>
        public Guid BookingId { get; set; }

        /// <summary>
        /// Gets or sets the ComplainType
        /// </summary>
        public string ComplainType { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsResolved
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        /// Gets or sets the LevelOfSatisfaction
        /// </summary>
        public string LevelOfSatisfaction { get; set; }
    }
}
