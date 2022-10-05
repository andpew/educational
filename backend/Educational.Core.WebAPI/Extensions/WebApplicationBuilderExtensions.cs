using Educational.Core.BLL.Services;
using Educational.Core.BLL.Services.Abstract;
using Educational.Core.DAL;
using Microsoft.EntityFrameworkCore;

namespace Educational.Core.WebAPI.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<DataContext>(o =>
        {
            o.UseNpgsql(builder.Configuration.GetConnectionString("Educational"))
                .EnableDetailedErrors();
        });

        builder.Services.AddMvc();

        builder.Services.AddScoped<IUserService, UserService>();
    }
}
