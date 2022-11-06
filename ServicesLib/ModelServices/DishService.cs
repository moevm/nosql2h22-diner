using DomainLib.Models;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class DishService: BaseModelService<Dish>
{
    public DishService(IOptions<DbConfig> dbConfig) : base(dbConfig)
    { }
}