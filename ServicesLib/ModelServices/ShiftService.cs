using DomainLib.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class ShiftService: BaseModelService<Shift>
{
    private readonly WeekService _weekService;

    public ShiftService(IOptions<DbConfig> dbConfig, WeekService weekService) : base(dbConfig)
    {
        this._weekService = weekService;
    }

    public async Task<Shift> CreateDefaultShift(string userId)
    {
        var week = await this._weekService.CreateDefaultWeek();
        var newShift = new Shift
        {
            UserId = userId,
            TargetWeekId = week.Id,
        };
        await this.CreateAsync(newShift);
        week.ShiftId = newShift.Id;
        await this._weekService.UpdateAsync(week.Id, week);
        return newShift;
    }
    
    public async Task<List<Shift>> FindBusyByWeekAndDay(int hours, DayOfWeek? dayOfWeek, bool free = false)
    {
        var weeks = await this._weekService.FindWeeksByHours(hours, dayOfWeek, free);
        var weeksIds = weeks.Select(x => x.ShiftId);
        return await this.WhereManyAsync(Builders<Shift>.Filter.Where(x => weeksIds.Contains(x.Id)));
    }
}