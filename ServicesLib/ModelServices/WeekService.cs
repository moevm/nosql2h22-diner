using DomainLib.DTO;
using DomainLib.Models;
using Exceptionless.DateTimeExtensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class WeekService: BaseModelService<Week>
{
    public WeekService(IOptions<DbConfig> dbConfig) : base(dbConfig) {}

    public int HourMap(DateTime x)
    {
        if (x.Hour < 9 || x.Hour > 22) return 0;
        var map = new List<int>()
        {
            9, 39, 10, 40, 11, 41, 12, 42, 13, 43, 14, 44, 15, 45, 16, 46, 17, 47, 18, 48, 19, 49, 20, 50, 21, 51, 22
        };
        var d = x.Floor(new TimeSpan(0, 30, 0));
        return map.IndexOf(d.Hour + d.Minute); 
    }

    public readonly int MaxWeekValue = 0b11111111111111111111111111111;
    public int? BinaryFromDate(DateTime from, DateTime? to = null)
    {
        if (to < from || from > to) return null;
        var hoursFrom = HourMap(from);
        var hoursTo = HourMap(to ?? DateTime.MinValue);
        int hours = 0;
        for(var i = hoursFrom; i < hoursTo; ++i) hours |= 1 << i;
        hours |= 1 << hoursFrom;
        return hours;
    }

    public async Task<Week> CreateDefaultWeek()
    {
        var workDay = 0b001111111100111111111111111;
        var freeDay = 0b000000000000000000000000000;
        var newWeek = new Week
        {
            Sunday = freeDay,
            Monday = workDay,
            Tuesday = workDay,
            Wednesday = workDay,
            Thursday = workDay,
            Friday = workDay,
            Saturday = freeDay,
        };
        await this.CreateAsync(newWeek);
        newWeek.CreatedAt = newWeek.CreatedAt.ToUniversalTime().StartOfWeek();
        await this.UpdateAsync(newWeek.Id, newWeek);
        return newWeek;
    }

    public async Task<Week> UpdateWeek()
    {
        return new Week();
    }

    public FilterDefinition<Week> FindWeeksByHours(int hours, DateTime? dateTime, bool free = false)
    {
        var dateFilter = Builders<Week>.Filter.Where(x => true);
        if (dateTime != null)
        {
            var day = dateTime.Value.ToUniversalTime().StartOfWeek();
            dateFilter = Builders<Week>.Filter.Where(x => x.CreatedAt == day);
        }
        var isFree = (int day) => free ? ~day : day;
        var weekFilter =  Builders<Week>.Filter.Where(x => true);
        if (dateTime != null)
        {
            var dayOfWeek = dateTime.Value.DayOfWeek;
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
        }   
        return  Builders<Week>.Filter.Where(x =>
            ((x.Monday & hours) != 0 || (x.Tuesday & hours) != 0 || (x.Wednesday & hours) != 0 ||
             (x.Thursday & hours) != 0 || (x.Friday & hours) != 0 || (x.Saturday & hours) != 0 ||
             (x.Sunday & hours) != 0) && dateFilter.Inject() && weekFilter.Inject());;
        }
}