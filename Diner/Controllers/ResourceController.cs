using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    
    [HttpPost]
    [Route("get-resource", Name = "getResource")]
    [ProducesResponseType(typeof(Resource), 200)]
    public async Task<IActionResult>? GetResource(string id)
    {
        var resource = await _resourceService.FindOneAsync(id);
        return resource != null ? Ok(resource) : Json(null);
    }
    
    [HttpGet]
    [Route("get-resources", Name = "getResources")]
    public async Task<List<Resource>> GetResources()
    {
        return await _resourceService.FindAllAsync();
    }
}