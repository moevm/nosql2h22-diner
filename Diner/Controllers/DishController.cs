using System.Text.RegularExpressions;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ServicesLib.ModelServices;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class DishController: Controller
{
    private readonly DishService _dishService;
    private readonly ResourceService _resourceService;
    private readonly DishResourceService _dishResourceService;

    
    public DishController(DishService dishService, ResourceService resourceService, DishResourceService dishResourceService)
    {
        _dishService = dishService;
        _resourceService = resourceService;
        _dishResourceService = dishResourceService;
    }

    [HttpPost]
    [Route("create-dish", Name = "createDish")]
    public async Task<IActionResult> CreateDish(DishDto dishDto)
    {
        try
        {
            return Ok(await _dishService.CreateDish(dishDto));
        }
        catch (Exception e)
        {
            if (e.Message.EndsWith("already exists"))
            {
                return Problem(detail: e.Message, statusCode: 409);
            }
            return NotFound(e.Message);
        }
        
    }
    
    [HttpPost]
    [Route("update-dish", Name = "updateDish")]
    public async Task<IActionResult> UpdateDish(DishDto dishDto)
    {
        try
        {
            return Ok(await _dishService.UpdateDish(dishDto));
        }
        catch (Exception e)
        {
            if (e.Message.EndsWith("is null"))
            {
                return ValidationProblem(e.Message);
            }
            return NotFound(e.Message);
        }
    }
    
    [HttpGet]
    [Route("get-dish", Name = "getDish")]
    [ProducesResponseType(typeof(Dish), 200)]
    public async Task<IActionResult>? GetDish(string? id)
    {
        if (!ObjectId.TryParse(id, out var x)) return StatusCode(400, "{  \"status\": 400, \"payload\": \" Wrong Id! \" }");
        var dish = await _dishService.FindOneAsync(id);
        return dish != null ? Ok(dish) : Json(null);
    }
    
    [HttpGet]
    [Route("get-dish-resources", Name = "getDishResources")]
    public async Task<List<Resource>>? GetDishResources(string? id)
    {
        if (!ObjectId.TryParse(id, out var x)) return new List<Resource>();
        var dishResource =
            (await _dishResourceService.WhereManyAsync(Builders<DishResource>.Filter.Where(x => x.DishId == id)))
            .Select(x => new { x.ResourceId, x.Required });
        var resourceIds = dishResource.Select(x => x.ResourceId);
        var resources =
            (await _resourceService.WhereManyAsync(
                Builders<Resource>
                    .Filter
                    .Where(x => resourceIds.Contains(x.Id))));
        foreach (var resource in resources)
        {
            resource.Amount = dishResource!.First(x => x.ResourceId == resource.Id).Required;
        }
        return resources;
    }
    
    [HttpGet]
    [Route("update-dish-resources", Name = "updateDishResources")]
    public async Task<List<Resource>>? UpdateDishResources()
    {
        return new List<Resource>();
    }
    
    [HttpGet]
    [Route("get-dishes", Name = "getDishes")]
    public async Task<List<Dish>> GetDishes(string? name)
    {
        if (string.IsNullOrEmpty(name)) return await _dishService.FindAllAsync();
        var filter = Builders<Dish>.Filter.Regex("Name", $"/{name}/i");
        return await _dishService.WhereManyAsync(filter);
    }
}