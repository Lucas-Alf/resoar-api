using Application.IoC;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Title showed in the swagger UI.
var swaggerTitle = "Resoar - API";

// Add services to the container.
builder.Services.ApplicationServicesIoC();
builder.Services.InfrastructureORM<EntityFrameworkIoC>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerTitle, Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", swaggerTitle);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Run migrations on startup.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationContext>();
    context.Database.Migrate();
}

app.Run();
