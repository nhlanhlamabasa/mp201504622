namespace HotelSystem.SharedKernel.Models
{
    /// <summary>
    /// Defines the <see cref="POP3EmailConfig" />
    /// </summary>
    public class POP3EmailConfig
    {
        /// <summary>
        /// Gets or sets the Auth
        /// </summary>
        public string[] Auth { get; set; }

        /// <summary>
        /// Gets or sets the Hots
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Ports
        /// </summary>
        public string[] Ports { get; set; }

        /// <summary>
        /// Gets or sets the TLS
        /// </summary>
        public string TLS { get; set; }

        /// <summary>
        /// Gets or sets the Username
        /// </summary>
        public string Username { get; set; }
    }
}
