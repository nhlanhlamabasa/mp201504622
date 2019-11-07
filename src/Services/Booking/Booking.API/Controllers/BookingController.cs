namespace Booking.API.Controllers
{
    using AutoMapper;
    using Booking.API.Entities;
    using Booking.API.Exceptions;
    using Booking.API.Interfaces;
    using Booking.API.Models;
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="BookingController" />
    /// </summary>
    [Authorize]
    //[ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/booking")]
    public class BookingController : ControllerBase
    {
        /// <summary>
        /// Defines the _bookingRepository
        /// </summary>
        private readonly IBookingRepository _bookingRepository;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private ILogger<BookingController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        /// <param name="bookingRepository"></param>
        /// <param name="logger">Logger</param>
        public BookingController(IBookingRepository bookingRepository, ILogger<BookingController> logger)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        /// <summary>
        /// The Delete
        /// </summary>
        /// <param name="Id">The Id<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([BindRequired, FromQuery]Guid Id)
        {
            try
            {
                await _bookingRepository.DeleteBooking(Id);
                return new NoContentResult();

            }
            catch (NotFoundException e)
            {
                return new NotFoundObjectResult(e.Message);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        /// <summary>
        /// Gets list of bookings
        /// </summary>
        /// <param name="Arrival">The Arrival<see cref="DateTime"/></param>
        /// <param name="Departure">The Departure<see cref="DateTime"/></param>
        /// <param name="BookerId">The BookerId<see cref="Guid"/></param>
        /// <param name="CheckInOut">The CheckIn<see cref="bool"/></param>
        /// <param name="FrontDesk"></param>
        /// <param name="FrontDeskId"></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        public async Task<IActionResult> Get([FromQuery]DateTime Arrival,
            [FromQuery]DateTime Departure,
            [FromQuery]Guid BookerId,
            [FromQuery]bool CheckInOut, [FromQuery]bool FrontDesk, [FromQuery]Guid FrontDeskId)
        {
            if (FrontDesk)
            {
                ICollection<Booking> bookings = await _bookingRepository.FrontDeskBookings(FrontDeskId);
                return new OkObjectResult(Mapper.Map<ICollection<BookingViewModel>>(bookings));
            }
            if (CheckInOut)
            {
                try
                {
                    Booking booking = await _bookingRepository.SearchBooking(Arrival, Departure, BookerId);
                    BookingViewModel model = Mapper.Map<BookingViewModel>(booking);
                    model.SelectedRooms = Mapper.Map<List<RoomViewModel>>(booking.SelectedRooms);
                    model.NumSelectedRooms = model.SelectedRooms.Count;
                    return new OkObjectResult(model);
                }
                catch (ArgumentNullException e)
                {
                    return new NotFoundObjectResult(e.Message);
                }
                catch (Exception)
                {
                    return new StatusCodeResult(500);
                }
            }
            else
            {
                _logger.LogInformation(LoggingEvents.ListItems, $"Get Bookings");
                ICollection<Booking> bookings = _bookingRepository.GetBookings();
                if (bookings == null)
                {
                    _logger.LogWarning(LoggingEvents.ListItemsNotFound, $"Get Bookings - NOT FOUND");
                    return new NotFoundObjectResult("No Bookings were found");
                }
                else
                {
                    ICollection<BookingViewModel> bookingViewModels = Mapper.Map<ICollection<BookingViewModel>>(bookings);
                    return new OkObjectResult(bookingViewModels);
                }
            }
        }

        /// <summary>
        /// Gets single booking
        /// </summary>
        /// <param name="id">booking id</param>
        /// <param name="WithInvoice">The WithInvoice<see cref="bool"/></param>
        /// <param name="InvoiceOnly">The InvoiceOnly<see cref="bool"/></param>
        /// <param name="complaintsOnly"></param>
        /// <param name="complaintOnly"></param>
        /// <param name="complaintId"></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        public async Task<IActionResult> Get([BindRequired, FromRoute]Guid id, [FromQuery]bool WithInvoice = false, [FromQuery]bool InvoiceOnly = false,
            [FromQuery]bool complaintsOnly = false, [FromQuery]bool complaintOnly = false, [FromQuery]Guid? complaintId = null)
        {
            if (InvoiceOnly)
            {
                _logger.LogInformation(LoggingEvents.GetItem, $"Get Invoice Only For BookingID: {id}");
                Invoice invoice = await _bookingRepository.GetInvoiceByBookingId(id);
                Booking booking = await _bookingRepository.GetBookingById(id, false);
                booking.SelectedRooms = _bookingRepository.GetSelectedRooms(booking.Id);
                Hotel hotel = await _bookingRepository.GetHotel(booking.HotelId);

                BookingViewModel bookingViewModel = Mapper.Map<BookingViewModel>(booking);
                bookingViewModel.SelectedRooms = Mapper.Map<List<RoomViewModel>>(booking.SelectedRooms);
                HotelViewModel hotelViewModel = Mapper.Map<HotelViewModel>(hotel);
                InvoiceViewModel model = Mapper.Map<InvoiceViewModel>(invoice);
                PaymentViewModel paymentViewModel = Mapper.Map<PaymentViewModel>(invoice.Payment);

                InvoiceDownload invoiceDownload = new InvoiceDownload()
                {
                    booking = bookingViewModel,
                    invoice = model,
                    hotel = hotelViewModel,
                    payment = paymentViewModel
                };
                return new OkObjectResult(invoiceDownload);
            }
            if (complaintsOnly)
            {
                ICollection<Complaint> complaints = await _bookingRepository.GetBookingComplaints(id);
                ICollection<ComplaintViewModel> complaintViewModels = Mapper.Map<ICollection<ComplaintViewModel>>(complaints);
                return new OkObjectResult(complaintViewModels);
            }
            if (complaintOnly)
            {
                ICollection<Complaint> complaints = await _bookingRepository.GetBookingComplaints(id);
                Complaint complaint = complaints.Single(x => x.Id == complaintId.Value);
                ComplaintViewModel model = Mapper.Map<ComplaintViewModel>(complaint);
                return new OkObjectResult(model);
            }
            else
            {
                _logger.LogInformation(LoggingEvents.GetItem, $"Get Booking By Id (Id: {id}, WithInvoice: {WithInvoice}");
                Booking result = await _bookingRepository.GetBookingById(id, WithInvoice);
                List<Room> selected = _bookingRepository.GetSelectedRooms(result.Id);
                if (result == null)
                {
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, $"Get Booking By Id (Id: {id}, WithInvoice: {WithInvoice}) NOT FOUND");
                    return new NotFoundObjectResult($"Booking not found with id: {id}");
                }
                else
                {
                    BookingViewModel model = Mapper.Map<BookingViewModel>(result);
                    model.SelectedRooms = Mapper.Map<List<RoomViewModel>>(selected);
                    return new OkObjectResult(model);
                }
            }
        }

        /// <summary>
        /// Creates booking
        /// </summary>
        /// <param name="HotelId">The HotelId<see cref="Guid"/></param>
        /// <param name="BookingId">Booking Id <see cref="Guid"/></param>
        /// <param name="Arrival">The Arrival<see cref="DateTime"/></param>
        /// <param name="Departure">The Departure<see cref="DateTime"/></param>
        /// <param name="BookerId">The BookerId<see cref="Guid"/></param>
        /// <param name="NumberOfGuests">The NumberOfGuests<see cref="int"/></param>
        /// <param name="Status">The Status<see cref="string"/></param>
        /// <param name="HotelName">Hotel Name <see cref="string"/></param>
        /// <param name="InvoiceId">Invoice id <see cref="Guid"/></param>
        /// <param name="PaymentId">The PaymentId<see cref="Guid"/></param>
        /// <param name="PaymentMethod">The PaymentMethod<see cref="string"/></param>
        /// <param name="AmountPaid">The AmountPaid<see cref="decimal"/></param>
        /// <param name="RoomsAndLines"></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Post(
            [BindRequired, FromQuery]Guid HotelId,
            [BindRequired, FromQuery]Guid BookingId,
            [BindRequired, FromQuery]DateTime Arrival,
            [BindRequired, FromQuery]DateTime Departure,
            [BindRequired, FromQuery]Guid BookerId,
            [BindRequired, FromQuery]int NumberOfGuests,
            [BindRequired, FromQuery]string Status,
            [BindRequired, FromQuery]string HotelName,
            [BindRequired, FromQuery]Guid InvoiceId,
            [BindRequired, FromQuery]Guid PaymentId,
            [BindRequired, FromQuery]string PaymentMethod,
            [BindRequired, FromQuery]decimal AmountPaid,
            [BindRequired, FromBody]BookingCollections RoomsAndLines)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Re-check to ensure rooms are still available for duration

                    List<Room> SelectedRooms = new List<Room>();
                    List<Line> Lines = new List<Line>();
                    foreach (RoomViewModel room in RoomsAndLines.RoomViewModels)
                    {
                        SelectedRooms.Add(Mapper.Map<RoomViewModel, Room>(room));
                    }
                    foreach (LineViewModel line in RoomsAndLines.LineViewModels)
                    {
                        Lines.Add(Mapper.Map<LineViewModel, Line>(line));
                    }

                    Booking booking = new Booking(HotelName, Arrival, Departure, NumberOfGuests, BookerId, SelectedRooms, HotelId);
                    booking.setID(BookingId);
                    booking.Invoice = new Invoice(InvoiceId, BookerId, BookingId);
                    booking.Invoice.Lines = Lines;
                    booking.Invoice.Inv_Date = DateTime.Now;
                    booking.Invoice.Payment = new Payment(PaymentId, InvoiceId, PaymentMethod, booking.Invoice.Total());

                    _logger.LogInformation(LoggingEvents.InsertItem, $"Insert Booking");

                    booking = await _bookingRepository.InsertBooking(booking);

                    BookingViewModel bookingViewModel = Mapper.Map<BookingViewModel>(booking);

                    return new OkObjectResult(bookingViewModel);
                }
                else
                {
                    var errors = Helpers.AddErrors(ModelState);
                    _logger.LogWarning(LoggingEvents.BadRequest, $"ModelState Errors: {errors} BAD REQUEST");
                    return new BadRequestObjectResult(errors);
                }
            }
            catch (TravelDatesOverlapException e)
            {
                _logger.LogWarning(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
            catch (PaymentException e)
            {
                _logger.LogWarning(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Update Booking
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CheckIn">The CheckIn<see cref="bool"/></param>
        /// <param name="CheckOut">The CheckOut<see cref="bool"/></param>
        /// <param name="AddComplaint"></param>
        /// <param name="RateComplaint"></param>
        /// <param name="bookingViewModel"></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPut("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        public async Task<IActionResult> Put([BindRequired, FromRoute]Guid id,
            [BindRequired, FromQuery]bool CheckIn,
            [BindRequired, FromQuery]bool CheckOut,
            [BindRequired, FromQuery]bool AddComplaint,
            [BindRequired, FromQuery]bool RateComplaint,
            [FromBody] BookingViewModel bookingViewModel)
        {
            _logger.LogInformation(LoggingEvents.GetItem, $"Get Booking By Id (BookingId: {id}");
            Booking booking = await _bookingRepository.GetBookingById(id, false);
            if (booking == null)
            {
                _logger.LogInformation(LoggingEvents.GetItemNotFound, $"Get Booking By Id (BookingId: {id}) NOT FOUND");
                return new NotFoundObjectResult($"Hotel not found with id: {id}");
            }

            if (CheckIn)
            {
                CheckInCheckList checkInCheckList = Mapper.Map<CheckInCheckList>(bookingViewModel.CheckInCheckList);
                var updatedBooking = await _bookingRepository.CheckIn(booking.Id, bookingViewModel.CheckInPerson, checkInCheckList);
                BookingViewModel vm = Mapper.Map<BookingViewModel>(updatedBooking);
                return new OkObjectResult(vm);

            }
            if (CheckOut)
            {
                var updatedBooking = await _bookingRepository.CheckOut(booking.Id, bookingViewModel.CheckOutPerson);
                BookingViewModel vm = Mapper.Map<BookingViewModel>(updatedBooking);
                return new OkObjectResult(vm);
            }
            if (AddComplaint)
            {
                Complaint complaint = Mapper.Map<Complaint>(bookingViewModel.NewComplaint);
                Booking updatedBooking = await _bookingRepository.AddComplaint(bookingViewModel.Id, complaint);
                BookingViewModel vm = Mapper.Map<BookingViewModel>(updatedBooking);
                return new OkObjectResult(vm);
            }
            if (RateComplaint)
            {
                Complaint complaint = Mapper.Map<Complaint>(bookingViewModel.NewComplaint);
                Booking updatedBooking = await _bookingRepository.UpdateComplaint(bookingViewModel.Id, complaint);
                BookingViewModel vm = Mapper.Map<BookingViewModel>(updatedBooking);
                return new OkObjectResult(vm);
            }

            if (!ModelState.IsValid)
            {
                var errors = Helpers.AddErrors(ModelState);
                _logger.LogWarning(LoggingEvents.BadRequest, $"Booking View Model Errors: {errors}");
                return new BadRequestObjectResult(errors);
            }
            else
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, $"Update Booking Id {bookingViewModel.Id}");
                Booking updatedBooking = await _bookingRepository.UpdateBooking(Mapper.Map<Booking>(bookingViewModel));
                BookingViewModel vm = Mapper.Map<BookingViewModel>(updatedBooking);
                return new OkObjectResult(vm);
            }
        }
    }
}
