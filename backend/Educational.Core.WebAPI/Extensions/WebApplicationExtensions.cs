using Educational.Core.DAL;
using Microsoft.EntityFrameworkCore;

namespace Educational.Core.WebAPI.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void ApplyDbMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DataContext>();
            db.Database.Migrate();
        }
    }
}
