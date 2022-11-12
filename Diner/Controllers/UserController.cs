using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
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
    
    [HttpPost("create-user")]
    [SwaggerOperation(OperationId = "create-payment")]
    public async Task<User> CreateUser(UserDto userDto)
    {
        return await _userService.CreateUserWithDefaults(userDto);
    }

    [HttpPost("get-users")]
    [SwaggerOperation(OperationId = "create-payment")]
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userService.FindAllAsync();
    }
    
    [HttpPost("get-user")]
    [SwaggerOperation(OperationId = "create-payment")]
    public async Task<User>? GetUser(string id)
    {
        return await _userService.FindOneAsync(id) ?? throw new HttpRequestException("User not found", null, HttpStatusCode.NotFound);;
    }
}