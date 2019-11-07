namespace Web.Services
{
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Web.Helpers;
    using Web.Interfaces;
    using Web.Models;
    using Web.ViewModels;

    public class BookingClient : IBookingClient
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly IHttpContextAccessor _contextAccessor;

        public BookingClient(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor)
        {
            _clientFactory = clientFactory;
            _contextAccessor = contextAccessor;
        }

        public async Task<HttpResponseMessage> FrontDeskBookings(Guid id)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/booking?" +
                $"FrontDesk={true}&FrontDeskId={id}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> AddComplaint(BookingViewModel booking)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(booking);
            HttpResponseMessage responseMessage = await client.PutAsync($"{client.BaseAddress}api/v1/booking/{booking.Id}?" +
                $"CheckIn={false}&CheckOut={false}&AddComplaint={true}&RateComplaint={false}", content);
            return responseMessage;
        }

        public async Task<HttpResponseMessage> CancelBooking(Guid id)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.DeleteAsync($"{client.BaseAddress}api/v1/booking?Id={id}");
            return responseMessage;
        }

        public Task<HttpResponseMessage> ChangeGuests(ChangeGuestsViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> CheckIn(BookingViewModel model)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(model);
            HttpResponseMessage responseMessage = await client.PutAsync($"{client.BaseAddress}api/v1/booking/{model.Id}?" +
                $"CheckIn={true}&CheckOut={false}&AddComplaint={false}&RateComplaint={false}", content);
            return responseMessage;
        }

        public async Task<HttpResponseMessage> CheckOut(BookingViewModel model)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(model);
            HttpResponseMessage responseMessage = await client.PutAsync($"{client.BaseAddress}api/v1/booking/{model.Id}?" +
                $"CheckIn={false}&CheckOut={true}&AddComplaint={false}&RateComplaint={false}", content);
            return responseMessage;
        }

        public async Task<List<SelectListItem>> DropDownHotelList()
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/hotel?all=true");

            if (responseMessage.IsSuccessStatusCode)
            {
                List<SelectListItem> list = new List<SelectListItem>();
                ICollection<HotelViewModel> hotels = await responseMessage.Content.ReadAsAsync<ICollection<HotelViewModel>>();
                foreach (var hotel in hotels)
                {
                    list.Add(new SelectListItem(hotel.Name, hotel.Id.ToString()));
                }
                return list;
            }
            else
            {
                throw new Exception(responseMessage.ReasonPhrase);
            }
        }

        public Task<HttpResponseMessage> EditBooking(BookingViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> GetBooking(Guid bookingId, bool WithInvoice = false)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/booking/{bookingId}?WithInvoice={WithInvoice}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetBooking(Guid bookingId, Guid bookerId)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/booker/{bookerId}?" +
                $"complaintOnly={true}&bookingId={bookingId}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetBookings(Guid bookerId)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/booker/{bookerId}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetComplaint(Guid id, Guid bookingId)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/booking/{bookingId}?complaintId={id}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetComplaints(Guid Id)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/booker/{Id}?complaintsOnly={true}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetHotel(Guid id)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/hotel/{id}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetInvoice(Guid bookingId)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/booking/{bookingId}?WithInvoice={true}&InvoiceOnly={true}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetRoom(Guid hotelId, Guid roomId)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/hotel/{hotelId}/room/{roomId}");
            return responseMessage;
        }

        public async Task<ICollection<RoomViewModel>> GetRooms(Guid hotelId)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/hotel?roomsOnly={true}");
            if (responseMessage.IsSuccessStatusCode)
            {
                ICollection<RoomViewModel> collection = await responseMessage.Content.ReadAsAsync<ICollection<RoomViewModel>>();
                return collection;
            }
            else
            {
                throw new Exception("Something went wrong when retrieving hotels");
            }
        }

        public async Task<HttpResponseMessage> GetRooms(Guid hotelId, BookingViewModel bookingViewModel, int? pageIndex, int? pageSize)
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/hotel/{hotelId}/room?start=" +
                $"{bookingViewModel.Arrival}&end={bookingViewModel.Departure}&numGuests={bookingViewModel.NumberOfGuests}" +
                $"&pageIndex={pageIndex}&pageSize={pageSize}");
            return responseMessage;
        }

        public async Task<HttpResponseMessage> GetRooms(ICollection<Guid> selectedRooms, Guid HotelId)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(selectedRooms);
            HttpResponseMessage responseMessage = await client.PutAsync($"{client.BaseAddress}api/v1/hotel/{HotelId}/room", content);
            return responseMessage;
        }

        public async Task<ICollection<HotelViewModel>> HotelList()
        {
            HttpClient client = await CreateClientWithToken();
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/hotel?all=true");
            if (responseMessage.IsSuccessStatusCode)
            {
                ICollection<HotelViewModel> collection = await responseMessage.Content.ReadAsAsync<ICollection<HotelViewModel>>();
                return collection;
            }
            else
            {
                throw new Exception("Something went wrong when retrieving hotels");
            }
        }

        public async Task<HttpResponseMessage> InsertBooking(BookingViewModel booking)
        {
            HttpClient client = await CreateClientWithToken();
            BookingCollections collections = new BookingCollections(booking.SelectedRooms, booking.Invoice.Lines);
            StringContent content = CreateContent(collections);
            HttpResponseMessage responseMessage = await client.PostAsync($"{client.BaseAddress}api/v1/booking?" +
                $"HotelId={booking.HotelId}&" +
                $"BookingId={booking.Id}&" +
                $"Arrival={booking.Arrival}&" +
                $"Departure={booking.Departure}&" +
                $"BookerId={booking.BookerId}&" +
                $"NumberOfGuests={booking.NumberOfGuests}&" +
                $"Status={booking.Status}&" +
                $"HotelName={booking.HotelName}&" +
                $"InvoiceId={booking.Invoice.Id}&" +
                $"InvoiceDate={booking.Invoice.Inv_Date}&" +
                $"PaymentId={booking.Payment.Id}&" +
                $"PaymentMethod={booking.Payment.Payment_Method}&" +
                $"AmountPaid={booking.Payment.AmountPaid}", content);
            return responseMessage;
        }

        public async Task<HttpResponseMessage> RateComplaintResolution(BookingViewModel model)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(model);
            HttpResponseMessage responseMessage = await client.PutAsync($"{client.BaseAddress}api/v1/booking/{model.Id}?" +
                $"CheckIn={false}&CheckOut={false}&AddComplaint={false}&RateComplaint={true}", content);
            return responseMessage;
        }

        public async Task<HttpResponseMessage> SearchCheckInOut(CheckInOutViewModel model)
        {
            HttpClient client = await CreateClientWithToken();
            StringContent content = CreateContent(model);
            HttpResponseMessage responseMessage = await client.GetAsync($"{client.BaseAddress}api/v1/booking?" +
                $"Arrival={model.Arrival}&" +
                $"Departure={model.Departure}&" +
                $"BookerId={model.BookerId}&" +
                $"CheckInOut={true}");
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
    }
}
