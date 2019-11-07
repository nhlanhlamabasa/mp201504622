namespace HotelSystem.SharedKernel.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="PaymentViewModel" />
    /// </summary>
    public class PaymentViewModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the AmountPaid
        /// </summary>
        [Required]
        [Display(Name = "Amount Paid")]
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// Gets or sets the Invoice
        /// </summary>
        [Display(Name = "Invoice")]
        public InvoiceViewModel Invoice { get; set; }

        /// <summary>
        /// Gets or sets the InvoiceId
        /// </summary>
        [Required]
        [Display(Name = "Invoice Id")]
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the Payment_Method
        /// </summary>
        [Required]
        [Display(Name = "Payment Method")]
        public string Payment_Method { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Invalid credit card number")]
        [RegularExpression(@"\d{4}\s\d{4}\s\d{4}\s\d{4}", ErrorMessage = "Invalid credit card number")]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Expiration")]
        public string Expiration { get; set; }

        [Required]
        [StringLength(maximumLength: 4, MinimumLength = 3, ErrorMessage = "Invalid CVV number")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Required]
        [RegularExpression(@"\d+")]
        [StringLength(maximumLength:6, MinimumLength = 4, ErrorMessage = "Invalid postal code")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }
    }
}
