namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Amenity view model
    /// </summary>
    public class AmenityViewModel
    {
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// Amenity Id
        /// </summary>
        [Required]
        [Display(Name = "Amenity Id")]
        public Guid Id { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [Required]
        public string ImageName { get; set; }

        [Required]
        public string ImageID { get; set; }

        [Display(Name = "Rooms")]
        public ICollection<RoomAmenityViewModel> Rooms { get; set; }
    }
}
