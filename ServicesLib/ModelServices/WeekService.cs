using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class WeekService: BaseModelService<Week>
{
    public WeekService(IOptions<DbConfig> dbConfig) : base(dbConfig) {}

    public async Task<Week> CreateDefaultWeek()
    {
        var workDay = "11111111110011111111111111110";
        var freeDay = "00000000000000000000000000000";
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
        return newWeek;
    }
}