using Microsoft.AspNetCore.Builder;
using CAP.API.Middleware;

namespace CAP.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder applicationBuilder) =>
        applicationBuilder.UseMiddleware<GlobalErrorHandlingMiddleware>();
}