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
    public async Task<IActionResult> Authenticate(AuthDto authDto)
    {
        var authInfo = await _userService.AuthenticateUser(authDto.Login, authDto.Password);
        if (authInfo == null) return Unauthorized();
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
        return Ok();
    }
    
    [HttpPost]
    [Route("logout", Name = "logOut")]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }

    [HttpGet]
    [Route("who-am-i", Name = "whoAmI")]
    [ProducesResponseType(typeof(User), 200)]
    public async Task<IActionResult> WhoAmI()
    {
        var id = HttpContext.User.FindFirstValue("Id") ?? null;
        var user = await this._userService.FindUserById(id);
        HttpContext.Response.Headers.Add("Content-Type", "application/json");
        return user != null ? Ok(user) : Json(null);
    }
}