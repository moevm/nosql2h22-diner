using System.Security.Claims;
using System.Text.RegularExpressions;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ServicesLib.ModelServices;
using ServicesLib.Services;
using ServicesLib.Services.Outputs;
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
    private readonly ExcelService _excelService;
    private static readonly List<UserRole> UserRoles = new List<UserRole>() { UserRole.Admin, UserRole.Manager };

    public UserController(UserService userService, ShiftService shiftService, WeekService weekService, ExcelService excelService)
    {
        _userService = userService;
        _shiftService = shiftService;
        _weekService = weekService;
        _excelService = excelService;
    }
    
    [HttpPost]
    [AllowAnonymous]
    [Route("create-user", Name = "createUser")]
    [ProducesResponseType(typeof(User), 200)]
    public async Task<IActionResult> CreateUser(CreateUserDto userDto)
    {
        if (!Enum.TryParse<UserRole>(HttpContext.User.FindFirstValue(ClaimTypes.Role), out var role) ||
            !UserRoles.Contains(role)) return Unauthorized();
        try
        {
            return Ok(await _userService.CreateUserWithDefaults(userDto));
        }
        catch (Exception e)
        {
            return StatusCode(409, "{  \"status\": 409, \"payload\": \" User is already exists! \" }");
        }
    }

    [HttpGet]
    [Route("get-users", Name = "getUsers")]
    [AllowAnonymous]
    public async Task<IEnumerable<User>> GetUsers(string? nameOrLogin, string? hoursMask, DateTime? date)
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
        if (!ObjectId.TryParse(id, out var val)) return StatusCode(400, "{  \"status\": 400, \"payload\": \" Wrong Id! \" }");
        var user = await _userService.FindOneAsync(id);
        return user != null ? Ok(user) : Json(null);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("update-user", Name = "updateUser")]
    [ProducesResponseType(typeof(User), 200)]
    public async Task<IActionResult> UpdateUser(UpdateUserDto userDto)
    {
        if (!Enum.TryParse<UserRole>(HttpContext.User.FindFirstValue(ClaimTypes.Role), out var role) ||
            !UserRoles.Contains(role)) return Unauthorized();
        if (!ObjectId.TryParse(userDto.Id, out var val)) return StatusCode(400, "{  \"status\": 400, \"payload\": \" Wrong Id! \" }");
        var user = await _userService.FindOneAsync(userDto.Id);
        if (user == null) return StatusCode(404, "{  \"status\": 400, \"payload\": \" Not Found! \" }");
        var newUser = new User()
        {
            Id = user.Id,
            Login = userDto.Login ?? user.Login,
            Role = userDto.Role ?? user.Role,
            FullName = userDto.FullName ?? user.FullName,
            ShiftId = user.ShiftId,
            Status = userDto.Status ?? user.Status,
        };
        await _userService.UpdateAsync(user.Id, newUser);
        return Ok(newUser);
    }
    
    [HttpPost]
    [Route("get-user-excel", Name = "getUserExcel")]
    public async Task<IActionResult> GetResourcesExcel()
    {
        var users = await _userService.FindAllAsync();
        var sheet = ExcelBuilder.FormatDataToExcel(users.Select(x => new ExcelUsersOutput(x)));
        sheet.SheetName = "Users report";
        HttpContext.Response.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        return Ok(_excelService.CreateNewExcelBuilder().AddSheet(sheet).AsStream());
    }
}