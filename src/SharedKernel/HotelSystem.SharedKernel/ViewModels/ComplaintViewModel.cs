namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ComplaintViewModel
    {
        public BookingViewModel Booking { get; set; }

        public Guid BookingId { get; set; }

        [Display(Name = "Complaint Type")]
        public string ComplainType { get; set; }

        [Display(Name = "Decription of complaint")]
        public string Description { get; set; }

        [Display(Name = "Has it been resolved?")]
        public bool IsResolved { get; set; }

        [Display(Name = "Your level of satisfaction")]
        public string LevelOfSatisfaction { get; set; }

        public Guid Id { get; set; }

        public DateTime ModifiedDate { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
