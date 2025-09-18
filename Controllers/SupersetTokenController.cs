using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class SupersetTokenController : ControllerBase
{
    private const string GUEST_TOKEN_SECRET = "c9e4a8b7d1f0e3c2a1b5d6f7e8a9b0c1d2e3f4a5b6c7d8e9f0a1b2c3d4e5f6a7";
    private const string DASHBOARD_UUID = "";

    public class GuestTokenPayload
    {
        public string DashboardId { get; set; } = DASHBOARD_UUID;
    }

    [HttpPost("guest-token")]
    public IActionResult GenerateGuestToken([FromBody] GuestTokenPayload payload)
    {
        var user = new Dictionary<string, object>
        {
            ["username"] = "report-viewer",
            ["first_name"] = "report-viewer",
            ["last_name"] = "report-viewer"
        };

        var resources = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> { ["type"] = "dashboard", ["id"] = payload.DashboardId }
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GUEST_TOKEN_SECRET));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;

        var jwtPayload = new JwtPayload(
            issuer: "Opencity",
            audience: "superset",
            claims: null,
            notBefore: now,
            expires: now.AddMinutes(5)
        );

        jwtPayload.Add("type", "guest");
        jwtPayload.Add("user", user);
        jwtPayload.Add("resources", resources);
        jwtPayload.Add("rls", Array.Empty<object>());
        jwtPayload.Add("rls_rules", Array.Empty<object>());
        jwtPayload.Add(JwtRegisteredClaimNames.Sub, user["username"].ToString());

        var token = new JwtSecurityToken(new JwtHeader(credentials), jwtPayload);
        var guestToken = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = guestToken });
    }
}
