namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;

    /// <summary>
    /// Defines the <see cref="CheckInCheckList" />
    /// </summary>
    public class CheckInCheckList : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckInCheckList"/> class.
        /// </summary>
        public CheckInCheckList() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckInCheckList"/> class.
        /// </summary>
        /// <param name="KeysReceived">The KeysReceived<see cref="bool"/></param>
        /// <param name="AllAmenities">The AllAmenities<see cref="bool"/></param>
        /// <param name="PrintedRecepiet">The PrintedRecepiet<see cref="bool"/></param>
        /// <param name="BookingId">The BookingId<see cref="Guid"/></param>
        public CheckInCheckList(bool KeysReceived, bool AllAmenities, bool PrintedRecepiet, Guid BookingId) : base()
        {
            this.KeysReceived = KeysReceived;
            this.AllAmenities = AllAmenities;
            this.PrintedRecepiet = PrintedRecepiet;
            this.BookingId = BookingId;
        }

        /// <summary>
        /// Gets or sets a value indicating whether AllAmenities
        /// </summary>
        public bool AllAmenities { get; set; }

        /// <summary>
        /// Gets or sets the Booking
        /// </summary>
        public Booking Booking { get; set; }

        /// <summary>
        /// Gets or sets the BookingId
        /// </summary>
        public Guid BookingId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether KeysReceived
        /// </summary>
        public bool KeysReceived { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether PrintedRecepiet
        /// </summary>
        public bool PrintedRecepiet { get; set; }
    }
}
