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
    public async Task<IEnumerable<Shift>> GetShifts(GetShiftDto getShiftDto)
    {
        return await _shiftService.FindBusyByWeekAndDay(getShiftDto.Hours, getShiftDto.DayOfWeek, getShiftDto.Free);
    }

    [HttpGet]
    [Route("get-shift", Name = "getShift")]
    [ProducesResponseType(typeof(Shift), 200)]
    public async Task<IActionResult> GetShift(string id)
    { 
        var shift = await _shiftService.FindOneAsync(id);
        return shift != null ? Ok(shift) : NotFound("No such shift");
    }
}
