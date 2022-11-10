using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class ShiftService: BaseModelService<Shift>
{
    public ShiftService(IOptions<DbConfig> dbConfig) : base(dbConfig)
    { }

    public async Task<Shift> CreateDefaultShift(string userId, string weekId)
    {
        var newShift = new Shift
        {
            UserId = userId,
            TargetWeekId = weekId,
        };
        await this.CreateAsync(newShift);
        return newShift;
    }
}