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
    public class PeriodSummaryViewComponent : ViewComponent
    {
        private readonly IManagerClient _managerClient;

        public PeriodSummaryViewComponent(IManagerClient managerClient)
        {
            _managerClient = managerClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                DateTime end = DateTime.Today;
                DateTime start = end.AddDays(-60);
                HttpResponseMessage responseMessage = await _managerClient.GetPeriodSummary(start, end);
                if (responseMessage.IsSuccessStatusCode)
                {
                    PeriodSummary complaints = await responseMessage.Content.ReadAsAsync<PeriodSummary>();
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
