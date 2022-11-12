using System.Net;
using System.Security.Claims;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.ModelServices;
using Swashbuckle.Swagger.Annotations;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController: Controller
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login", Name = "logIn")]
    public async Task<IResult> Authenticate(AuthDto authDto)
    {
        var authInfo = await _userService.AuthenticateUser(authDto.Login, authDto.Password);
        if (authInfo == null) return Results.Unauthorized();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, authDto.Login),
            new Claim("Id", authInfo.UserId),
            new Claim(ClaimTypes.Role, "Administrator"),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(50),
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity), 
            authProperties);
        return Results.Ok();
    }
    
    [HttpGet]
    [Route("who-am-i", Name = "whoAmI")]
    public async Task<User?> WhoAmI()
    {
        var id = HttpContext.User.FindFirstValue("Id") ?? "";
        var user = await this._userService.FindUserById(id);
        return user ?? throw new HttpRequestException("User not found", null, HttpStatusCode.NotFound);
    }
}