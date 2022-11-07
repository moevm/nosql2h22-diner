using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class WeekService: BaseModelService<Week>
{
    public WeekService(IOptions<DbConfig> dbConfig) : base(dbConfig) {}
}