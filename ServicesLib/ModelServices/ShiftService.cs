using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class ShiftService: BaseModelService<Shift>
{
    public ShiftService(IOptions<DbConfig> dbConfig) : base(dbConfig)
    { }
}