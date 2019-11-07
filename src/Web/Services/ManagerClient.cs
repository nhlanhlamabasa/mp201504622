namespace Web.Services
{
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

    public class ManagerClient : IManagerClient
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly IHttpContextAccessor _contextAccessor;

        public ManagerClient(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor)
        {
            _clientFactory = clientFactory;
            _contextAccessor = contextAccessor;
        }

        public async Task<HttpResponseMessage> AddHotel(HotelViewModel hotel)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(hotel);
            HttpResponseMessage responseMessage = await client.PostAsync($"{client.BaseAddress}api/v1/manager/hotel", content);
            return responseMessage;
        }

        public async Task<HttpResponseMessage> AddRoom(Guid HotelId, RoomViewModel room)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(room);
            HttpResponseMessage responseMessage = await client.PostAsync($"{client.BaseAddress}api/v1/manager/hotel/{HotelId}/room", content);
            return responseMessage;
        }

        public async Task<HttpResponseMessage> EditHotel(Guid HotelId, HotelViewModel hotel)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(hotel);
            HttpResponseMessage responseMessage = await client.PutAsync($"{client.BaseAddress}api/v1/manager/hotel/{HotelId}", content);
            return responseMessage;
        }

        public async Task<HttpResponseMessage> EditRoom(Guid HotelId, Guid RoomId, RoomViewModel room)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(room);
            HttpResponseMessage responseMessage = await client.PutAsync($"{client.BaseAddress}api/v1/manager/hotel/{HotelId}/room" +
                $"/{RoomId}", content);
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetHotel(Guid HotelId, bool WithDetails = false)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/manager/hotel/{HotelId}?" +
                $"WithDetails={WithDetails}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetHotels(Guid ManagerId)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/manager/hotel");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetRoom(Guid HotelId, Guid RoomId, bool WithDetails = false)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/manager/hotel/{HotelId}/room" +
                $"/{RoomId}?WithDetails={WithDetails}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetRooms(Guid HotelId)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/manager/hotel/" +
                $"{HotelId}/room");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> RemoveRoom(Guid HotelId, Guid RoomId)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.DeleteAsync($"{client.BaseAddress}api/v1/manager/hotel/" +
                $"{HotelId}/room");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> RemoveRoom(Guid HotelId, Guid RoomId, RoomViewModel room)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.DeleteAsync($"{client.BaseAddress}api/v1/manager/hotel/" +
                $"{HotelId}/room");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> RemoveHotel(Guid HotelId)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.DeleteAsync($"{client.BaseAddress}api/v1/manager/hotel/{HotelId}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> RemoveHotel(Guid HotelId, HotelViewModel hotel)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.DeleteAsync($"{client.BaseAddress}api/v1/manager/hotel/{HotelId}");
            return responseMessage;
        }

        private async Task<HttpClient> CreateClientWithToken()
        {
            string access_token = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            HttpClient client = _clientFactory.CreateClient(NamedClients.BookingClient);
            client.SetBearerToken(access_token);
            return client;
        }

        private StringContent CreateContent(object data)
        {
            return new StringContent(Serialize.SerializeObj(data), Encoding.UTF8, "application/json");
        }

        public async Task<HttpResponseMessage> AddAmenity(AmenityViewModel model)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(model);
            HttpResponseMessage responseMessage = await client.PostAsync($"{client.BaseAddress}api/v1/amenity", content);
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetAmenities()
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/amenity");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetHotels()
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/manager?hotels={true}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetSummary()
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/manager?summary={true}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetPeriodSummary(DateTime start, DateTime end)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/manager?periodSummary={true}&start={start}&end={end}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetComplaints()
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/manager?complaints={true}");
            return responseMessage;
        }
    }
}
