using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServicesLib.ModelServices;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class ResourceController: Controller
{
    private readonly ResourceService _resourceService;
    
    public ResourceController(ResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [HttpPost]
    [Route("create-resource", Name = "createResource")]
    [ProducesResponseType(typeof(Resource), 200)]
    public async Task<IActionResult> CreateDish(ResourceDto resourceDto)
    {
        try
        {
            return Ok(await _resourceService.CreateResource(resourceDto));
        }
        catch (Exception e)
        {
            return Problem(detail: e.Message, statusCode: 409);
        }
    }
    
    [HttpPost]
    [Route("update-resource", Name = "updateResource")]
    [ProducesResponseType(typeof(Resource), 200)]
    public async Task<IActionResult> UpdateResource(ResourceDto resourceDto)
    {
        try
        {
            return Ok(await _resourceService.UpdateResource(resourceDto));
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
    [Route("get-resource", Name = "getResource")]
    [ProducesResponseType(typeof(Resource), 200)]
    public async Task<IActionResult>? GetResource(string id)
    {
        var resource = await _resourceService.FindOneAsync(id);
        return resource != null ? Ok(resource) : Json(null);
    }
    
    [HttpGet]
    [Route("get-resources", Name = "getResources")]
    public async Task<List<Resource>> GetResources(string? name)
    {
        if (string.IsNullOrEmpty(name)) return await this._resourceService.FindAllAsync();
        var filter = Builders<Resource>.Filter.Regex("Name", $"/{name}/i");
        return await _resourceService.WhereManyAsync(filter);
    }
}