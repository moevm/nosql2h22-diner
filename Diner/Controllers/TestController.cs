using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServicesLib.ModelServices;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TestController: Controller
{
    private readonly UserService _userService;

    public TestController(UserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("auth")]
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

    [HttpPost("create-user")]
    public async Task<User> CreateUser(UserDto userDto)
    { 
        return await _userService.CreateUserWithDefaults(userDto);
    }
    
    
    
    [Authorize]
    [HttpPost("get-users")]
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userService.FindAllAsync();
    }
}