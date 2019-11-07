using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.API.Configuration
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("booking_api", "Booking API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "swaggerui",
                    ClientName = "Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {"https://localhost:5003/oauth2-redirect.html"},
                    AllowedScopes = new List<string>
                    {
                        "booking_api"
                    }
                },

                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientUri = "https://localhost:5007",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    RequireConsent = false,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:5007/signin-oidc",
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:5007/signout-callback-oidc"
                    },
                    AllowedScopes =  new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "booking_api"
                        
                    },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var customProfile = new IdentityResource(
            name: "custom.profile",
            displayName: "Custom Profile",
            claimTypes: new[] { JwtClaimTypes.Role })
            {
                ShowInDiscoveryDocument = true,
                Required = true,
                Enabled = true,
                Description = "Role claims"
            };

            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                customProfile
            };
        }

        public static IEnumerable<object> GetResources()
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<object> GetApis()
        {
            throw new NotImplementedException();
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser{SubjectId = "818727", Username = "alice", Password = "alice",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServerConstants.ClaimValueTypes.Json)
                    }
                },
                new TestUser{SubjectId = "88421113", Username = "bob", Password = "bob",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("location", "somewhere")
                    }
                }
            };
        }
    }
}
