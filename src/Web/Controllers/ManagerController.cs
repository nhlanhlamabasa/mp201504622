namespace Web.Controllers
{
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Web.Extensions;
    using Web.Interfaces;

    [Authorize(Policy = "MANAGER")]
    public class ManagerController : Controller
    {
        private readonly IManagerClient _managerClient;
        private readonly IBookingClient _bookingClient;

        public ManagerController(IManagerClient managerClient, IBookingClient bookingClient)
        {
            _managerClient = managerClient;
            _bookingClient = bookingClient;
        }

        [HttpGet]
        public async Task<IActionResult> Complaints()
        {
            try
            {
                HttpResponseMessage responseMessage = await _managerClient.GetComplaints();
                if (responseMessage.IsSuccessStatusCode)
                {
                    ICollection<ComplaintViewModel> complaints = await responseMessage.Content.ReadAsAsync<ICollection<ComplaintViewModel>>();
                    return View(complaints);
                }
                else
                {
                    TempData["_alert.type"] = "danger";
                    TempData["_alert.title"] = "Error";
                    TempData["_alert.body"] = responseMessage.ReasonPhrase;
                    return View();
                }

            }
            catch(Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddRoom(Guid HotelId)
        {
            try
            {
                HotelViewModel hotel;
                HttpResponseMessage responseMessage = await _bookingClient.GetHotel(HotelId);
                if(responseMessage.IsSuccessStatusCode)
                {
                    hotel = await responseMessage.Content.ReadAsAsync<HotelViewModel>();
                    RoomViewModel model = new RoomViewModel()
                    {
                        HotelId = HotelId,
                        CreationTime = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        RoomNumber = hotel.NumberOfRooms + 1,
                        Id = Guid.NewGuid(),
                        HotelName = hotel.Name
                    };
                    ViewData["HotelName"] = hotel.Name;
                    return View(model);
                }
                else
                {
                    if(responseMessage.StatusCode == HttpStatusCode.NotFound || 
                        responseMessage.StatusCode == HttpStatusCode.BadRequest)
                    {
                        string message = await responseMessage.Content.ReadAsStringAsync();
                        return RedirectToAction(nameof(Rooms), new { HotelId })
                            .WithDanger("Error", message);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Rooms), new { HotelId }).WithDanger("Error", responseMessage.ReasonPhrase);
                    }
                }
            }
            catch(Exception e)
            {
                return RedirectToAction(nameof(Rooms), new { HotelId }).WithDanger("Error", e.Message);
            }
        }

        [HttpGet]
        public IActionResult AddHotel()
        {   
            HotelViewModel model = new HotelViewModel()
            {
                Id = Guid.NewGuid(),
                ManagerId = Helpers.Claims.GetUserID(HttpContext.User.Claims),
                CreationTime = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoom(Guid HotelId,RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage = await _managerClient.AddRoom(HotelId, model);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Hotels)).WithSuccess("Success", "Hotel Deleted.");
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
                    return View(model).WithDanger("Error", e.Message);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHotel(HotelViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage = await _managerClient.AddHotel( model);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Hotels)).WithSuccess("Success", "Hotel Deleted.");
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
                catch(Exception e)
                {
                    return View(model).WithDanger("Error", e.Message);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoom(Guid HotelId, Guid RoomId)
        {
            try
            {
                HttpResponseMessage responseMessage = await _managerClient.RemoveRoom(HotelId, RoomId);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return View().WithSuccess("Success", "Room Deleted.");
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
        public IActionResult Delete_Hotel(Guid Id)
        {
            return RedirectToAction(nameof(DeleteHotel), new { Id });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteHotel(Guid Id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _managerClient.RemoveHotel(Id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return View().WithSuccess("Success", "Hotel Deleted.");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoom(Guid HotelId, Guid RoomId, RoomViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage = await _managerClient.RemoveRoom(HotelId, RoomId, model);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Hotels)).WithSuccess("Success", "Room Removed");
                    }
                    else
                    {
                        if (responseMessage.StatusCode == HttpStatusCode.BadRequest || responseMessage.StatusCode == HttpStatusCode.NotFound)
                        {
                            string message = await responseMessage.Content.ReadAsStringAsync();
                            return View(model).WithDanger("Error", message);
                        }
                        else
                        {
                            return View(model).WithDanger("Error", responseMessage.ReasonPhrase);
                        }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHotel(Guid Id, HotelViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage = await _managerClient.RemoveHotel(Id, model);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Hotels)).WithSuccess("Success", "Hotel Removed");
                    }
                    else
                    {
                        if (responseMessage.StatusCode == HttpStatusCode.BadRequest || responseMessage.StatusCode == HttpStatusCode.NotFound)
                        {
                            string message = await responseMessage.Content.ReadAsStringAsync();
                            return View(model).WithDanger("Error", message);
                        }
                        else
                        {
                            return View(model).WithDanger("Error", responseMessage.ReasonPhrase);
                        }
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
        public async Task<IActionResult> HotelDetails(Guid Id)
        {
            try
            {
                HttpResponseMessage responseMessage = await _bookingClient.GetHotel(Id);
                if (responseMessage.IsSuccessStatusCode)
                {
                    HotelViewModel Hotel = await responseMessage.Content.ReadAsAsync<HotelViewModel>();
                    return View(Hotel);
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
        public async Task<IActionResult> RoomDetails(Guid HotelId, Guid RoomId)
        {
            try
            {
                HttpResponseMessage responseMessage = await _managerClient.GetRoom(HotelId,RoomId,WithDetails: true);
                if (responseMessage.IsSuccessStatusCode)
                {
                    RoomViewModel room = await responseMessage.Content.ReadAsAsync<RoomViewModel>();
                    return View(room);
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
        public async Task<IActionResult> EditRoom(Guid HotelId, Guid RoomId)
        {
            try
            {
                HttpResponseMessage responseMessage = await _managerClient.GetRoom(HotelId,RoomId);
                if (responseMessage.IsSuccessStatusCode)
                {
                    RoomViewModel room = await responseMessage.Content.ReadAsAsync<RoomViewModel>();
                    return View(room);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoom(Guid HotelId, Guid RoomId, RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage = await _managerClient.EditRoom(HotelId, RoomId, model);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Hotels)).WithSuccess("Success", "Hotel Updated");
                    }
                    else
                    {
                        if (responseMessage.StatusCode == HttpStatusCode.BadRequest || responseMessage.StatusCode == HttpStatusCode.NotFound)
                        {
                            string message = await responseMessage.Content.ReadAsStringAsync();
                            return View(model).WithDanger("Error", message);
                        }
                        else
                        {
                            return View(model).WithDanger("Error", responseMessage.ReasonPhrase);
                        }
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
        public async Task<IActionResult> Hotels()
        {
            try
            {
                HttpResponseMessage responseMessage = await _managerClient.GetHotels();
                if(responseMessage.IsSuccessStatusCode)
                {
                    ICollection<HotelViewModel> hotels = await responseMessage.Content.ReadAsAsync<ICollection<HotelViewModel>>();
                    return View(hotels);
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

        [HttpGet]
        public async Task<IActionResult> Rooms(Guid HotelId)
        {
            try
            {
                ICollection<RoomViewModel> Rooms = await _bookingClient.GetRooms(HotelId);
                return View(Rooms);
            }
            catch(Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }

        [HttpGet]
        public IActionResult AddAmenity()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAmenity(AmenityViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseMessage = await _managerClient.AddAmenity(model);
                    if(responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(ManagerController.Amenities))
                            .WithSuccess("Success", "Amenity added.");
                    }
                    else
                    {
                        if(responseMessage.StatusCode == HttpStatusCode.BadRequest || responseMessage.StatusCode == HttpStatusCode.NotFound)
                        {
                            string message = await responseMessage.Content.ReadAsStringAsync();
                            return View(model).WithDanger("Error", message);
                        }
                        else
                        {
                            return View(model).WithDanger("Error", responseMessage.ReasonPhrase);
                        }
                    }
                }
                catch(Exception e)
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
        public async Task<IActionResult> Amenities()
        {
            try
            {
                HttpResponseMessage responseMessage = await _managerClient.GetAmenities();
                if (responseMessage.IsSuccessStatusCode)
                {
                    ICollection<AmenityViewModel> amenities = await responseMessage.Content.ReadAsAsync<ICollection<AmenityViewModel>>();
                    return View(amenities);
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
            catch(Exception e)
            {
                return View().WithDanger("Error", e.Message);
            }
        }
    }
}
