using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.ModelServices;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class DishController: Controller
{
    private readonly DishService _dishService;
    
    public DishController(DishService dishService)
    {
        _dishService = dishService;
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
    
    [HttpPost]
    [Route("get-dish", Name = "getDish")]
    [ProducesResponseType(typeof(Dish), 200)]
    public async Task<IActionResult>? GetDish(string id)
    {
        var dish = await _dishService.FindOneAsync(id);
        return dish != null ? Ok(dish) : Json(null);
    }
    
    [HttpGet]
    [Route("get-dishes", Name = "getDishes")]
    public async Task<List<Dish>> GetDishes()
    {
        return await _dishService.FindAllAsync();
    }
}