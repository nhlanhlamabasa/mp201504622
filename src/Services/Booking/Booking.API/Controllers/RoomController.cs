namespace Booking.API.Controllers
{
    using AutoMapper;
    using Booking.API.Entities;
    using Booking.API.Interfaces;
    using Booking.API.Models;
    using Booking.API.Specifications.RoomSpecifications;
    using HotelSystem.SharedKernel;
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
    /// Defines the <see cref="RoomController" />
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/hotel/{hotelId}/room")]
    public class RoomController : ControllerBase
    {
        /// <summary>
        /// Defines the _hotelRepository
        /// </summary>
        private readonly IHotelRepository _hotelRepository;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<RoomController> _logger;

        /// <summary>
        /// Defines the _roomRepository
        /// </summary>
        private readonly IRoomRepository _roomRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomController"/> class.
        /// </summary>
        /// <param name="roomRepository">The roomRepository<see cref="IRoomRepository"/></param>
        /// <param name="hotelRepository">Hotel Repository</param>
        /// <param name="logger">The logger<see cref="ILogger{RoomController}"/></param>
        public RoomController(IRoomRepository roomRepository, IHotelRepository hotelRepository, ILogger<RoomController> logger)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets room by id
        /// </summary>
        /// <param name="hotelId">Hotel Id</param>
        /// <param name="id">Room id</param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpGet("{id:Guid}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        public async Task<IActionResult> Get([BindRequired, FromRoute]Guid hotelId, [BindRequired, FromRoute]Guid id)
        {
            bool result = await HotelExists(hotelId);
            if (result)
            {
                _logger.LogInformation(LoggingEvents.GetItem, $"Get Room By Id: {id}");
                Room room = await _roomRepository.GetRoomById(id);
                if(room == null)
                {
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, $"Get Room By Id: {id} NOT FOUND");
                    return new NotFoundObjectResult($"Room Id: {id} NOT FOUND");
                }
                RoomViewModel RoomViewModel = Mapper.Map<RoomViewModel>(room);
                return new OkObjectResult(RoomViewModel);
            }
            else
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, $"Hotel Id {hotelId} NOT FOUND");
                return new NotFoundObjectResult($"hotel with id {hotelId} not found");
            }
        }

        /// <summary>
        /// Retrieves all selected rooms
        /// </summary>
        /// <param name="hotelId">Hotel Id</param>
        /// <param name="SelectedRooms">Selected room ids</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Put([BindRequired, FromRoute]Guid hotelId, [BindRequired, FromBody]ICollection<Guid> SelectedRooms)
        {
            bool result = await HotelExists(hotelId);
            if (result)
            {
                if (SelectedRooms != null)
                {
                    List<RoomViewModel> rooms = new List<RoomViewModel>();
                    foreach (Guid roomId in SelectedRooms)
                    {
                        _logger.LogInformation(LoggingEvents.GetItem, $"Get Room By Id: {roomId}");
                        Room room = await _roomRepository.GetRoomById(roomId);
                        if(room == null)
                        {
                            _logger.LogWarning(LoggingEvents.GetItemNotFound, $"Get Room By Id: {roomId} NOT FOUND");
                            return new NotFoundObjectResult($"Room Id: {roomId} NOT FOUND");
                        }
                        RoomViewModel roomViewModel = Mapper.Map<RoomViewModel>(room);
                        rooms.Add(roomViewModel);
                    }
                    return new OkObjectResult(rooms);
                }
                else
                {
                    return new BadRequestObjectResult("Please select rooms");
                }
            }
            else
            {
                return new NotFoundObjectResult($"hotel with id {hotelId} not found");
            }
        }

        /// <summary>
        /// Gets available rooms
        /// </summary>
        /// <param name="hotelId">Hotel Id</param>
        /// <param name="start">Start of Travel</param>
        /// <param name="end">End of Travel</param>
        /// <param name="numGuests">The numGuests<see cref="int"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        public async Task<IActionResult> Get([BindRequired, FromRoute]Guid hotelId, [BindRequired, FromQuery]string start,
            [BindRequired, FromQuery]string end, [BindRequired, FromQuery]int numGuests)
        {
            bool result = await HotelExists(hotelId);
            if (result == false)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, $"Get Hotel By Id: {hotelId} NOT FOUND");
                return new NotFoundObjectResult($"hotel with id {hotelId} not found");
            }
            else if (start == null && end == null)
            {
                _logger.LogWarning(LoggingEvents.BadRequest);
                return new BadRequestObjectResult($"Travel dates cannot be null");
            }
            else
            {
                try
                {
                    _logger.LogInformation(LoggingEvents.ListItems, "Get Available Rooms");
                    RoomQuerySpecification roomQuery = new RoomQuerySpecification(hotelId);
                    NoBookingsQuerySpecification noBookingsQuery = new NoBookingsQuerySpecification();
                    TravelDatesQuerySpecification travelDatesQuery = new TravelDatesQuerySpecification(new DateTimeRange(DateTime.Parse(start), DateTime.Parse(end)));
                    List<Room> AvailableRooms = _roomRepository.GetRoomsWithSpecification(roomQuery, noBookingsQuery, travelDatesQuery);

                    if(AvailableRooms == null)
                    {
                        _logger.LogWarning(LoggingEvents.ListItemsNotFound, "No Rooms Available");
                        return new NotFoundObjectResult("No Rooms Available");
                    }

                    List<Room> SuitableRooms = new List<Room>();
                    Availability.SuitableRoomCombinations(AvailableRooms, numGuests, ref SuitableRooms);

                    if(SuitableRooms.Count == 0)
                    {
                        _logger.LogWarning(LoggingEvents.ListItemsNotFound, "No Rooms Available");
                        return new NotFoundObjectResult("No Rooms Available");
                    }

                    List<RoomViewModel> RoomViewModels = Mapper.Map<List<RoomViewModel>>(SuitableRooms);
                    return new OkObjectResult(RoomViewModels);

                }
                catch (ArgumentOutOfRangeException)
                {
                    _logger.LogWarning(LoggingEvents.BadRequest);
                    return new BadRequestObjectResult($"Travel dates are not valid");
                }
            }
        }

        /// <summary>
        /// Adds new room to hotel
        /// </summary>
        /// <param name="hotelId">Hotel id</param>
        /// <param name="RoomViewModel">RoomViewModel to be added</param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Post([BindRequired, FromRoute]Guid hotelId, [FromBody] RoomViewModel RoomViewModel)
        {
            if (ModelState.IsValid)
            {
                bool result = await HotelExists(hotelId);
                if (result)
                {
                    _logger.LogInformation(LoggingEvents.InsertItem, $"Update Room Id: {RoomViewModel.Id}");
                    Room Room = Mapper.Map<Room>(RoomViewModel);
                    Room Result = await _roomRepository.InsertRoom(Room);
                    RoomViewModel ResultVM = Mapper.Map<RoomViewModel>(Result);
                    return new OkObjectResult(ResultVM);
                }
                else
                {
                    _logger.LogWarning(LoggingEvents.GetItemNotFound, $"hotel with id {hotelId} not found");
                    return new NotFoundObjectResult($"hotel with id {hotelId} not found");
                }
            }
            else
            {
                var errors = Helpers.AddErrors(ModelState);
                _logger.LogWarning(LoggingEvents.BadRequest, $"Errors: {errors}");
                return new BadRequestObjectResult(errors);
            }
        }

        /// <summary>
        /// Updates room view model
        /// </summary>
        /// <param name="hotelId">Hotel Id</param>
        /// <param name="id">Room Id</param>
        /// <param name="RoomViewModel">RoomView model to be updated</param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPut("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Put([BindRequired, FromRoute]Guid hotelId,
            [BindRequired, FromQuery] Guid id, [FromBody] RoomViewModel RoomViewModel)
        {
            if (ModelState.IsValid)
            {
                bool result = await HotelExists(hotelId);
                Room room = await _roomRepository.GetRoomById(id);
                if (result)
                {
                    if (room == null)
                    {
                        return new NotFoundObjectResult($"Room with id: {id} was not found");
                    }
                    else
                    {
                        Room updatedRoom = Mapper.Map<Room>(RoomViewModel);
                        updatedRoom = await _roomRepository.UpdateRoom(updatedRoom);
                        RoomViewModel RoomViewModelResult = Mapper.Map<RoomViewModel>(updatedRoom);
                        return new OkObjectResult(RoomViewModelResult);
                    }
                }
                else
                {
                    return new NotFoundObjectResult($"hotel with id {hotelId} not found");
                }
            }
            else
            {
                var errors = Helpers.AddErrors(ModelState);
                return BadRequest(errors);
            }
        }

        /// <summary>
        /// The HotelExists
        /// </summary>
        /// <param name="hotelId">The hotelId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{T}"/>whose type is <see cref="bool"></see>/></returns>
        [NonAction]
        private async Task<bool> HotelExists(Guid hotelId)
        {
            _logger.LogInformation(LoggingEvents.GetItem, $"Get Hotel By Id {hotelId}");
            Hotel hotel = await _hotelRepository.GetHotelById(hotelId);
            bool value = (hotel != null) ? true : false;
            _logger.LogInformation(LoggingEvents.GetItem, $"Hotel Found? {value}");
            return value;
        }
    }
}
