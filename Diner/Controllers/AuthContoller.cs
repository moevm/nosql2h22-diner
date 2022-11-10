using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DomainLib.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServicesLib.ModelServices;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("token")]
    public async Task<IResult> Authenticate(AuthDto authDto)
    {
        if (!await this._userService.AuthenticateUser(authDto.Login, authDto.Password)) return Results.Unauthorized();
        var issuer = "theBoris";
        var audience = "theBoris";
        var key = Encoding.ASCII.GetBytes
            ("thereIsCoolKeyFromConfigs");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, authDto.Login),
                new Claim(JwtRegisteredClaimNames.Email, authDto.Login),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(600),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return Results.Ok(stringToken);
    }
}