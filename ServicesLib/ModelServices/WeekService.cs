using DomainLib.Models;
using Exceptionless.DateTimeExtensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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
        var workDay = 0b00111111110011111111111111111;
        var freeDay = 0b00000000000000000000000000000;
        var newWeek = new Week
        {
            Monday = workDay,
            Tuesday = workDay,
            Wednesday = workDay,
            Thursday = workDay,
            Friday = workDay,
            Saturday = freeDay,
            Sunday = freeDay,
        };
        await this.CreateAsync(newWeek);
        newWeek.CreatedAt = newWeek.CreatedAt.StartOfWeek();
        await this.UpdateAsync(newWeek.Id, newWeek);
        return newWeek;
    }
}