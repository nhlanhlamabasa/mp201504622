namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CheckInOutViewModel
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Arrival { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Departure { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public Guid BookerId { get; set; }

        [Required]
        public bool CheckIn { get; set; }

        [Required]
        public bool CheckOut { get; set; }
    }
}
