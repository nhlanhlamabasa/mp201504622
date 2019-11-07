using HotelSystem.SharedKernel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Interfaces;

namespace Web.Components
{
    public class ComplaintsViewComponent : ViewComponent
    {
        private readonly IManagerClient _managerClient;

        public ComplaintsViewComponent(IManagerClient managerClient)
        {
            _managerClient = managerClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                HttpResponseMessage responseMessage = await _managerClient.GetComplaints();
                if (responseMessage.IsSuccessStatusCode)
                {
                    string json = await responseMessage.Content.ReadAsStringAsync();
                    ICollection<ComplaintViewModel> complaints = JsonConvert.DeserializeObject<ICollection<ComplaintViewModel>>(json);
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
            catch (Exception e)
            {
                TempData["_alert.type"] = "danger";
                TempData["_alert.title"] = "Error";
                TempData["_alert.body"] = e.Message;
                return View();
            }
        }
    }
}
