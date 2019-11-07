namespace Web.Controllers
{
    using HotelSystem.SharedKernel.ViewModels;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Web.Extensions;
    using Web.Interfaces;
    using Web.Models;

    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IIdentityClient _identityClient;

        private readonly ILogger<AccountController> _logger;

        public AccountController(IHttpContextAccessor contextAccessor, IIdentityClient identityClient, 
            ILogger<AccountController> logger)
        {
            _contextAccessor = contextAccessor;
            _identityClient = identityClient;
            _logger = logger;
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        public IActionResult Login(string returnUrl)
        {
            _logger.LogInformation("Logging in");
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    ViewBag.Manager = true;
                    return Challenge(new AuthenticationProperties
                    {
                        RedirectUri = returnUrl,
                        AllowRefresh = true
                    }, "oidc");
                }
                else
                {
                    ViewBag.Manager = true;
                    return Challenge(new AuthenticationProperties
                    {
                        RedirectUri = "/Home/Index",
                        AllowRefresh = true
                    }, "oidc");
                }
            }
            else
            {
                ViewBag.Manager = true;
                return Challenge(new AuthenticationProperties
                {
                    RedirectUri = "/Home/Index",
                    AllowRefresh = true
                }, "oidc");
            }
        }

        public IActionResult Logout()
        {
            _logger.LogInformation("Logging out");
            DeleteCookies();
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "/Home/Index"
            },  "Cookies","oidc");
        }

        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Register view");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation(LoggingEvents.InsertItem, $"Register user: {model.Name}");
                HttpResponseMessage responseMessage = await _identityClient.Register(model);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return Redirect("/Account/Login").WithSuccess("Success!", "Account Created.");
                }
                else
                {
                    ICollection<string> errors = await responseMessage.Content.ReadAsAsync<ICollection<string>>();
                    AddErrors(errors);
                    _logger.LogWarning(LoggingEvents.ItemBadRequest, $"Register Fail Errors: {errors}");
                    return View(model);
                }
            }
            else
            {   
                return View(model);
            }
        }

        private void AddErrors(ICollection<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private void DeleteCookies()
        {
            HttpContext.Session.Clear();
            foreach(string key in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(key);
            }
        }
    }
}
