using System.Runtime.CompilerServices;
using System.Security.Claims;
using DomainLib.DTO;
using DomainLib.Models;
using Exceptionless.DateTimeExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServicesLib.ModelServices;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class WeekController: Controller
{
    private readonly UserService _userService;
    private readonly WeekService _weekService;
    private readonly ShiftService _shiftService;

    public WeekController(UserService userService, WeekService weekService, ShiftService shiftService)
    {
        _weekService = weekService;
        _userService = userService;
        _shiftService = shiftService;
    }
    
    /// <summary>
    /// @deprecated
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get-my-week", Name = "getMyWeek")]
    public async Task<IActionResult> GetMyWeek()
    {
        var userId = HttpContext.User.FindFirstValue("Id");
        if (userId == null) return Json(null);
        var user = await _userService.FindUserById(userId);
        if (user == null) return Json(null);
        var shift = await this._shiftService.FindOneAsync(user.ShiftId);
        var week = await this._weekService.WhereOneAsync(Builders<Week>.Filter.Where(x => x.Id == shift!.TargetWeekId));
        if (week != null) return Ok(week);
        return Json(null);
    }

    [HttpGet]
    [Route("get-week", Name = "getWeek")]
    public async Task<IActionResult> GetWeek(string id, DateTime? dateTime)
    {
        var user = await _userService.FindUserById(id);
        if (user == null) return Json(null);
        DateTime? date = null;
        if (dateTime != null) date = dateTime.Value.ToUniversalTime().StartOfWeek();
        var shift = await this._shiftService.FindOneAsync(user.ShiftId);
        var filter = date == null
            ? Builders<Week>.Filter.Where(x => x.Id == shift!.TargetWeekId)
            : Builders<Week>.Filter.Where(x => x.CreatedAt == date && x.ShiftId == user.ShiftId);
        var week = await this._weekService.WhereOneAsync(filter);
        if (week != null) return Ok(week);
        return Json(null);
    }

    [HttpPost]
    [Route("update-week", Name = "updateWeek")]
    public async Task<IActionResult> UpdateWeek(WeekDto weekDto)
    {
        var user = await _userService.FindUserById(weekDto.UserId);
        if (user == null) return Json(null);
        var newWeek = new Week()
        {
            Sunday = Convert.ToInt32(weekDto.Sunday, 2),
            Monday = Convert.ToInt32(weekDto.Monday, 2),
            Tuesday = Convert.ToInt32(weekDto.Tuesday, 2),
            Wednesday = Convert.ToInt32(weekDto.Wednesday, 2),
            Thursday = Convert.ToInt32(weekDto.Thursday, 2),
            Friday = Convert.ToInt32(weekDto.Friday, 2),
            Saturday = Convert.ToInt32(weekDto.Saturday, 2),
        };
        var date = weekDto.CreatedAt.AddHours(3).ToUniversalTime().StartOfWeek();
        
        var oldWeek = await _weekService.WhereOneAsync(Builders<Week>.Filter.Where(x =>
            x.CreatedAt == date && user.ShiftId == x.ShiftId));
        if (oldWeek != null)
        {
            newWeek.Id = oldWeek.Id;
            await _weekService.UpdateAsync(oldWeek.Id, newWeek);
        }
        else
            await this._weekService.CreateAsync(newWeek);
        
        var shift = await _shiftService.FindOneAsync(user.ShiftId);
        if (shift == null) throw new Exception("NO_SHIFT_FOUND");
        if (weekDto.CreatedAt.StartOfWeek() == DateTime.Now.StartOfWeek()) shift!.TargetWeekId = newWeek.Id;
        else shift.Weeks = new [] { newWeek.Id };
        newWeek.ShiftId = shift.Id;
        newWeek.CreatedAt = weekDto.CreatedAt.AddHours(3).ToUniversalTime().StartOfWeek();
        await this._weekService.UpdateAsync(newWeek.Id, newWeek);
        await this._shiftService.UpdateAsync(shift.Id, shift);
        return Ok();
    }
}