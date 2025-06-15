using CalculadoraSQIA.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CalculadoraSQIA.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiResponseMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiResponseMiddleware>();
        }
    }
}
