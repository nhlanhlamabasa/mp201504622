using System.ComponentModel.DataAnnotations;

namespace IdentityServer.API.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
