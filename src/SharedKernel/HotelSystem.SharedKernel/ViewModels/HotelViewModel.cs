namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// ViewModel for hotel
    /// </summary>
    public class HotelViewModel
    {
        /// <summary>
        /// Gets or sets the City
        /// </summary>
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the Country
        /// Hotel address
        /// </summary>
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// Hotel Id
        /// </summary>
        [Required]
        [Display(Name = "Hotel Id")]
        public Guid Id { get; set; }

        [Required]
        public string ImageID { get; set; }

        /// <summary>
        /// Gets or sets the ManagerId
        /// Hotel Manager id
        /// </summary>
        [Display(Name = "Manager Id")]
        public Guid? ManagerId { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// Hotel Name
        /// </summary>
        [Required]
        [Display(Name = "Hotel Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the PostalCode
        /// Hotel contact details
        /// </summary>
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the Province
        /// </summary>
        [Required]
        [Display(Name = "Province")]
        public string Province { get; set; }

        /// <summary>
        /// Gets or sets the Rating
        /// Hotel ratings
        /// </summary>
        [Required]
        [Display(Name = "Rating")]
        public Ratings Rating { get; set; }

        /// <summary>
        /// Gets or sets the Rooms
        /// </summary>
        [Display(Name = "Rooms")]
        public ICollection<RoomViewModel> Rooms { get; set; }

        /// <summary>
        /// Gets or sets the Street
        /// </summary>
        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        public int NumberOfRooms {get; set;}

        public int NumberOfBookings { get; set; }

        [Required]
        public string ImageName { get; set; }

        /// <summary>
        /// The CalculateNumberOfRooms
        /// </summary>
        public void CalculateNumberOfRooms()
        {
            if (Rooms != null)
            {
                NumberOfRooms = Rooms.Count;
            }
        }

        /// <summary>
        /// The CalculateNumberOfBookings
        /// </summary>
        public void CalculateNumberOfBookings()
        {
            if (Rooms != null)
            {
                foreach (RoomViewModel room in Rooms)
                {
                    if (room.RoomBookings != null)
                    {
                        NumberOfBookings += room.RoomBookings.Count;
                    }
                }
            }
        }
    }
}
