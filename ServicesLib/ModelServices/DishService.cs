using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class DishService: BaseModelService<Dish, DishDbConfig>
{
    public DishService(IOptions<DishDbConfig> dbConfig) : base(dbConfig)
    { }
}