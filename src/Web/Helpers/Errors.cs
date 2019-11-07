namespace Web.Helpers
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Helpers" />
    /// </summary>
    public static class Errors
    {
        /// <summary>
        /// The AddErrors
        /// </summary>
        /// <param name="result">The result<see cref="ModelStateDictionary"/></param>
        /// <returns>The <see cref="ICollection{string}"/></returns>
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
