namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    /// <summary>
    /// Room view model
    /// </summary>
    public class RoomViewModel
    {
        /// <summary>
        /// Gets or sets the Amenities
        /// </summary>
        [Display(Name = "Amenities")]
        public ICollection<RoomAmenityViewModel> Amenities { get; set; }

        [Required]
        public string ImageID { get; set; }

        /// <summary>
        /// Gets or sets the Capacity
        /// </summary>
        [Required]
        [Display(Name = "Number of Beds")]
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the Cost
        /// Room cost
        /// </summary>
        [Required]
        [Display(Name = "Cost")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the CreationTime
        /// </summary>
        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the HotelId
        /// Hotel id
        /// </summary>
        [Required]
        [Display(Name = "Hotel Id")]
        public Guid HotelId { get; set; }

        [Required()]
        [Display(Name = "Hotel Name")]
        public string HotelName {get; set;}

        [Required]
        public string ImageName { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// Room id
        /// </summary>
        [Required]
        [Display(Name = "Room Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDate
        /// </summary>
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the RoomBookings
        /// </summary>
        public ICollection<RoomBookingViewModel> RoomBookings { get; set; }

        /// <summary>
        /// Gets or sets the RoomNumber
        /// Room number
        /// </summary>
        [Required]
        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }

        /// <summary>
        /// Gets or sets the RoomType
        /// Room type
        /// </summary>
        [Required]
        [Display(Name = "Room Type")]
        public string RoomType { get; set; }

        /// <summary>
        /// Gets the StrCost
        /// </summary>
        public string StrCost => Cost.ToString("C", new CultureInfo("en-ZA"));

        public RoomViewModel() { }
    }
}
