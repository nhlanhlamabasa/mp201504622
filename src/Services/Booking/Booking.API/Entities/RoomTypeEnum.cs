namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;

    /// <summary>
    /// Defines the <see cref="RoomTypeEnum" />
    /// </summary>
    public class RoomTypeEnum : Entity<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoomTypeEnum"/> class.
        /// </summary>
        public RoomTypeEnum() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomTypeEnum"/> class.
        /// </summary>
        /// <param name="Id">The Id<see cref="int"/></param>
        /// <param name="Capacity">The Capacity<see cref="int"/></param>
        /// <param name="Cost">The Cost<see cref="decimal"/></param>
        /// <param name="Type">The Type<see cref="string"/></param>
        /// <param name="ImageName">The ImageName<see cref="string"/></param>
        /// <param name="ImageID">The ImageID<see cref="string"/></param>
        public RoomTypeEnum(int Id, int Capacity, decimal Cost, string Type, string ImageName, string ImageID) : base()
        {
            this.Id = Id;
            this.Capacity = Capacity;
            this.Cost = Cost;
            this.Type = Type;
            this.ImageName = ImageName;
            this.ImageID = ImageID;
        }

        /// <summary>
        /// Gets or sets the ImageID
        /// </summary>
        public string ImageID { get; set; }

        /// <summary>
        /// Gets or sets the Capacity
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the Cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the ImageName
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public string Type { get; set; }
    }
}
