using Educational.Core.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.ConfigureServices();

builder.WebHost.UseUrls("http://*:5000");

var app = builder.Build();

app.ApplyDbMigrations();

app.UseExceptionMiddleware();

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:4200");
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowCredentials();
    builder.WithExposedHeaders("Token-Expired");
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(o => o.RouteTemplate = "api/swagger/{documentName}/swagger.json");
    app.UseSwaggerUI(o => o.RoutePrefix = "api/swagger");
};

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
