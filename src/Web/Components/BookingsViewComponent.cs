using HotelSystem.SharedKernel.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Interfaces;

namespace Web.Components
{
    public class BookingsViewComponent : ViewComponent
    {
        private readonly IBookingClient _bookingClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public BookingsViewComponent(IBookingClient bookingClient, IHttpContextAccessor contextAccessor)
        {
            _bookingClient = bookingClient;
            _contextAccessor = contextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                Guid Id = Helpers.Claims.GetUserID(_contextAccessor.HttpContext.User.Claims);
                HttpResponseMessage responseMessage = await _bookingClient.FrontDeskBookings(Id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    ICollection<BookingViewModel> bookings = await responseMessage.Content.ReadAsAsync<ICollection<BookingViewModel>>();
                    return View(bookings);
                }
                else
                {
                    TempData["_alert.type"] = "danger";
                    TempData["_alert.title"] = "Error";
                    TempData["_alert.body"] = responseMessage.ReasonPhrase;
                    return View();
                }
            }
            catch (Exception e)
            {
                TempData["_alert.type"] = "danger";
                TempData["_alert.title"] = "Error";
                TempData["_alert.body"] = e.Message;
                return View();
            }
        }
    }
}
