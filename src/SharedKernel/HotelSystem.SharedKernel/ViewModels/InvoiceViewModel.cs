namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="InvoiceViewModel" />
    /// </summary>
    public class InvoiceViewModel
    {
        /// <summary>
        /// Gets or sets the AppUser_BookerId
        /// </summary>
        [Required]
        [Display(Name = "Booker Id")]
        public Guid AppUser_BookerId { get; set; }

        /// <summary>
        /// Gets or sets the BookingId
        /// </summary>
        public Guid BookingId { get; set; }

        /// <summary>
        /// Gets or sets the CreationTime
        /// </summary>
        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Required]
        [Display(Name = "Invoice Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Inv_Date
        /// </summary>
        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime Inv_Date { get; set; }

        /// <summary>
        /// Gets or sets the Lines
        /// </summary>
        [Required]
        [Display(Name = "Invoice Lines")]
        public List<LineViewModel> Lines { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDate
        /// </summary>
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// The Total
        /// </summary>
        /// <returns>The <see cref="decimal"/></returns>
        public decimal Total()
        {
            decimal Total = 0;
            foreach (LineViewModel line in Lines)
            {
                Total += line.Total();
            }
            return Total;
        }
    }
}
