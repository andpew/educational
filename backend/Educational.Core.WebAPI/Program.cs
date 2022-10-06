using Educational.Core.DAL;
using Educational.Core.WebAPI.Extensions;
using Educational.Core.WebAPI.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

builder.WebHost.UseUrls("https://*:5000");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();
}

app.UseExceptionMiddleware();

app.UseCors(builder =>
{
    builder.AllowAnyHeader();
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(o => o.RouteTemplate = "api/swagger/{documentName}/swagger.json");
    app.UseSwaggerUI(o => o.RoutePrefix = "api/swagger");
};

app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(cfg =>
{
    cfg.MapControllers();
});

app.Run();
