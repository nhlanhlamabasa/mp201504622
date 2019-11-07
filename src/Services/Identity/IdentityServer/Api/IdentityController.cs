using HotelSystem.SharedKernel;
using HotelSystem.SharedKernel.ViewModels;
using IdentityModel;
using IdentityServer.API.Data;
using IdentityServer.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityServer.Api
{
    [Produces("application/json")]
    [Route("api/Identity")]
    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ICollection<string>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(NotFoundObjectResult))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkObjectResult))]
        public async Task<IActionResult> Get([FromQuery]Guid? bookerId, [FromQuery]bool? fullName, [FromQuery]string email)
        {
            if(fullName != null)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(bookerId.ToString());
                if(user == null)
                {
                    return new NotFoundObjectResult("Not found");
                }
                FullName name = new FullName(user.FirstName, user.LastName);
                return new OkObjectResult(name);
            }
            if(!string.IsNullOrEmpty(email))
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new NotFoundObjectResult("Not found");
                }
                return new OkObjectResult(user.Id);
            }
            else
            {
                return new BadRequestObjectResult("Error");
            }
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ICollection<string>))]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser(model.Email, model.Username, model.PhoneNumber,
                    new FullName(model.Name, model.LastName),
                    new ContactDetails(model.PostalCode),
                    new Address(model.Country, model.City, model.Province, model.Street));

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    ICollection<Claim> Claims = new List<Claim>()
                    {
                        new Claim(JwtClaimTypes.Id, user.Id),
                        new Claim(JwtClaimTypes.Email, model.Email),
                        new Claim(JwtClaimTypes.Role, UserRoles.GUEST),
                        new Claim(JwtClaimTypes.Name, user.FirstName),
                        new Claim(JwtClaimTypes.FamilyName, user.LastName)
                    };
                    await _userManager.AddToRoleAsync(user, UserRoles.GUEST);
                    await _userManager.AddClaimsAsync(user, Claims);

                    return Ok();
                }
                else
                {
                    var errors = AddErrors(result);
                    return BadRequest(errors);
                }

            }
            return BadRequest(ModelState);
        }

        private ICollection<string> AddErrors(IdentityResult result)
        {
            ICollection<string> errors = new List<string>();
            foreach(var error in result.Errors)
            {
                errors.Add(error.Description);
            }
            return errors;
        }
    }
}
