namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    public class BookingViewModel
    {
        [Required]
        [Display(Name = "Arrival")]
        [DataType(DataType.DateTime)]
        public DateTime Arrival { get; set; }

        [Display(Name = "Booker Id")]
        public Guid BookerId { get; set; }

        [Required]
        [Display(Name = "Departure")]
        [DataType(DataType.DateTime)]
        public DateTime Departure { get; set; }
         
        [Required]
        [Display(Name = "Hotel Id")]
        public Guid HotelId { get; set; }

        [Required]
        [Display(Name = "Hotel Name")]
        public string HotelName { get; set; }

        [Display(Name = "Booking Id")]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Guests")]
        public int NumberOfGuests { get; set; }

        [Display(Name = "Booked Rooms")]
        public ICollection<RoomBookingViewModel> RoomBookings { get; set; }

        [Display(Name = "Selected Rooms")]
        public List<RoomViewModel> SelectedRooms { get; set; }

        public string Status { get; set; }

        [Display(Name = "Invoice")]
        public InvoiceViewModel Invoice { get; set; }

        [Display]
        public PaymentViewModel Payment { get; set; }

        public string TotalCost()
        {
            decimal Total = 0;
            foreach (RoomViewModel room in SelectedRooms)
            {
                Total += room.Cost;
            }
            return Total.ToString("C", new CultureInfo("en-ZA"));
        }

        [Required]
        [Display(Name = "Selected Rooms")]
        public int NumSelectedRooms { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        public BookingViewModel() { }

        public Guid CheckInPerson { get; set; }

        public Guid CheckOutPerson { get; set; }

        public CheckInCheckListViewModel CheckInCheckList { get; set; }

        public ICollection<ComplaintViewModel> Complaints { get; set; }

        public ComplaintViewModel NewComplaint { get; set; }
    }
}
