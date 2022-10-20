using DameChales.API.App.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace DameChales.API.App.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }
}