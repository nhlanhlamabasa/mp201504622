using HotelSystem.SharedKernel.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.Helpers;
using Web.Interfaces;
using Web.Models;

namespace Web.Services
{
    public class IdentityClient : IIdentityClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _contextAccessor;

        public IdentityClient(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor)
        {
            _clientFactory = clientFactory;
            _contextAccessor = contextAccessor;
        }

        public async Task<HttpResponseMessage> GetBookerId(string email)
        {
            HttpClient client = CreateClient();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/identity?email={email}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetName(Guid bookerId)
        {
            HttpClient client = CreateClient();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/identity?bookerId={bookerId}&fullName={true}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> Register(RegisterViewModel model)
        {
            HttpClient client = CreateClient();
            StringContent content = CreateContent(model);
            HttpResponseMessage responseMessage = await client.PostAsync($"{client.BaseAddress}api/identity", content);
            return responseMessage;
        }

        private HttpClient CreateClient()
        {
            HttpClient client = _clientFactory.CreateClient(NamedClients.IdentityClient);
            return client;
        }

        private async Task<HttpClient> CreateClientWithToken()
        {
            string access_token = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            HttpClient client = _clientFactory.CreateClient(NamedClients.IdentityClient);
            client.SetBearerToken(access_token);
            return client;
        }

        private StringContent CreateContent(object data)
        {
            return new StringContent(Serialize.SerializeObj(data), Encoding.UTF8, "application/json");
        }
    }
}
