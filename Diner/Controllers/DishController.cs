using System.Security.Claims;
using System.Text.RegularExpressions;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServicesLib.ModelServices;
using ServicesLib.Services;
using ServicesLib.Services.Outputs;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class DishController: Controller
{
    private readonly DishService _dishService;
    private readonly ResourceService _resourceService;
    private readonly DishResourceService _dishResourceService;
    private readonly ExcelService _excelService;

    private static readonly List<UserRole> UserRoles = new List<UserRole>() { UserRole.Cook, UserRole.Admin, UserRole.Manager };
    public DishController(DishService dishService, ResourceService resourceService, DishResourceService dishResourceService, ExcelService excelService)
    {
        _dishService = dishService;
        _resourceService = resourceService;
        _dishResourceService = dishResourceService;
        _excelService = excelService;
    }

    [HttpPost]
    [Route("create-dish", Name = "createDish")]
    public async Task<IActionResult> CreateDish(DishDto dishDto)
    {
        if (!Enum.TryParse<UserRole>(HttpContext.User.FindFirstValue(ClaimTypes.Role), out var role) ||
            !UserRoles.Contains(role)) return Unauthorized();
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
        if (!Enum.TryParse<UserRole>(HttpContext.User.FindFirstValue(ClaimTypes.Role), out var role) ||
            !UserRoles.Contains(role)) return Unauthorized();
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
        if (!Enum.TryParse<UserRole>(HttpContext.User.FindFirstValue(ClaimTypes.Role), out var role) ||
            !UserRoles.Contains(role)) return null;
        return new List<Resource>();
    }
    
    [HttpGet]
    [Route("get-dishes", Name = "getDishes")]
    public async Task<List<Dish>> GetDishes(string? name, DishType? dishType)
    {
        if (string.IsNullOrEmpty(name) && dishType == null) return await _dishService.FindAllAsync();
        var filter = Builders<Dish>.Filter.Regex("Name", $"/{name}/i");
        var dishFilter = dishType == null ? Builders<Dish>.Filter.Where(x => true) : Builders<Dish>.Filter.Where(x => x.DishType == dishType);
        return await _dishService.WhereManyAsync(Builders<Dish>.Filter.Where(x => filter.Inject() && dishFilter.Inject()));
    }
    
    [HttpPost]
    [Route("get-dishes-excel", Name = "getDishesExcel")]
    public async Task<IActionResult> GetDishesExcel()
    {
        var dish = await _dishService.FindAllAsync();
        var sheet = ExcelBuilder.FormatDataToExcel(dish.Select(x => new ExcelDishesOutput(x)));
        sheet.SheetName = "Dishes report";
        HttpContext.Response.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        return Ok(_excelService.CreateNewExcelBuilder().AddSheet(sheet).AsStream());
    }
}