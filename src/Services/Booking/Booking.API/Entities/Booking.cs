namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="Booking" />
    /// </summary>
    public class Booking : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Booking"/> class.
        /// </summary>
        public Booking() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Booking"/> class.
        /// </summary>
        /// <param name="HotelName">Hotel Name</param>
        /// <param name="Arrival">The Arrival<see cref="DateTime"/></param>
        /// <param name="Departure">The Departure<see cref="DateTime"/></param>
        /// <param name="NumberOfGuests">The NumberOfGuests<see cref="int"/></param>
        /// <param name="BookerId">The BookerId<see cref="Guid"/></param>
        /// <param name="SelectedRooms"></param>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        public Booking(string HotelName, DateTime Arrival, DateTime Departure, int NumberOfGuests, Guid BookerId,
            ICollection<Room> SelectedRooms,
            Guid HotelId)
            : base()
        {
            this.HotelName = HotelName;
            this.Id = Guid.NewGuid();
            this.NumberOfGuests = NumberOfGuests;
            this.BookerId = BookerId;
            this.Arrival = Arrival;
            this.Departure = Departure;
            Status = BookingStatus.AwaitingPayment.ToString();
            this.SelectedRooms = SelectedRooms;
            this.HotelId = HotelId;
            CheckInPerson = Guid.Empty;
            CheckOutPerson = Guid.Empty;
        }

        /// <summary>
        /// Gets or sets the Arrival
        /// Gets the Arrival
        /// </summary>
        [Required]
        public DateTime Arrival { get; set; }

        /// <summary>
        /// Gets or sets the BookerId
        /// Gets the BookerId
        /// </summary>
        [Required]
        public Guid BookerId { get; set; }

        /// <summary>
        /// Gets or sets the CheckInCheckList
        /// </summary>
        public CheckInCheckList CheckInCheckList { get; set; }

        /// <summary>
        /// Gets or sets the CheckInPerson
        /// </summary>
        public Guid CheckInPerson { get; set; }

        /// <summary>
        /// Gets or sets the CheckOutPerson
        /// </summary>
        public Guid CheckOutPerson { get; set; }

        /// <summary>
        /// Gets or sets the Complaints
        /// </summary>
        public ICollection<Complaint> Complaints { get; set; }

        /// <summary>
        /// Gets or sets the Departure
        /// Gets the Departure
        /// </summary>
        [Required]
        public DateTime Departure { get; set; }

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
        /// Gets or sets the Invoice
        /// </summary>
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the NumberOfGuests
        /// Gets the NumberOfGuests
        /// </summary>
        [Required]
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// Gets or sets the RoomBookings
        /// Gets the RoomBookings
        /// </summary>
        [Required]
        public ICollection<RoomBooking> RoomBookings { get; set; } = new List<RoomBooking>();

        /// <summary>
        /// Gets or sets the SelectedRooms
        /// </summary>
        [NotMapped]
        public ICollection<Room> SelectedRooms { get; set; }

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// The AddRoom
        /// </summary>
        /// <param name="room">The room<see cref="Room"/></param>
        public void AddRoom(Room room)
        {
            this.RoomBookings.Add(new RoomBooking(room, this));
        }

        /// <summary>
        /// The setID
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        public void setID(Guid Id)
        {
            this.Id = Id;
        }
    }
}
