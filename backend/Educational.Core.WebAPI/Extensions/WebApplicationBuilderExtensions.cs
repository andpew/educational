using Educational.Core.BLL.MappingProfiles;
using Educational.Core.BLL.Services;
using Educational.Core.BLL.Services.Abstract;
using Educational.Core.DAL;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace Educational.Core.WebAPI.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        AddLogger(builder);
        AddDatabase(builder);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMvc();

        AddAutoMapper(builder);

        AddCustomServices(builder);
    }

    private static void AddLogger(WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        ILogger logger = new LoggerConfiguration()
            .WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Information
            )
            .WriteTo.File(
                formatter: new CompactJsonFormatter(),
                path: "Logs/log-errors.json",
                restrictedToMinimumLevel: LogEventLevel.Error,
                rollingInterval: RollingInterval.Day
            )
            .CreateLogger();
        builder.Logging.AddSerilog(logger);
        builder.Services.AddSingleton(logger);
    }

    private static void AddDatabase(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(o =>
        {
            o.UseNpgsql(builder.Configuration.GetConnectionString("Educational"))
                .EnableDetailedErrors();
        });
    }

    private static void AddAutoMapper(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        });
        Assembly.GetExecutingAssembly();
    }

    private static void AddCustomServices(WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthService, AuthService>();
    }
}
