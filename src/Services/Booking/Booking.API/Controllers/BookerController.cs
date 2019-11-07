namespace Booking.API.Controllers
{
    using AutoMapper;
    using Booking.API.Entities;
    using Booking.API.Interfaces;
    using Booking.API.Models;
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="BookerController" />
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/booker/{bookerId}")]
    public class BookerController : ControllerBase
    {
        /// <summary>
        /// Defines the _bookerRepository
        /// </summary>
        private readonly IBookerRepository _bookerRepository;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<BookerController> _logger;
        private readonly IBookingRepository  _bookingRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookerController"/> class.
        /// </summary>
        /// <param name="bookerRepository">The bookerRepository<see cref="IBookerRepository"/></param>
        /// <param name="bookingRepository"></param>
        /// <param name="logger">The logger<see cref="ILogger{BookerController}"/></param>
        public BookerController(IBookerRepository bookerRepository, 
            IBookingRepository bookingRepository,
            ILogger<BookerController> logger)
        {
            _bookerRepository = bookerRepository;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves one booking or all bookings
        /// </summary>
        /// <param name="bookerId">The BookerId<see cref="Guid"/></param>
        /// <param name="bookingId">The BookingId<see cref="Guid"/></param>
        /// <param name="complaintsOnly"></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundResult))]
        public async Task<IActionResult> Get([BindRequired, FromRoute]Guid bookerId, 
            [FromQuery]Guid? bookingId, [FromQuery]bool complaintsOnly = false)
        {
            if(bookingId != null)
            {
                _logger.LogInformation(LoggingEvents.GetItem, $"GetBookingByBookerId(BookerId: {bookerId}, BookingId: {bookingId})");
                Entities.Booking booking = _bookerRepository.GetBookingByBookerId(bookerId, bookingId.Value);
                if (booking == null)
                {
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, $"GetBookingByBookerId(BookerId: {bookerId}, BookingId: {bookingId}) NOT FOUND");
                    return new NotFoundObjectResult($"Not Found. User: {bookerId}. Requested booking: {bookingId}");
                }
                BookingViewModel bookingViewModel = Mapper.Map<BookingViewModel>(booking);
                return new OkObjectResult(bookingViewModel);
            }
            if(complaintsOnly)
            {
                List<Complaint> complaints = new List<Complaint>();
                ICollection<Booking> bookings =  _bookerRepository.GetBookingsByBookerId(bookerId);
                foreach(var booking in bookings)
                {
                    ICollection<Complaint> bookingComplaints = await _bookingRepository.GetBookingComplaints(booking.Id);
                    complaints.AddRange(bookingComplaints);
                }
                return new OkObjectResult(Mapper.Map<ICollection<ComplaintViewModel>>(complaints));
            }
            else
            {
                _logger.LogInformation(LoggingEvents.ListItems, $"GetBookingsByBookerId(BookerId: {bookerId})");
                ICollection<Entities.Booking> bookings = _bookerRepository.GetBookingsByBookerId(bookerId);

                if(bookings != null)
                {
                    ICollection<BookingViewModel> bookingViewModels = Mapper.Map<ICollection<BookingViewModel>>(bookings);
                    return new OkObjectResult(bookingViewModels);
                }
                else
                {
                    _logger.LogWarning(LoggingEvents.ListItemsNotFound, $"GetBookingsByBookerId(BookerId: {bookerId})");
                    return new NotFoundObjectResult("No Bookings found");
                }
            }
        }
    }
}
