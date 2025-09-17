// Controllers/SupersetTokenController.cs
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt; // Asegúrate de que esta dependencia esté instalada
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class SupersetTokenController : ControllerBase
{
    // ⚠️ 1. REEMPLAZA ESTO: Tu clave secreta GENERADA (de 64 caracteres)
    // DEBE COINCIDIR con la de Superset
    private const string GUEST_TOKEN_SECRET = "c9e4a8b7d1f0e3c2a1b5d6f7e8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7";

    // ⚠️ 2. REEMPLAZA ESTO: El UUID del dashboard de Superset
    private const string DASHBOARD_UUID = "fb45c317-2c88-43ae-a744-f13dd11b09c3";

    // Clase simple para el payload de la petición POST (aunque usaremos un valor constante para DashboardId en este ejemplo)
    public class GuestTokenPayload
    {
        // En un escenario real, podrías pasar el DashboardId aquí si tu app tiene varios.
        public string DashboardId { get; set; } = DASHBOARD_UUID;
    }

    // Endpoint de Prueba (Hello World)
    [HttpGet("hello-world")]
    public IActionResult HelloWorld()
    {
        // ... (Este método ya funciona) ...
        var response = new
        {
            message = "Hello from .NET Backend!",
            instructions = "Ready to generate JWT token for Superset."
        };
        return Ok(response);
    }

    // Endpoint REAL para el Embedding
    [HttpPost("guest-token")]
    public IActionResult GenerateGuestToken([FromBody] GuestTokenPayload payload)
    {
        // 1. Definir los recursos y usuario (Superset necesita estos claims en el payload)
        var resources = new[]
        {
            new { type = "dashboard", id = payload.DashboardId } // Usa el ID del payload (o el constante)
        };

        var user = new
        {
            username = "embed-user",
            first_name = "Embedded",
            last_name = "User"
        };

        // 2. Crear los Claims
        var claims = new List<Claim>
        {
            // Serializar objetos complejos a JSON strings para los claims
            new Claim("user", JsonSerializer.Serialize(user), JsonClaimValueTypes.Json),
            new Claim("resources", JsonSerializer.Serialize(resources), JsonClaimValueTypes.Json),
            new Claim("rls", JsonSerializer.Serialize(Array.Empty<object>()), JsonClaimValueTypes.Json) // Row Level Security
        };

        // 3. Firmar el token con la clave secreta de Superset
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GUEST_TOKEN_SECRET));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5), // Token expira en 5 minutos (tiempo recomendado)
            signingCredentials: credentials);

        var guestToken = new JwtSecurityTokenHandler().WriteToken(token);

        // 4. Devolver el token a Angular
        return Ok(new { token = guestToken });
    }
}