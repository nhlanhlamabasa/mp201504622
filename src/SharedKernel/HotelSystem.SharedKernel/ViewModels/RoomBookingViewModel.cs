namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Room Booking view model
    /// </summary>
    public class RoomBookingViewModel
    {
        public RoomBookingViewModel()
        {

        }

        /// <summary>
        /// Gets the Booking
        /// Booking
        /// </summary>
        [Required]
        [Display(Name = "Booking")]
        public BookingViewModel Booking { get; private set; }

        /// <summary>
        /// Gets the BookingId
        /// Booking id
        /// </summary>
        [Required]
        [Display(Name = "Booking Id")]
        public Guid BookingId { get; private set; }

        /// <summary>
        /// Gets the End
        /// Travel end date
        /// </summary>
        [Required]
        [Display(Name = "Departure")]
        public DateTime End { get; private set; }

        /// <summary>
        /// Gets the Room
        /// Room
        /// </summary>
        [Required]
        [Display(Name = "Room")]
        public RoomViewModel Room { get; private set; }

        /// <summary>
        /// Gets the RoomId
        /// Room id
        /// </summary>
        [Required]
        [Display(Name = "Room Id")]
        public Guid RoomId { get; private set; }

        /// <summary>
        /// Gets the Start
        /// Travel begin date
        /// </summary>
        [Required]
        [Display(Name = "Arrival")]
        public DateTime Start { get; private set; }

        public RoomBookingViewModel(BookingViewModel booking, RoomViewModel room)
        {
            Room = room;
            RoomId = room.Id;
            Booking = booking;
            BookingId = booking.Id;
            Start = booking.Arrival;
            End = booking.Departure;
        }

        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

    }
}
