namespace HotelSystem.SharedKernel.ViewModels
{
    using System;

    public class CheckInCheckListViewModel
    {
        public bool AllAmenities { get; set; }

        public BookingViewModel Booking { get; set; }

        public Guid BookingId { get; set; }

        public Guid Id { get; set; }

        public bool KeysReceived { get; set; }

        public bool PrintedRecepiet { get; set; }
    }
}
