namespace Web.Controllers
{
    using FluentValidation.Results;
    using HotelSystem.SharedKernel;
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Rotativa.AspNetCore;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Web.Extensions;
    using Web.Interfaces;
    using Web.Models;
    using Web.Validation;
    using Web.ViewModels;

    [Authorize(Policy = "GUEST")]
    public class BookingController : Controller
    {
        private readonly IBookingClient _bookingClient;

        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingClient bookingClient, ILogger<BookingController> logger)
        {
            _bookingClient = bookingClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Complaints()
        {
            try
            {
                Guid Id = Helpers.Claims.GetUserID(HttpContext.User.Claims);
                HttpResponseMessage responseMessage = await _bookingClient.GetComplaints(Id);
                if(responseMessage.IsSuccessStatusCode)
                {
                    ICollection<ComplaintViewModel> complaints = await responseMessage.Content.ReadAsAsync<ICollection<ComplaintViewModel>>();
                    return View(complaints);
                }
                else
                {
                    return View().WithDanger("Error", responseMessage.ReasonPhrase);
                }
            }
            catch(Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RateComplaintResolution(Guid Id, Guid bookingId)
        {
            try
            {
                HttpResponseMessage responseMessage = await _bookingClient.GetComplaint(Id, bookingId);
                if(responseMessage.IsSuccessStatusCode)
                {
                    ComplaintViewModel model = await responseMessage.Content.ReadAsAsync<ComplaintViewModel>();
                    return View(model);
                }
                else
                {
                    return View().WithDanger("Error", responseMessage.ReasonPhrase);
                }
            }
            catch(Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RateComplaintResolution(ComplaintViewModel model)
        {
            ComplaintRatingValidator validations = new ComplaintRatingValidator();
            ValidationResult validationResult = validations.Validate(model);
            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure error in validationResult.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                return View(model);
            }
            else
            {
                try
                {
                    HttpResponseMessage responseMessage_1 = await _bookingClient.GetBooking(model.BookingId, true);
                    if(responseMessage_1.IsSuccessStatusCode)
                    {
                        BookingViewModel bookingViewModel = await responseMessage_1.Content.ReadAsAsync<BookingViewModel>();
                        bookingViewModel.NewComplaint = new ComplaintViewModel()
                        {
                            BookingId = model.BookingId,
                            LevelOfSatisfaction = model.LevelOfSatisfaction,
                            Id = model.Id,
                            ModifiedDate = model.ModifiedDate,
                            ComplainType = model.ComplainType,
                            CreationTime = model.CreationTime,
                            IsResolved = model.IsResolved,
                            Description = model.Description
                        };
                        HttpResponseMessage responseMessage = await _bookingClient.RateComplaintResolution(bookingViewModel);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Complaints), new { Id = model.BookingId }).WithSuccess("Success", "Complaint rated");
                        }
                        else
                        {
                            return View().WithDanger("Error", responseMessage.ReasonPhrase);
                        }
                    }
                    else
                    {
                        return View().WithDanger("Error", responseMessage_1.ReasonPhrase);
                    }
                    
                }
                catch(Exception e)
                {
                    return View().WithDanger("Error", e.Message);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Complaint(Guid Id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _bookingClient.GetBooking(Id, true);
                if(responseMessage.IsSuccessStatusCode)
                {
                    ComplaintViewModel model = new ComplaintViewModel()
                    {
                        BookingId = Id,
                        IsResolved = false
                    };
                    return View(model);
                }
                else
                {
                    return View().WithDanger("Error", responseMessage.ReasonPhrase);
                }
            }
            catch(Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Complaint(ComplaintViewModel complaintViewModel)
        {
            if(complaintViewModel.BookingId != Guid.Empty)
            {
                ComplaintValidator validations = new ComplaintValidator();
                ValidationResult validationResult = validations.Validate(complaintViewModel);
                if (!validationResult.IsValid)
                {
                    foreach (ValidationFailure error in validationResult.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                    return View(complaintViewModel);
                }
                else
                {
                    HttpResponseMessage responseMessage = await _bookingClient.GetBooking(complaintViewModel.BookingId, true);
                    if(responseMessage.IsSuccessStatusCode)
                    {
                        BookingViewModel booking = await responseMessage.Content.ReadAsAsync<BookingViewModel>();
                        booking.NewComplaint = new ComplaintViewModel()
                        {
                            BookingId = complaintViewModel.BookingId,
                            IsResolved = complaintViewModel.IsResolved,
                            ComplainType = complaintViewModel.ComplainType,
                            Description = complaintViewModel.Description
                        };
                        HttpResponseMessage responseMessage_1 = await _bookingClient.AddComplaint(booking);
                        if(responseMessage_1.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Bookings)).WithSuccess("Success", "Complaint made");
                        }
                        else
                        {
                            return View(complaintViewModel).WithDanger("Error", responseMessage_1.ReasonPhrase);
                        }
                    }
                    else
                    {
                        return View(complaintViewModel).WithDanger("Error", responseMessage.ReasonPhrase);
                    }
                }
            }
            else
            {
                return RedirectToAction(nameof(Bookings)).WithDanger("Error","Choose valid booking");
            }
        }

        [HttpGet]
        public IActionResult Book()
        {
            try
            {
                _logger.LogInformation(LoggingEvents.GetItem, $"Get Booking");
                BookingViewModel model = BookingFactory.GetBookingViewModel(HttpContext);
                model.BookerId = Helpers.Claims.GetUserID(HttpContext.User.Claims);
                model = BookingFactory.GenerateInvoice(model);
                model.Payment = new PaymentViewModel()
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = model.Invoice.Id,
                    AmountPaid = model.Invoice.Total()
                };
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogTrace(new EventId(LoggingEvents.ItemsError, "ItemsError"), e, "An exception occured");
                return RedirectToAction("Index", "Home").WithDanger("Error", e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Book(BookingViewModel model)
        {
            BookingFactory.ClearAll(HttpContext);
            if (ModelState.IsValid)
            {
                try
                {
                    BookingSubmitValidator validationRules = new BookingSubmitValidator();
                    ValidationResult result = validationRules.Validate(model);
                    if (!result.IsValid)
                    {
                        foreach (ValidationFailure error in result.Errors)
                            ModelState.AddModelError("", error.ErrorMessage);
                        return View(model);
                    }

                    HttpResponseMessage responseMessage = await _bookingClient.InsertBooking(model);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(BookingController.Bookings)).WithSuccess("Success", "Booking Complete");
                    }
                    else
                    {
                        if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                        {
                            string message = await responseMessage.Content.ReadAsStringAsync();
                            return View(model).WithDanger("Error", message);
                        }
                        return View(model).WithDanger("Error", responseMessage.ReasonPhrase);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogTrace(new EventId(LoggingEvents.ItemsError, "ItemsError"), e, "An exception occured");
                    return View(model).WithDanger("Error", e.Message);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Bookings(ICollection<BookingViewModel> bookings, string returnUrl = null)
        {
            return await Task.FromResult<IActionResult>(View());
        }

        [HttpGet]
        public async Task<IActionResult> Bookings(string returnUrl = null)
        {
            try
            {
                _logger.LogInformation("Get User Claims");
                Guid BookerId = Helpers.Claims.GetUserID(HttpContext.User.Claims);
                if (BookerId == null || BookerId == Guid.Empty)
                {
                    _logger.LogWarning("User Id Not Found");
                }
                _logger.LogInformation(LoggingEvents.GetItems, $"Get Bookings (BookerId: {BookerId})");
                HttpResponseMessage responseMessage = await _bookingClient.GetBookings(BookerId);
                if (responseMessage.IsSuccessStatusCode)
                {
                    ICollection<BookingViewModel> bookings = await responseMessage.Content.ReadAsAsync<ICollection<BookingViewModel>>();
                    return View(bookings);
                }
                else
                {
                    _logger.LogWarning(LoggingEvents.ItemsError, $"StatusCode: {responseMessage.StatusCode}");
                    string message = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        if (responseMessage.StatusCode == HttpStatusCode.BadRequest || responseMessage.StatusCode == HttpStatusCode.NotFound)
                        {
                            return Redirect(returnUrl).WithDanger("Error", message);
                        }
                        else
                        {
                            return RedirectToAction(returnUrl).WithDanger("Error", responseMessage.ReasonPhrase);
                        }
                    }
                    else
                    {
                        if (responseMessage.StatusCode == HttpStatusCode.BadRequest || responseMessage.StatusCode == HttpStatusCode.NotFound)
                        {
                            return RedirectToAction("Index", "Home").WithDanger("Error", message);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home").WithDanger("Error", responseMessage.ReasonPhrase);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                _logger.LogTrace(new EventId(LoggingEvents.ItemsError, "ItemsError"), e, "An exception occured");
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction(returnUrl).WithDanger("Error", e.Message);
                }
                else
                {
                    return RedirectToAction("Index", "Home").WithDanger("Error", e.Message);
                }
            }
        }

        [HttpGet]
        public IActionResult Cancel(Guid Id)
        {
            return RedirectToAction(nameof(CancelBooking), new { Id });
        }

        [HttpGet]
        public async Task<IActionResult> CancelBooking(Guid Id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _bookingClient.CancelBooking(Id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return View().WithSuccess("Success", "Booking Canceled");
                }
                else
                {
                    if (responseMessage.StatusCode == HttpStatusCode.BadRequest || responseMessage.StatusCode == HttpStatusCode.NotFound)
                    {
                        string message = await responseMessage.Content.ReadAsStringAsync();
                        return View().WithDanger("Error", message);
                    }
                    else
                    {
                        return View().WithDanger("Error", responseMessage.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _bookingClient.GetBooking(Id, true);
                if (responseMessage.IsSuccessStatusCode)
                {
                    BookingViewModel model = await responseMessage.Content.ReadAsAsync<BookingViewModel>();
                    return View(model);
                }
                else
                {
                    if (responseMessage.StatusCode == HttpStatusCode.BadRequest || responseMessage.StatusCode == HttpStatusCode.NotFound)
                    {
                        string message = await responseMessage.Content.ReadAsStringAsync();
                        return View().WithDanger("Error", message);
                    }
                    else
                    {
                        return View().WithDanger("Error", responseMessage.ReasonPhrase);
                    }
                }

            }
            catch (Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadInvoice(Guid Id)
        {
            ViewData["BookingId"] = Id;
            try
            {
                HttpResponseMessage responseMessage = await _bookingClient.GetInvoice(Id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    InvoiceDownload invoice = await responseMessage.Content.ReadAsAsync<InvoiceDownload>();

                    invoice.Name = new FullName(Helpers.Claims.GetUserFirstName(HttpContext.User.Claims),
                        Helpers.Claims.GetUserLastName(HttpContext.User.Claims));
                    invoice.Email = Helpers.Claims.GetUserEmail(HttpContext.User.Claims);

                    return new ViewAsPdf(invoice)
                    {
                        FileName = "Invoice.pdf",
                        PageSize = Rotativa.AspNetCore.Options.Size.A4,
                        PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
                    };
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
        public async Task<IActionResult> EditBooking(ChangeGuestsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage = await _bookingClient.ChangeGuests(model);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        BookingViewModel bookingViewModel = await responseMessage.Content.ReadAsAsync<BookingViewModel>();
                        return View(model);
                    }
                    else
                    {
                        if (responseMessage.StatusCode == HttpStatusCode.BadRequest || responseMessage.StatusCode == HttpStatusCode.NotFound)
                        {
                            string message = await responseMessage.Content.ReadAsStringAsync();
                            return View().WithDanger("Error", message);
                        }
                        else
                        {
                            return View().WithDanger("Error", responseMessage.ReasonPhrase);
                        }
                    }
                }
                catch (Exception e)
                {
                    return View().WithDanger("Error", e.Message);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditBooking(Guid Id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _bookingClient.GetBooking(Id, WithInvoice: true);
                if (responseMessage.IsSuccessStatusCode)
                {
                    BookingViewModel model = await responseMessage.Content.ReadAsAsync<BookingViewModel>();
                    BookingFactory.StoreBookingViewModel(model, HttpContext);
                    ChangeGuestsViewModel changeModel = new ChangeGuestsViewModel(model.Id, model.Arrival,
                        model.Departure, model.HotelId,
                        model.HotelName, model.NumberOfGuests, model.SelectedRooms);
                    ViewData["BookingId"] = Id;
                    return View(changeModel);
                }
                else
                {
                    if (responseMessage.StatusCode == HttpStatusCode.BadRequest || responseMessage.StatusCode == HttpStatusCode.NotFound)
                    {
                        string message = await responseMessage.Content.ReadAsStringAsync();
                        return View().WithDanger("Error", message);
                    }
                    else
                    {
                        return View().WithDanger("Error", responseMessage.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }

        [HttpGet]
        public async Task<JsonResult> EditGetRooms()
        {
            try
            {
                _logger.LogInformation(LoggingEvents.GetItem, "Get BookingViewModel");
                BookingViewModel model = BookingFactory.GetBookingViewModel(HttpContext);
                if (model == null)
                {
                    _logger.LogInformation(LoggingEvents.ItemNotFound, "Get BookingViewModel - Not Found");
                }
                _logger.LogInformation(LoggingEvents.GetItems, $"Get Rooms: (HotelId: {model.HotelId} )");
                model.NumberOfGuests = 3;
                HttpResponseMessage response = await _bookingClient.GetRooms(model.HotelId, model, null, null);
                if (response.IsSuccessStatusCode)
                {
                    ICollection<RoomViewModel> availableRooms = await response.Content.ReadAsAsync<ICollection<RoomViewModel>>();
                    return new JsonResult(availableRooms);
                }
                else
                {
                    _logger.LogWarning(LoggingEvents.ItemsError, $"StatusCode: {response.StatusCode}");
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new JsonResult($"Error! {response.ReasonPhrase}");
                }

            }
            catch (Exception e)
            {
                _logger.LogTrace(new EventId(LoggingEvents.ItemsError, "ItemsError"), e, "An exception occured");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(e.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetRooms()
        {
            try
            {
                _logger.LogInformation(LoggingEvents.GetItem, "Get BookingViewModel");
                BookingViewModel model = BookingFactory.GetBookingViewModel(HttpContext);
                if (model == null)
                {
                    _logger.LogInformation(LoggingEvents.ItemNotFound, "Get BookingViewModel - Not Found");
                }
                _logger.LogInformation(LoggingEvents.GetItems, $"Get Rooms: (HotelId: {model.HotelId} )");
                HttpResponseMessage response = await _bookingClient.GetRooms(model.HotelId, model, null, null);
                if (response.IsSuccessStatusCode)
                {
                    ICollection<RoomViewModel> availableRooms = await response.Content.ReadAsAsync<ICollection<RoomViewModel>>();
                    return new JsonResult(availableRooms);
                }
                else
                {
                    _logger.LogWarning(LoggingEvents.ItemsError, $"StatusCode: {response.StatusCode}");
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new JsonResult($"Error! {response.ReasonPhrase}");
                }

            }
            catch (Exception e)
            {
                _logger.LogTrace(new EventId(LoggingEvents.ItemsError, "ItemsError"), e, "An exception occured");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(e.Message);
            }
        }

        [HttpPut]
        [AllowAnonymous]
        public JsonResult RemoveRoom(Guid Id)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.DeleteItem, $"Remove Room: {Id}");
                BookingFactory.RemoveRoom(HttpContext, Id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return new JsonResult("OK");
            }
            catch (Exception e)
            {
                _logger.LogTrace(new EventId(LoggingEvents.ItemsError, "ItemsError"), e, "An exception occured");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                TempData["_alert.type"] = "danger";
                TempData["_alert.title"] = "Error";
                TempData["_alert.body"] = e.Message;
                return new JsonResult(e.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> SelectRoom(Guid Id)
        {
            try
            {
                _logger.LogInformation(LoggingEvents.InsertItem, $"Reserve Room: {Id}");
                BookingViewModel bookingViewModel = BookingFactory.GetBookingViewModel(HttpContext);
                HttpResponseMessage responseMessage = await _bookingClient.GetRoom(bookingViewModel.HotelId, Id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    RoomViewModel roomViewModel = await responseMessage.Content.ReadAsAsync<RoomViewModel>();
                    BookingFactory.ReserveRoom(roomViewModel, HttpContext);
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    return new JsonResult("OK");
                }
                else
                {
                    TempData["_alert.type"] = "danger";
                    TempData["_alert.title"] = "Error";
                    TempData["_alert.body"] = responseMessage.ReasonPhrase;
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new JsonResult(responseMessage.ReasonPhrase);
                }

            }
            catch (Exception e)
            {
                _logger.LogTrace(new EventId(LoggingEvents.ItemsError, "ItemsError"), e, "An exception occured");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                TempData["_alert.type"] = "danger";
                TempData["_alert.title"] = "Error";
                TempData["_alert.body"] = e.Message;
                return new JsonResult(e.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SelectRooms()
        {
            try
            {
                BookingViewModel model = BookingFactory.GetBookingViewModel(HttpContext);
                ExpiredSessionValidator validationRules = new ExpiredSessionValidator();
                ValidationResult result = validationRules.Validate(model);
                if (!result.IsValid)
                {
                    return RedirectToAction("Index", "Home");
                }
                return View(model);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SelectRooms(BookingViewModel model)
        {
            try
            {
                int NumberOfBeds = BookingFactory.NumberOfBeds(HttpContext);
                SelectRoomsValidator validationRules = new SelectRoomsValidator(NumberOfBeds);
                ValidationResult result = validationRules.Validate(model);
                if (!result.IsValid)
                {
                    foreach (ValidationFailure error in result.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                    BookingFactory.ClearSelection(HttpContext);
                    return View(model);
                }
                else
                {
                    return RedirectToAction(nameof(BookingController.Book));
                }
            }
            catch (Exception e)
            {
                BookingFactory.ClearSelection(HttpContext);
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
        }
    }
}
