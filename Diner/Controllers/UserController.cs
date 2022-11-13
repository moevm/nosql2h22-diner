using System.Text.RegularExpressions;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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
    private readonly WeekService _weekService;

    public UserController(UserService userService, ShiftService shiftService, WeekService weekService)
    {
        _userService = userService;
        _shiftService = shiftService;
        _weekService = weekService;
    }
    
    [HttpPost]
    [AllowAnonymous]
    [Route("create-user", Name = "createUser")]
    public async Task<User> CreateUser(UserDto userDto)
    {
        return await _userService.CreateUserWithDefaults(userDto);
    }

    [HttpGet]
    [Route("get-users", Name = "getUsers")]
    [AllowAnonymous]
    public async Task<IEnumerable<User>> GetUsers(string? nameOrLogin, string? hoursMask, DateTime date)
    {
        if (string.IsNullOrEmpty(nameOrLogin) && string.IsNullOrEmpty(hoursMask) && date == null) return await this._userService.FindAllAsync();
        FilterDefinition<User> Filter(string name) => Builders<User>.Filter.Regex(name, $"/{nameOrLogin}/i");
        List<Week>? weeks = null;
        if (!string.IsNullOrEmpty(hoursMask) && date != null)
        {
            var weeksFilter = _weekService.FindWeeksByHours(Convert.ToInt32(hoursMask, 2), date);
            weeks = await _weekService.WhereManyAsync(weeksFilter);
        }
        var users = await _userService.WhereManyAsync(Filter("FullName") | Filter("Login"));
        return weeks == null ? users : users.Where(x => weeks.Select(week => week.ShiftId).Contains(x.ShiftId));
    }
    
    [HttpGet]
    [Route("get-user", Name = "getUser")]
    [ProducesResponseType(typeof(User), 200)]
    public async Task<IActionResult>? GetUser(string id)
    {
        var user = await _userService.FindOneAsync(id);
        return user != null ? Ok(user) : Json(null);
    }
}