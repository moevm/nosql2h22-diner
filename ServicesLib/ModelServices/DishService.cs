using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UtilsLib.Configurations;


namespace ServicesLib.ModelServices;

public class DishService: BaseModelService<Dish> {
    private readonly ResourceService _resourceService;
    private readonly DishResourceService _dishResourceService;
    public DishService(
        IOptions<DbConfig> dbConfig,
        ResourceService resourceService,
        DishResourceService dishResourceService
    ) : base(dbConfig) {
        _resourceService = resourceService;
        _dishResourceService = dishResourceService;
    }

    #region Plain

    public async Task<Dish> CreateDish(DishDto dto) {
        var filter = Builders<Dish>.Filter.Where(x => x.Name == dto.Name);
        if (await WhereOneAsync(filter) is null) throw new Exception("Dish is already exists");

        var dish = new Dish() {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            // TODO сделать автоинкремент
            NumericId = 0
        };
        
        await CreateAsync(dish);
        
        foreach (var dishResourceDto in dto.ListDishResourceDtos) {
            var resourceFilter = Builders<Resource>.Filter.Where(x => x.Id == dishResourceDto.Id);
            if (await _resourceService.WhereOneAsync(resourceFilter) is null) 
                throw new Exception("Resource is already exists");
            
            var dishResource = new DishResource() {
                Required = dishResourceDto.Required,
                ResourceId = dishResourceDto.Id,
                DishId = dish.Id
            };
            await _dishResourceService.CreateAsync(dishResource);
        }
        
        return dish;
    }

    #endregion
}