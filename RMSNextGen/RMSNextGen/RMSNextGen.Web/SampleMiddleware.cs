using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RMSNextGen.Web
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SampleMiddleware
    {
        private readonly RequestDelegate _next;

        public SampleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SampleMiddlewareExtensions
    {
        public static IApplicationBuilder UseSampleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SampleMiddleware>();
        }
    }
}
