using FITLIFENETWORKPROYECTO.Middleware;
using FITLIFENETWORKPROYECTO.Models.Context;
using FITLIFENETWORKPROYECTO.Repositories;
using FITLIFENETWORKPROYECTO.Repositorys;
using FITLIFENETWORKPROYECTO.Repositorys.Interfaces;
using FITLIFENETWORKPROYECTO.Repository;
using FITLIFENETWORKPROYECTO.Repository.Interfaces;
using FITLIFENETWORKPROYECTO.Services;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<FITLIFENETWORKDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepository, RepositoryFitLife>();
builder.Services.AddScoped<IServices, ServicesFIT>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Configurar JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key no configurada"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/health", () => Results.Ok(new
{
    status = "ok",
    message = "FitLife Network backend activo",
    timestampUtc = DateTime.UtcNow
}));

app.MapGet("/api/health/db", async (FITLIFENETWORKDbContext db) =>
{
    try
    {
        var conectado = await db.Database.CanConnectAsync();
        var usuarios = conectado ? await db.Usuarios.CountAsync() : 0;
        var servidor = db.Database.GetDbConnection().DataSource;
        return Results.Ok(new { conectado, servidor, usuarios });
    }
    catch (Exception ex)
    {
        return Results.Json(new { conectado = false, error = ex.Message }, statusCode: 500);
    }
});

app.MapControllers();

app.Run();


