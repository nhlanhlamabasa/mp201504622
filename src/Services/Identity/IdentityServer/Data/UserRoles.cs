namespace IdentityServer.API.Data
{
    public static class UserRoles
    {
        public static string ADMIN { get; } = new string("ADMIN");
        public static string GUEST { get; } = new string("GUEST");
        public static string MANAGER { get; } = new string("MANAGER");
        public static string FRONTDESK { get; } = new string("FRONTDESK");
    }
}
