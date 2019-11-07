using HotelSystem.SharedKernel.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Web.Interfaces
{
    public interface IIdentityClient
    {
        Task<HttpResponseMessage> Register(RegisterViewModel model);

        Task<HttpResponseMessage> GetName(Guid bookerId);

        Task<HttpResponseMessage> GetBookerId(string email);
    }
}
