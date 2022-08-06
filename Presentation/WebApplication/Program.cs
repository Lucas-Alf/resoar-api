using Application.IoC;
using Domain.Utils;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.IoC;
using Microsoft.EntityFrameworkCore;
using WebApplication.Config;

// Title showed in the swagger UI.
var swaggerTitle = "RESOAR - API";

// Validate environment variables.
EnvironmentManager.Validate();

// Application builder
var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Add Entity Framework DbContext
builder.Services.AddEntityFramework();

// Add Repositories
builder.Services.AddRepositories();

// Add Services
builder.Services.AddServices();

// Add Authentication
builder.Services.AddJwtAuthentication();

// Add Controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(swaggerTitle);

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

app.UseAuthentication();

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
