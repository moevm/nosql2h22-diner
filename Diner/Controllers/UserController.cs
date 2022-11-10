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
[Authorize]
[Route("/api/[controller]")]
public class UserController : Controller
{
    private readonly UserService _userService;
    private readonly ShiftService _shiftService;

    public UserController(UserService userService, ShiftService shiftService)
    {
        _userService = userService;
        _shiftService = shiftService;
    }
    
    [HttpPost("create-user")]
    public async Task<User> CreateUser(UserDto userDto)
    {
        return await _userService.CreateUserWithDefaults(userDto);
    }

    [HttpPost("get-users")]
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userService.FindAllAsync();
    }
}