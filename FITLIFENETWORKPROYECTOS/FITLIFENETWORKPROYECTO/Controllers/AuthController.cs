using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FITLIFENETWORKPROYECTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Login endpoint that validates credentials and returns JWT token
        /// </summary>
        /// <param name="loginRequest">User login credentials</param>
        /// <returns>JWT token with user info</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] UserLoginDTO loginRequest)
        {
            try
            {
                if (loginRequest == null)
                    return BadRequest(new { message = "Se requiere objeto de LOGIN" });

                if (string.IsNullOrWhiteSpace(loginRequest.Nombre) || string.IsNullOrWhiteSpace(loginRequest.Contrasena))
                    return BadRequest(new { message = "Usuario y contraseña son requeridos" });

                // Validate user credentials
                var usuario = await _authRepository.ValidateUserAsync(loginRequest.Nombre, loginRequest.Contrasena);

                if (usuario == null)
                    return Unauthorized(new { message = "Usuario y/o contraseña incorrectos" });

                // Get user roles
                var roles = await _authRepository.GetUserRolesAsync(usuario.Id);

                // Create JWT token
                var token = GenerateJwtToken(usuario, roles);

                return Ok(new LoginResponseDTO
                {
                    Token = token,
                    UsuarioId = usuario.Id,
                    Nombre = usuario.Nombre,
                    Roles = roles
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        private string GenerateJwtToken(Models.Tablas.Usuario usuario, List<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key no configurada"));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim("usuario", usuario.Nombre)
            };

            // Add roles as claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var expiresInMinutes = int.TryParse(jwtSettings["ExpiresInMinutes"], out var minutes) ? minutes : 60;

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
