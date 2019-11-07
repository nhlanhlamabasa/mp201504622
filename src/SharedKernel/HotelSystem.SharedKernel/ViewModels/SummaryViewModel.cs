using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HotelSystem.SharedKernel.ViewModels
{
    public class SummaryViewModel
    {
        [Display(Name = "Total Number of Hotels")]
        public int TotalNumberOfHotels { get; set; }

        [Display(Name = "Total Number of Rooms")]
        public int TotalNumberOfRooms { get; set; }

        [Display(Name = "Total Number of Amenities")]
        public int TotalNumberOfAmenities { get; set; }

        [Display(Name = "Value of Payments Received")]
        public decimal TotalOfValueOfPayments { get; set; }
    }
}
