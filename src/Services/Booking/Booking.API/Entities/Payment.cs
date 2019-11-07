namespace Booking.API.Entities
{
    using HotelSystem.SharedKernel;
    using System;

    /// <summary>
    /// Defines the <see cref="Payment" />
    /// </summary>
    public class Payment : Entity<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Payment"/> class.
        /// </summary>
        public Payment() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Payment"/> class.
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <param name="InvoiceId">The InvoiceId<see cref="Guid"/></param>
        /// <param name="Payment_Method">The Payment_Method<see cref="string"/></param>
        /// <param name="AmountPaid">The AmountPaid<see cref="decimal"/></param>
        public Payment(Guid Id, Guid InvoiceId, string Payment_Method, decimal AmountPaid)
        {
            this.Id = Id;
            this.InvoiceId = InvoiceId;
            this.Payment_Method = Payment_Method;
        }

        /// <summary>
        /// Gets or sets the AmountPaid
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Gets or sets the Invoice
        /// </summary>
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceId
        /// </summary>
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the Payment_Method
        /// </summary>
        public string Payment_Method { get; set; }
    }
}
