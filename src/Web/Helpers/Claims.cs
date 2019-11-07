namespace Web.Helpers
{
    using IdentityModel;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;

    /// <summary>
    /// Defines the <see cref="Claims" />
    /// </summary>
    public static class Claims
    {
        /// <summary>
        /// The GetUserID
        /// </summary>
        /// <param name="claims">The claims<see cref="IEnumerable{Claim}"/></param>
        /// <returns>The <see cref="Guid"/></returns>
        public static Guid GetUserID(IEnumerable<Claim> claims)
        {
            Guid bookerId = Guid.Empty;
            foreach (Claim claim in claims)
            {
                if (claim.Type == JwtClaimTypes.Subject)
                {
                    bookerId = Guid.Parse(claim.Value);
                    break;
                }
            }
            return bookerId;
        }

        public static string GetUserEmail(IEnumerable<Claim> claims)
        {
            string email = string.Empty;
            foreach (Claim claim in claims)
            {
                if (claim.Type == JwtClaimTypes.Email)
                {
                    email = claim.Value;
                    break;
                }
            }
            return email;
        }

        public static bool IsManager(IEnumerable<Claim> claims)
        {
            bool isManager = false;

            foreach (Claim claim in claims)
            {
                if (claim.Type == JwtClaimTypes.Role)
                {
                    if(claim.Value == "MANAGER")
                    {
                        isManager = true;
                    }
                    break;
                }
            }

            return isManager;
        }

        public static bool IsFrontDesk(IEnumerable<Claim> claims)
        {
            bool isFrontDesk = false;

            foreach (Claim claim in claims)
            {
                if (claim.Type == JwtClaimTypes.Role)
                {
                    if (claim.Value == "FRONTDESK")
                    {
                        isFrontDesk = true;
                    }
                    break;
                }
            }

            return isFrontDesk;
        }

        public static string GetUserFirstName(IEnumerable<Claim> claims)
        {
            string email = string.Empty;
            foreach (Claim claim in claims)
            {
                if (claim.Type == JwtClaimTypes.Name)
                {
                    email = claim.Value;
                    break;
                }
            }
            return email;
        }

        public static string GetUserLastName(IEnumerable<Claim> claims)
        {
            string email = string.Empty;
            foreach (Claim claim in claims)
            {
                if (claim.Type == JwtClaimTypes.FamilyName)
                {
                    email = claim.Value;
                    break;
                }
            }
            return email;
        }
    }
}
