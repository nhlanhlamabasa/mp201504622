namespace Booking.API.Models
{
    /// <summary>
    /// Defines the <see cref="Clients" />
    /// </summary>
    public class Clients
    {
        /// <summary>
        /// Defines the <see cref="BaseUri" />
        /// </summary>
        public sealed class BaseUri
        {
            /// <summary>
            /// Defines the BookingClient
            /// </summary>
            public const string StripeClient = "https://api.stripe.com/v1";
        }

        /// <summary>
        /// Defines the <see cref="NamedClients" />
        /// </summary>
        public sealed class NamedClients
        {
            /// <summary>
            /// Defines the BookingClient
            /// </summary>
            public const string StripeClient = "Stripe";

        }
    }
}
