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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ManagerController" />
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/manager/")]
    public class ManagerController : ControllerBase
    {
        /// <summary>
        /// Defines the _managerReposoitory
        /// </summary>
        private readonly IManagerRepository _managerReposoitory;
        private readonly IBookingRepository _bookingRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerController"/> class.
        /// </summary>
        /// <param name="managerRepository">The managerRepository<see cref="IManagerRepository"/></param>
        /// <param name="bookingRepository"></param>
        public ManagerController(IManagerRepository managerRepository,
            IBookingRepository bookingRepository)
        {
            _managerReposoitory = managerRepository;
            _bookingRepository = bookingRepository;
        }



        /// <summary>
        /// The get action for retrieving hotels
        /// </summary>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery]bool hotels = false, [FromQuery]bool summary = false,
            [FromQuery]bool periodSummary = false, [FromQuery]DateTime? start = null, [FromQuery]DateTime? end = null,
            [FromQuery]bool complaints = false, [FromQuery]bool complaint =false, [FromQuery]Guid? complaintId = null)
        {
            if(hotels)
            {
                try
                {
                    ICollection<Hotel> list = await _managerReposoitory.GetHotels();

                    foreach(Hotel hotel in list)
                    {
                        hotel.Rooms = await _managerReposoitory.GetRooms(hotel.Id);

                    }

                    ICollection<HotelViewModel> models = Mapper.Map<ICollection<HotelViewModel>>(list);
                    return new OkObjectResult(models);
                }
                catch (Exception e)
                {
                    return new BadRequestObjectResult(e.Message);
                }
            }
            if(complaint)
            {
                Complaint complaintModel = await _managerReposoitory.GetComplaint(complaintId.Value);
                if(complaintModel == null)
                {
                    return new NotFoundObjectResult("Not found");

                }
                else
                {
                    return new OkObjectResult(Mapper.Map<ComplaintViewModel>(complaintModel));
                }
            }
            if(complaints)
            {
                ICollection<Complaint> complaintsList = await _managerReposoitory.GetComplaints();
                return new OkObjectResult(Mapper.Map<ICollection<ComplaintViewModel>>(complaintsList));
            }
            if(summary)
            {
                SummaryViewModel summaryViewModel = new SummaryViewModel();
                summaryViewModel.TotalNumberOfAmenities = _managerReposoitory.NumberOfAmenities();
                summaryViewModel.TotalNumberOfHotels = _managerReposoitory.NumberOfHotels();
                summaryViewModel.TotalNumberOfRooms = _managerReposoitory.NumberOfRooms();
                summaryViewModel.TotalOfValueOfPayments = _managerReposoitory.TotalPaymentsReceived();
                return new OkObjectResult(summaryViewModel);
            }
            if(periodSummary)
            {
                PeriodSummary periodSummaryViewModel = _managerReposoitory.GetPeriodSummary(start.Value, end.Value);
                return new OkObjectResult(periodSummary);
            }
            return new BadRequestObjectResult("Bad Request");
        }

    }
}