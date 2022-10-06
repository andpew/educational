using Educational.Core.WebAPI.Middlewares;

namespace Educational.Core.WebAPI.Extensions;

public static class ApplicationBuilderExtensionsExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}
