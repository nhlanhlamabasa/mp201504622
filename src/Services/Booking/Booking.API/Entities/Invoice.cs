namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="Invoice" />
    /// </summary>
    public class Invoice : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Invoice"/> class.
        /// </summary>
        public Invoice() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Invoice"/> class.
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <param name="AppUser_BookerId">The AppUser_BookerId<see cref="Guid"/></param>
        /// <param name="BookingId"></param>
        public Invoice(Guid Id, Guid AppUser_BookerId, Guid BookingId) : base()
        {
            this.Id = Id;
            this.AppUser_BookerId = AppUser_BookerId;
            this.BookingId = BookingId;
        }

        /// <summary>
        /// Gets or sets the AppUser_BookerId
        /// </summary>
        public Guid AppUser_BookerId { get; set; }

        /// <summary>
        /// Gets or sets the Booking
        /// </summary>
        public Booking Booking { get; set; }

        /// <summary>
        /// Gets or sets the BookingId
        /// </summary>
        public Guid BookingId { get; set; }

        /// <summary>
        /// Gets or sets the Inv_Date
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inv_Date { get; set; }

        /// <summary>
        /// Gets or sets the Lines
        /// </summary>
        public ICollection<Line> Lines { get; set; }

        /// <summary>
        /// Gets or sets the Payment
        /// </summary>
        public Payment Payment { get; set; }

        /// <summary>
        /// The Total
        /// </summary>
        /// <returns>The <see cref="decimal"/></returns>
        public decimal Total()
        {
            decimal Total = 0;
            if (Lines != null)
            {
                foreach (Line line in Lines)
                {
                    Total += line.Line_Price * line.Line_Units;
                }
            }
            return Total;
        }
    }
}
