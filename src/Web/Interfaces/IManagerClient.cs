namespace Web.Interfaces
{
    using HotelSystem.SharedKernel.ViewModels;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IManagerClient
    {
        Task<HttpResponseMessage> AddHotel(HotelViewModel hotel);

        Task<HttpResponseMessage> AddRoom(Guid HotelId, RoomViewModel room);

        Task<HttpResponseMessage> EditHotel(Guid HotelId, HotelViewModel hotel);

        Task<HttpResponseMessage> EditRoom(Guid HotelId, Guid RoomId, RoomViewModel room);

        Task<HttpResponseMessage> GetHotel(Guid HotelId, bool WithDetails = false);

        Task<HttpResponseMessage> GetHotels();

        Task<HttpResponseMessage> GetRoom(Guid HotelId, Guid RoomId, bool WithDetails = false);

        Task<HttpResponseMessage> GetRooms(Guid HotelId);

        Task<HttpResponseMessage> RemoveRoom(Guid HotelId, Guid RoomId);

        Task<HttpResponseMessage> RemoveRoom(Guid HotelId, Guid RoomId, RoomViewModel room);

        Task<HttpResponseMessage> RemoveHotel(Guid HotelId);

        Task<HttpResponseMessage> RemoveHotel(Guid HotelId, HotelViewModel hotel);

        Task<HttpResponseMessage> AddAmenity(AmenityViewModel model);

        Task<HttpResponseMessage> GetAmenities();

        Task<HttpResponseMessage> GetSummary();

        Task<HttpResponseMessage> GetPeriodSummary(DateTime start, DateTime end);

        Task<HttpResponseMessage> GetComplaints();
    }
}
