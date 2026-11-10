using FITLIFENETWORKPROYECTO.Middleware;
using FITLIFENETWORKPROYECTO.Models.Context;
using FITLIFENETWORKPROYECTO.Repositories;
using FITLIFENETWORKPROYECTO.Repositorys;
using FITLIFENETWORKPROYECTO.Repositorys.Interfaces;
using FITLIFENETWORKPROYECTO.Services;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<FITLIFENETWORKDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepository, RepositoryFitLife>();
builder.Services.AddScoped<IServices, ServicesFIT>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();