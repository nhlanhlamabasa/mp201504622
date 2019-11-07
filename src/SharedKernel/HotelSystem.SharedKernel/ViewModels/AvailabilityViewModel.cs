namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="AvailabilityViewModel" />
    /// </summary>
    public class AvailabilityViewModel
    {
        /// <summary>
        /// Gets or sets the Arrival
        /// </summary>
        [Required]
        [Display(Name = "Arrival")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Arrival { get; set; } = new DateTime();

        /// <summary>
        /// Gets or sets the Departure
        /// </summary>
        [Required]
        [Display(Name = "Departure")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Departure { get; set; } = new DateTime();

        /// <summary>
        /// Gets or sets the HotelId
        /// </summary>
        [Required]
        [Display(Name = "Hotel Name")]
        public Guid HotelId { get; set; }

        /// <summary>
        /// Gets or sets the HotelName
        /// </summary>
        public string HotelName { get; set; }

        /// <summary>
        /// Gets or sets the NumberOfGuests
        /// </summary>
        [Required]
        [Display(Name = "Number of Guests")]
        public int NumberOfGuests { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}
