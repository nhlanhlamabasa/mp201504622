namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;

    /// <summary>
    /// Defines the <see cref="Line" />
    /// </summary>
    public class Line : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        public Line() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <param name="InvoiceId">The InvoiceId<see cref="Guid"/></param>
        /// <param name="Line_Price">The Line_Price<see cref="decimal"/></param>
        /// <param name="Line_Units">The Line_Units<see cref="int"/></param>
        /// <param name="Room_Type">The Room_Type<see cref="string"/></param>
        public Line(Guid Id, Guid InvoiceId, decimal Line_Price, int Line_Units, string Room_Type) : base()
        {
            this.Id = Id;
            this.InvoiceId = InvoiceId;
            this.Line_Price = Line_Price;
            this.Line_Units = Line_Units;
            this.Room_Type = Room_Type;
        }

        /// <summary>
        /// Gets or sets the Invoice
        /// </summary>
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceId
        /// </summary>
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the Line_Price
        /// </summary>
        public decimal Line_Price { get; set; }

        /// <summary>
        /// Gets or sets the Line_Units
        /// </summary>
        public int Line_Units { get; set; }

        /// <summary>
        /// Gets or sets the Room_Type
        /// </summary>
        public string Room_Type { get; set; }
    }
}
