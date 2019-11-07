namespace Web.Middleware
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    /// <summary>
    /// Defines the <see cref="ExceptionMiddleware" />
    /// </summary>
    public class ExceptionMiddleware
    {
        /// <summary>
        /// Defines the _next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next<see cref="RequestDelegate"/></param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// The InvokeAsync
        /// </summary>
        /// <param name="context">The context<see cref="HttpContext"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            //First, get the incoming request
            var request = await FormatRequest(context.Request);

            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                //Format the response from the server
                var response = await FormatResponse(context.Response);

                //TODO: Save log to chosen datastore

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        /// <summary>
        /// The FormatRequest
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableRewind();

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        /// <summary>
        /// The FormatResponse
        /// </summary>
        /// <param name="response">The response<see cref="HttpResponse"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }

    /// <summary>
    /// Defines the <see cref="ExceptionMiddlewareExtensions" />
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// The UseExceptionMiddleware
        /// </summary>
        /// <param name="builder">The builder<see cref="IApplicationBuilder"/></param>
        /// <returns>The <see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
