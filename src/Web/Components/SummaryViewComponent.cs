using HotelSystem.SharedKernel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Interfaces;

namespace Web.Components
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly IManagerClient _managerClient;

        public SummaryViewComponent(IManagerClient managerClient)
        {
            _managerClient = managerClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                HttpResponseMessage responseMessage = await _managerClient.GetSummary();
                if (responseMessage.IsSuccessStatusCode)
                {
                    SummaryViewModel complaints = await responseMessage.Content.ReadAsAsync<SummaryViewModel>();
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
