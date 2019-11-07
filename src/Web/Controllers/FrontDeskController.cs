namespace Web.Controllers
{
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Web.Extensions;
    using Web.Interfaces;

    [Authorize(Policy = "FRONTDESK")]
    public class FrontDeskController : Controller
    {
        private readonly IBookingClient _bookingClient;

        public FrontDeskController(IBookingClient bookingClient)
        {
            _bookingClient = bookingClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CheckIn(Guid Id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _bookingClient.GetBooking(Id, true);
                if(responseMessage.IsSuccessStatusCode)
                {
                    BookingViewModel booking = await responseMessage.Content.ReadAsAsync<BookingViewModel>();
                    return View(booking);
                }
                else
                {
                    return View().WithDanger("Error", responseMessage.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckIn(BookingViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage = await _bookingClient.CheckIn(model);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index)).WithSuccess("Success", "Checked In");
                    }
                    else
                    {
                        return View(model).WithDanger("Error", responseMessage.ReasonPhrase);
                    }
                }
                catch (Exception e)
                {
                    return View(model).WithDanger("Error", e.Message);
                }
            }
            else
            {
                return View(model);
            }
        }
            

        [HttpGet]
        public async Task<IActionResult> CheckOut(Guid Id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _bookingClient.GetBooking(Id, true);
                if (responseMessage.IsSuccessStatusCode)
                {
                    BookingViewModel booking = await responseMessage.Content.ReadAsAsync<BookingViewModel>();
                    return View(booking);
                }
                else
                {
                    return View().WithDanger("Error", responseMessage.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(BookingViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage = await _bookingClient.CheckOut(model);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index)).WithSuccess("Success", "Checked Out");
                    }
                    else
                    {
                        return View(model).WithDanger("Error", responseMessage.ReasonPhrase);
                    }
                }
                catch (Exception e)
                {
                    return View(model).WithDanger("Error", e.Message);
                }
            }
            else
            {
                return View(model);
            }
            
        }
    }
}

