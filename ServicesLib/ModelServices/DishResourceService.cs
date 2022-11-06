using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class DishResourceService: BaseModelService<DishResource>
{
    public DishResourceService(IOptions<DbConfig> dbConfig) : base(dbConfig)
    {
    }
}