namespace Booking.API.Models
{
#pragma warning disable
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Collections.Generic;

    public static class Helpers
    {

        public static ICollection<string> AddErrors(ModelStateDictionary result)
        {
            ICollection<string> errors = new List<string>();
            foreach (var modelState in result.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    errors.Add(modelError.ErrorMessage);
                }
            }
            return errors;
        }
    }
}
