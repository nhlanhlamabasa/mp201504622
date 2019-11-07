using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using Web.Helpers;
using Web.Interfaces;

namespace Web.Models
{
    public class UserClaims : IUserClaims
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserClaims(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetClaim(string claimtype)
        {
            foreach (Claim claim in _contextAccessor.HttpContext.User.Claims)
            {
                if (claim.Type.ToString() == claimtype)
                {
                    return claim.Value;
                }
            }
            throw new Exception($"{claimtype} was not found");
        }

        public bool IsManager()
        {
            return Claims.IsManager(_contextAccessor.HttpContext.User.Claims);
        }

        public bool IsFrontDesk()
        {
            return Claims.IsFrontDesk(_contextAccessor.HttpContext.User.Claims);
        }
    }
}
