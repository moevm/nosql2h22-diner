using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.ModelServices;

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
    
    [HttpPost("get-shifts")]
    public async Task<IEnumerable<Shift>> GetShifts(GetShiftDto getShiftDto)
    {
        // Получение всех смен
        // Получение смен по занятости { свободен / занят в период или конкретный час }
        return await _shiftService.FindBusyByWeekAndDay(getShiftDto.Hours, getShiftDto.DayOfWeek);
    }
}