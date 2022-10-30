using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class ShiftService: BaseModelService<Shift, ShiftDbConfig>
{
    public ShiftService(IOptions<ShiftDbConfig> dbConfig) : base(dbConfig)
    { }
}