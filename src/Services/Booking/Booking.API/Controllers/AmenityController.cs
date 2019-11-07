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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AmenityController" />
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/amenity")]
    public class AmenityController : ControllerBase
    {
        /// <summary>
        /// Defines the _managerRepository
        /// </summary>
        private readonly IManagerRepository _managerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityController"/> class.
        /// </summary>
        /// <param name="managerRepository">The managerRepository<see cref="IManagerRepository"/></param>
        public AmenityController(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        /// <summary>
        /// The get retrieves all amenities
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ICollection<Amenity> amenities = await _managerRepository.GetAmenities();
            return new OkObjectResult(amenities);
        }

        /// <summary>
        /// The Post
        /// </summary>
        /// <param name="model">The model<see cref="AmenityViewModel"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPost]
        public async Task<IActionResult> Post([BindRequired, FromBody]AmenityViewModel model)
        {
            if (ModelState.IsValid)
            {
                Amenity amenity = Mapper.Map<AmenityViewModel, Amenity>(model);
                amenity = await _managerRepository.AddAmenity(amenity);
                model = Mapper.Map<Amenity, AmenityViewModel>(amenity);
                return new OkObjectResult(amenity);
            }
            else
            {
                var errors = Helpers.AddErrors(ModelState);
                return new BadRequestObjectResult(errors.ToString());
            }
        }
    }
}
