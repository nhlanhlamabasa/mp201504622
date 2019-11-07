namespace HotelSystem.SharedKernel.Models
{
    /// <summary>
    /// Defines the <see cref="Email" />
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Gets or sets the Body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the From
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the Recepient
        /// </summary>
        public string Recepient { get; set; }

        /// <summary>
        /// Gets or sets the Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the Token
        /// </summary>
        public object Token { get; set; }
    }
}
