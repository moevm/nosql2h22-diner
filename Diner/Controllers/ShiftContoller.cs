using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.ModelServices;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ShiftController
{
    private readonly ShiftService _shiftService;

    public ShiftController(ShiftService shiftService)
    {
        _shiftService = shiftService;
    }
    
    [HttpPost("get-shifts")]
    public async Task<IEnumerable<Shift>> GetShifts(WeekDto weekDto)
    {
        return await _shiftService.FindByWeekAndDay(weekDto.Hours, weekDto.DayOfWeek);
    }
}