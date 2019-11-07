namespace HotelSystem.SharedKernel.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="ForgotPasswordViewModel" />
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        [Required(ErrorMessage = "Please enter your email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }
}
