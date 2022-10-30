using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class DishResourceService: BaseModelService<DishResource, DishRecourceDbConfig>
{
    public DishResourceService(IOptions<DishRecourceDbConfig> dbConfig) : base(dbConfig)
    {
    }
}