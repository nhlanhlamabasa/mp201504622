namespace Web.Models
{
    public sealed class NamedClients
    {
        public const string BookingClient = "Booking.API";
        public const string IdentityClient = "IdentityServer";
    }

    public sealed class BaseUri
    {
        public const string BookingClient = "https://localhost:5003";
        public const string IdentityClient = "https://localhost:5001";
    }

    public sealed class Schemes
    {
        public const string AuthSchemes = "Cookies" + "," + "oidc";
        public const string Cookies = "Cookies";
        public const string Oidc = "oidc";
    }
}
