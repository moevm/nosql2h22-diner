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

    #region PublicMethods

    public async Task<Dish> CreateDish(DishDto dto) {
        var filter = Builders<Dish>.Filter.Where(x => x.Name == dto.Name);
        if (await WhereOneAsync(filter) is not null) throw new Exception("Dish is already exists");

        var dish = new Dish() {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
        };

        await CreateAsync(dish);
        
        foreach (var dishResourceDto in dto.ListDishResourceDtos) {
            var resourceFilter = Builders<Resource>.Filter.Where(x => x.Id == dishResourceDto.ResourceId);
            if (await _resourceService.WhereOneAsync(resourceFilter) is null) 
                throw new Exception("Resource not found");
            
            var dishResource = new DishResource() {
                Required = dishResourceDto.Required,
                ResourceId = dishResourceDto.ResourceId,
                DishId = dish.Id
            };
            await _dishResourceService.CreateAsync(dishResource);
        }
        
        return dish;
    }
    
    public async Task<Dish> UpdateDish(DishDto dto) {
        if (dto.Id is null) throw new Exception("DishId is null");
        var filter = Builders<Dish>.Filter.Where(x => x.Id == dto.Id);
        var dish = await WhereOneAsync(filter) ?? throw new Exception("Dish not found");

        dish.Description = dto.Description;
        dish.Name = dto.Name;
        dish.Price = dto.Price;

        foreach (var dishResourceDto in dto.ListDishResourceDtos)
        {
            if (dishResourceDto.Id is null) throw new Exception("DishResourceId is null");
            var dishResourceFilter = Builders<DishResource>.Filter.Where(x => x.Id == dishResourceDto.Id);
            var dishResource = await _dishResourceService.WhereOneAsync(dishResourceFilter);
            var resourceFilter = Builders<Resource>.Filter.Where(x => x.Id == dishResourceDto.ResourceId);
            if (await _resourceService.WhereOneAsync(resourceFilter) is null) throw new Exception("Resource not found");

            dishResource ??= new DishResource()
            {
                DishId = dish.Id,
                ResourceId = dishResourceDto.ResourceId
            };

            dishResource.Required = dishResourceDto.Required;

            await _dishResourceService.UpdateAsync(dishResource.Id,dishResource);
        }

        await UpdateAsync(dish.Id,dish);
        
        return dish;
    }

    #endregion
}