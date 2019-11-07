namespace HotelSystem.SharedKernel.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="RegisterViewModel" />
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the City
        /// </summary>
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the ConfirmPassword
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [Required]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNumber
        /// </summary>
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the PostalCode
        /// </summary>
        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the Province
        /// </summary>
        [Required]
        [Display(Name = "Province")]
        public string Province { get; set; }

        /// <summary>
        /// Gets or sets the Street
        /// </summary>
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Street Address")]
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the Username
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
    }
}
