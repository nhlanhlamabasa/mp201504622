namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="LineViewModel" />
    /// </summary>
    public class LineViewModel
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Required]
        [Display(Name = "Line Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Inv_Id
        /// </summary>
        [Required]
        [Display(Name = "Invoice Id")]
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the Invoice
        /// </summary>
        public InvoiceViewModel Invoice { get; set; }

        /// <summary>
        /// Gets or sets the Line_Price
        /// </summary>
        [Display(Name = "Line Price")]
        public decimal Line_Price { get; set; }

        /// <summary>
        /// Gets or sets the Line_Units
        /// </summary>
        [Display(Name = "Line Units")]
        public int Line_Units { get; set; }

        /// <summary>
        /// Gets or sets the Room_Type
        /// </summary>
        [Display(Name = "Room Type")]
        [Required]
        public string Room_Type { get; set; }

        public decimal Total()
        {
            return Line_Price * Line_Units;
        }

        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}
