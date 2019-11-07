using System;
using System.ComponentModel.DataAnnotations;

namespace HotelSystem.SharedKernel.ViewModels
{
    public class PeriodSummary
    {   

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Display(Name = "Value of Payment Received")]
        public decimal TotalPayments { get; set; }

        public Guid MostBookedHotelID { get; set; }

        [Display(Name = "Most Popular Hotel")]
        public string MostBookedHotel { get; set; }

        [Display(Name = "Least Popular Hotel")]
        public string LeastBookedHotel { get; set; }

        public Guid LeastBookedHotelID { get; set; }

        public PeriodSummary(DateTime Start, DateTime End, decimal TotalPayments, Guid MostBookedHotelID, string MostBookedHotel, 
            string LeastBookedHotel, Guid LeastBookedHotelID)
        {
            this.Start = Start;
            this.End = End;
            this.TotalPayments = TotalPayments;
            this.MostBookedHotelID = MostBookedHotelID;
            this.MostBookedHotel = MostBookedHotel;
            this.LeastBookedHotel = LeastBookedHotel;
            this.LeastBookedHotelID = LeastBookedHotelID;
        }
    }
}
