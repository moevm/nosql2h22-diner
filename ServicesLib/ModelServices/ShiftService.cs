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
    
    public async Task<List<Shift>> FindByWeekAndDay(int hours, DayOfWeek? dayOfWeek)
    { 
        var weekFilter = Builders<Week>.Filter.Where(x =>
                 ((x.Monday & hours) != 0 || (x.Tuesday & hours) != 0 || (x.Wednesday & hours) != 0 ||
                  (x.Thursday & hours) != 0 || (x.Friday & hours) != 0 || (x.Saturday & hours) != 0 ||
                  (x.Sunday & hours) != 0));
        if (dayOfWeek != null)
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    weekFilter = Builders<Week>.Filter.Where(x => (x.Monday & hours) != 0);
                    break;
                case DayOfWeek.Tuesday:
                    weekFilter = Builders<Week>.Filter.Where(x => (x.Tuesday & hours) != 0);
                    break;
                case DayOfWeek.Wednesday:
                    weekFilter = Builders<Week>.Filter.Where(x => (x.Wednesday & hours) != 0);
                    break;
                case DayOfWeek.Thursday:
                    weekFilter = Builders<Week>.Filter.Where(x => (x.Thursday & hours) != 0);
                    break;
                case DayOfWeek.Friday:
                    weekFilter = Builders<Week>.Filter.Where(x => (x.Friday & hours) != 0);
                    break;
                case DayOfWeek.Saturday:
                    weekFilter = Builders<Week>.Filter.Where(x => (x.Saturday & hours) != 0);
                    break;
                case DayOfWeek.Sunday:
                    weekFilter = Builders<Week>.Filter.Where(x => (x.Sunday & hours) != 0);
                    break;
            }
        var weeks = await this._weekService.WhereManyAsync(weekFilter);
        var weeksIds = weeks.Select(x => x.ShiftId);
        return await this.WhereManyAsync(Builders<Shift>.Filter.Where(x => weeksIds.Contains(x.Id)));
    }
}