namespace Web.Controllers
{
    using HotelSystem.SharedKernel;
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Web.Extensions;
    using Web.Interfaces;
    using Web.Models;
    using Web.ViewModels;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookingClient _bookingClient;
        private readonly IIdentityClient _identityClient;

        public HomeController(ILogger<HomeController> logger, IBookingClient bookingClient, IIdentityClient identityClient)
        {
            _logger = logger;
            _bookingClient = bookingClient;
            _identityClient = identityClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Index View");
            return View();
        }

        [HttpPost]
        public IActionResult Index(AvailabilityViewModel model)
        {
            if(ModelState.IsValid)
            {
                _logger.LogInformation("Save Availability model in Session");
                BookingViewModel bookingViewModel = new BookingViewModel()
                {
                    Id = Guid.NewGuid(),
                    Arrival = model.Arrival,
                    Departure = model.Departure,
                    HotelId = model.HotelId,
                    HotelName = model.HotelName,
                    NumberOfGuests = model.NumberOfGuests,
                    Status = BookingStatus.AwaitingPayment.ToString()
                };
                BookingFactory.StoreBookingViewModel(bookingViewModel, HttpContext);
                return RedirectToAction("SelectRooms", "Booking");
            }
            else
            {   
                var errors = Helpers.Errors.AddErrors(ModelState);
                return View().WithDanger("Error", errors.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(CheckInOutViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage_1 = await _identityClient.GetBookerId(model.Email);
                    if(responseMessage_1.IsSuccessStatusCode)
                    {
                        string json = await responseMessage_1.Content.ReadAsStringAsync();
                        string id = JsonConvert.DeserializeObject<string>(json);
                        model.BookerId = Guid.Parse(id);

                        HttpResponseMessage responseMessage = await _bookingClient.SearchCheckInOut(model);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            BookingViewModel booking = await responseMessage.Content.ReadAsAsync<BookingViewModel>();
                            Enum.TryParse(booking.Status, out BookingStatus myStatus);

                            switch(myStatus)
                            {
                                case BookingStatus.AwaitingPayment:
                                    {
                                        return View("Index").WithDanger("Error", "Payment not received");
                                    }
                                case BookingStatus.Paid:
                                    {
                                        booking.CheckInPerson = Helpers.Claims.GetUserID(HttpContext.User.Claims);
                                        return View("./Views/FrontDesk/CheckIn.cshtml", booking);
                                    }
                                case BookingStatus.CheckedIn:
                                    {
                                        booking.CheckOutPerson = Helpers.Claims.GetUserID(HttpContext.User.Claims);
                                        return View("./Views/FrontDesk/CheckIn.cshtml", booking);
                                    }
                                case BookingStatus.CheckedOut:
                                    {
                                        return View("Index").WithDanger("Error", "Already checked out");
                                    }
                                case BookingStatus.Cancelled:
                                    {
                                        return View("Index").WithDanger("Error", $"Booking was cancelled on {booking.ModifiedDate}");
                                    }
                                default:
                                    {
                                        return View("Index");
                                    }
                            }
                        }
                        else
                        {
                            return View("Index").WithDanger("Error", responseMessage.ReasonPhrase);
                        }
                    }
                    else
                    {
                        return View("Index").WithDanger("Error", responseMessage_1.ReasonPhrase);
                    }
                }
                catch (Exception e)
                {
                    return View("Index").WithDanger("Error", e.Message);
                }
            }
            else
            {
                var errors = Helpers.Errors.AddErrors(ModelState);
                return View("Index").WithDanger("Error", errors.ToString());
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { Message =  "Something went wrong"});
        }
    }
}
