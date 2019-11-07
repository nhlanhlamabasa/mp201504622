namespace Booking.API.Exceptions
{
#pragma warning disable
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;


    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;


        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }


        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (httpContext.Response.HasStarted)
                {
                    throw;
                }
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "application/json";
                var data = new { Message = ex.Message };
                var json = JsonConvert.SerializeObject(data);
                await httpContext.Response.WriteAsync(json);
                return;


            }
        }
    }


    public static class ExceptionMiddlewareExtensions
    {

        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
