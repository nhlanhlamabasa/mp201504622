namespace Web.Interfaces
{
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Web.ViewModels;

    public interface IBookingClient
    {
        Task<HttpResponseMessage> FrontDeskBookings(Guid Id);

        Task<HttpResponseMessage> GetComplaints(Guid Id);

        Task<HttpResponseMessage> AddComplaint(BookingViewModel booking);

        Task<HttpResponseMessage> CancelBooking(Guid id);

        Task<HttpResponseMessage> ChangeGuests(ChangeGuestsViewModel model);

        Task<HttpResponseMessage> CheckIn(BookingViewModel model);

        Task<HttpResponseMessage> CheckOut(BookingViewModel model);

        Task<List<SelectListItem>> DropDownHotelList();

        Task<HttpResponseMessage> EditBooking(BookingViewModel model);

        Task<HttpResponseMessage> GetBooking(Guid bookingId, bool WithInvoice = false);

        Task<HttpResponseMessage> GetBooking(Guid bookingId, Guid bookerId);

        Task<HttpResponseMessage> GetBookings(Guid bookerId);

        Task<HttpResponseMessage> GetHotel(Guid id);

        Task<HttpResponseMessage> GetInvoice(Guid BookingId);

        Task<HttpResponseMessage> GetRoom(Guid hotelId, Guid roomId);

        Task<ICollection<RoomViewModel>> GetRooms(Guid hotelId);

        Task<HttpResponseMessage> GetRooms(Guid hotelId, BookingViewModel bookingViewModel, int? pageIndex, int? pageSize);

        Task<HttpResponseMessage> GetRooms(ICollection<Guid> selectedRooms, Guid HotelId);

        Task<ICollection<HotelViewModel>> HotelList();

        Task<HttpResponseMessage> InsertBooking(BookingViewModel bookingViewModel);

        Task<HttpResponseMessage> SearchCheckInOut(CheckInOutViewModel model);

        Task<HttpResponseMessage> GetComplaint(Guid id, Guid bookingId);

        Task<HttpResponseMessage> RateComplaintResolution(BookingViewModel model);
    }
}
