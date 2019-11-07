using HotelSystem.SharedKernel.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class ChangeGuestsViewModel
    {

        public ChangeGuestsViewModel(Guid BookingId, DateTime Arrival, DateTime Departure, Guid HotelId, 
            string HotelName, int NumberOfGuests, List<RoomViewModel> SelectedRooms)
        {
            this.BookingId = BookingId;
            this.Arrival = Arrival;
            this.Departure = Departure;
            this.HotelId = HotelId;
            this.HotelName = HotelName;
            this.NumberOfGuests = NumberOfGuests;
            this.ModifiedDate = DateTime.UtcNow;
            this.SelectedRooms = SelectedRooms;
        }

        [Required]
        public Guid BookingId { get; set; }

        [Required]
        [Display(Name = "Arrival")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Arrival { get; set; } = new DateTime();

        [Required]
        [Display(Name = "Departure")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Departure { get; set; } = new DateTime();

        [Required]
        [Display(Name = "Hotel Name")]
        public Guid HotelId { get; set; }

        [Required]
        [Display(Name = "Hotel Name")]
        public string HotelName { get; set; }

        [Required]
        [Display(Name = "Number of Guests")]
        public int NumberOfGuests { get; set; }

        [Required]
        [Display(Name = "New Number of Guests")]
        public int NewNumberOfGuests { get; set; }

        [Required]
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; } = new DateTime();

        [Required]
        [Display(Name = "Selected Rooms")]
        public List<RoomViewModel> SelectedRooms { get; set; } = new List<RoomViewModel>();
    }
}
