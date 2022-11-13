using System.Net;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.ModelServices;
using Swashbuckle.Examples;
using Swashbuckle.Swagger.Annotations;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize]
public class ShiftController: Controller
{
    private readonly ShiftService _shiftService;

    public ShiftController(ShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    [HttpGet]
    [Route("get-shifts", Name = "getShifts")]
    public async Task<IEnumerable<Shift>> GetShifts([FromQuery(Name="hours")] int hours, [FromQuery(Name="hours")] DateTime dateTime, [FromQuery(Name="hours")] bool free)
    {
        return await _shiftService.FindBusyByWeekAndDay(hours, dateTime, free);
    }

    [HttpGet]
    [Route("get-shift", Name = "getShift")]
    [ProducesResponseType(typeof(Shift), 200)]
    public async Task<IActionResult> GetShift([FromQuery(Name="hours")]string id)
    { 
        var shift = await _shiftService.FindOneAsync(id);
        HttpContext.Response.Headers.Add("Content-Type", "application/json");
        return shift != null ? Ok(shift) : Json(null);
    }
}
