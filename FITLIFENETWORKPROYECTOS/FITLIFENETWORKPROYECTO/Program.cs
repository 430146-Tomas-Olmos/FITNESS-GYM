using FITLIFENETWORKPROYECTO.Models.Context;
using FITLIFENETWORKPROYECTO.Repositories;
using FITLIFENETWORKPROYECTO.Repositorys;
using FITLIFENETWORKPROYECTO.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configuración de Entity Framework Core y SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<FITLIFENETWORKDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IRepository, RepositoryFitLife>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();