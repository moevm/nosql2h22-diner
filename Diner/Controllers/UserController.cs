using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.ModelServices;
using Swashbuckle.Swagger.Annotations;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class UserController : Controller
{
    private readonly UserService _userService;
    private readonly ShiftService _shiftService;

    public UserController(UserService userService, ShiftService shiftService)
    {
        _userService = userService;
        _shiftService = shiftService;
    }
    
    [HttpPost]
    [Route("create-user", Name = "createUser")]
    public async Task<User> CreateUser(UserDto userDto)
    {
        return await _userService.CreateUserWithDefaults(userDto);
    }

    [HttpGet]
    [Route("get-users", Name = "getUsers")]
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userService.FindAllAsync();
    }
    
    [HttpGet]
    [Route("get-user", Name = "getUser")]
    [ProducesResponseType(typeof(User), 200)]
    public async Task<IActionResult>? GetUser(string id)
    {
        var user = await _userService.FindOneAsync(id);
        return user != null ? Ok(user) : NotFound("No such user");
    }
}