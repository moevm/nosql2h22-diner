using System.Net;
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
public class ShiftController
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
    public async Task<Shift?> GetShift(string id)
    {
        return await _shiftService.FindOneAsync(id) ??
               throw new HttpRequestException("User not found", null, HttpStatusCode.NotFound);
    }
}
