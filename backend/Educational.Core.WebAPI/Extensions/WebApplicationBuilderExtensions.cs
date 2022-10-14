using Educational.Core.BLL.Factories;
using Educational.Core.BLL.MappingProfiles;
using Educational.Core.BLL.Services;
using Educational.Core.BLL.Services.Interfaces;
using Educational.Core.Common.Options;
using Educational.Core.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using ILogger = Serilog.ILogger;

namespace Educational.Core.WebAPI.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        AddLogger(builder);
        AddDatabase(builder);
        AddSwagger(builder);

        builder.Services.AddMvc();

        builder.Services.AddHttpContextAccessor();
        AddJwt(builder);

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

    private static void AddSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme. " +
                    "\r\n\r\nExample: \"Bearer 12345abcdef\""
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });
    }

    private static void AddJwt(WebApplicationBuilder builder)
    {
        var jwtIssuerOptions = new JwtIssuerOptions();
        builder.Configuration.GetSection("JwtIssuerOptions").Bind(jwtIssuerOptions);
        builder.Services.AddSingleton(jwtIssuerOptions);

        var jwtRefreshTokenOptions = new JwtRefreshTokenOptions();
        builder.Configuration.GetSection("JwtRefreshTokenOptions").Bind(jwtRefreshTokenOptions);
        builder.Services.AddSingleton(jwtRefreshTokenOptions);

        // Added for fixing different jwt claims types
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtIssuerOptions.Key)),

            ValidateLifetime = true,

            ValidateIssuer = true,
            ValidIssuer = jwtIssuerOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtIssuerOptions.Audience
        };
        builder.Services.AddSingleton(tokenValidationParameters);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddScoped<JwtFactory>();
    }
}
