using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SupersetTokenController : ControllerBase
{
    // 🚨 REEMPLAZA ESTAS CONSTANTES con tus valores reales cuando implementes el JWT
    private const string GUEST_TOKEN_SECRET = "c9e4a8b7d1f0e3c2a1b5d6f7e8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7";
    private const string DASHBOARD_UUID = "328ca164-9cf7-48e5-b178-ad2ff4a141bc";

    // Endpoint de Prueba (Hello World)
    [HttpGet("hello-world")]
    public IActionResult HelloWorld()
    {
        var response = new
        {
            message = "Hello from .NET Backend!",
            instructions = "Ready to generate JWT token for Superset."
        };
        return Ok(response);
    }

    // Endpoint REAL para el Embedding
    [HttpPost("guest-token")]
    public IActionResult GenerateGuestToken()
    {
        // ⚠️ La lógica JWT se pegará aquí

        // Simplemente devolvemos un error 501 (No implementado) por ahora.
        return StatusCode(501, new { error = "JWT generation not yet implemented." });
    }
}