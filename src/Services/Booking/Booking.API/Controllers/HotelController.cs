namespace Booking.API.Controllers
{
    using AutoMapper;
    using Booking.API.Entities;
    using Booking.API.Interfaces;
    using Booking.API.Models;
    using HotelSystem.SharedKernel.Models;
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="HotelController" />
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/hotel")]
    public class HotelController : ControllerBase
    {
        /// <summary>
        /// Defines the _hotelRepository
        /// </summary>
        private readonly IHotelRepository _hotelRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotelController"/> class.
        /// </summary>
        /// <param name="hotelRepository">Repository for managing hotel</param>
        public HotelController(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        /// <summary>
        /// Gets all hotels
        /// </summary>
        /// <param name="all">The all<see cref="bool"/></param>
        /// <param name="pageIndex">The pageIndex<see cref="int"/></param>
        /// <param name="pageSize">The pageSize<see cref="int"/></param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        public IActionResult Get([BindRequired, FromQuery]bool all, [FromQuery]int? pageIndex,
            [FromQuery]int? pageSize)
        {
            if (all)
            {
                ICollection<Hotel> hotels = _hotelRepository.GetHotels();
                if (hotels == null)
                {
                    return new NotFoundObjectResult("No Hotels were found");
                }
                else
                {
                    ICollection<HotelViewModel> hotelViewModels = Mapper.Map<ICollection<HotelViewModel>>(hotels);
                    return new OkObjectResult(hotelViewModels);
                }
            }
            else
            {
                PaginatedList<Hotel> hotels = _hotelRepository.GetHotels(pageIndex.Value, pageSize.Value);
                if (hotels == null)
                {
                    return new NotFoundObjectResult("No Hotels were found");
                }
                else
                {
                    PaginatedList<HotelViewModel> hotelViewModels = Mapper.Map<PaginatedList<HotelViewModel>>(hotels);
                    return new OkObjectResult(hotelViewModels);
                }
            }
        }

        /// <summary>
        /// Gets a hotel
        /// </summary>
        /// <param name="id">Hotel id</param>
        /// <param name="roomsOnly">The roomsOnly<see cref="bool"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        public async Task<IActionResult> Get([BindRequired, FromRoute]Guid id, [FromQuery]bool roomsOnly)
        {
            Hotel hotel = await _hotelRepository.GetHotelById(id);
            if (hotel == null)
            {
                return new NotFoundObjectResult($"Hotel not found with id: {id}");
            }
            else
            {
                if (roomsOnly)
                {
                    ICollection<Room> rooms = await _hotelRepository.GetAllRooms(id);
                    return new OkObjectResult(rooms);
                }
                else
                {
                    HotelViewModel hotelViewModel = Mapper.Map<HotelViewModel>(hotel);
                    return new OkObjectResult(hotelViewModel);
                }

            }
        }

        /// <summary>
        /// Creates new hotels
        /// </summary>
        /// <param name="hotelViewModel"></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(CreatedAtActionResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Post([FromBody] HotelViewModel hotelViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = Helpers.AddErrors(ModelState);
                return new BadRequestObjectResult(errors);
            }
            else
            {
                Hotel hotel = Mapper.Map<Hotel>(hotelViewModel);
                await _hotelRepository.InsertHotel(hotel);
                return CreatedAtAction(nameof(HotelController.Post), new { id = hotel.Id }, hotelViewModel);
            }
        }

        /// <summary>
        /// Updates hotel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hotelViewModel"></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPut("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        public async Task<IActionResult> Put([BindRequired,FromRoute]Guid id, [FromBody] HotelViewModel hotelViewModel)
        {
            Hotel hotel = await _hotelRepository.GetHotelById(id);
            if (hotel == null)
            {
                return new NotFoundObjectResult($"Hotel not found with id: {id}");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    var errors = Helpers.AddErrors(ModelState);
                    return new BadRequestObjectResult(errors);
                }
                else
                {
                    Hotel updatedHotel = await _hotelRepository.UpdateHotel(Mapper.Map<Hotel>(hotelViewModel));
                    HotelViewModel vm = Mapper.Map<HotelViewModel>(updatedHotel);
                    return new OkObjectResult(vm);
                }
            }
        }
    }
}
