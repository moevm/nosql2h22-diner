using System.Security.Claims;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServicesLib.ModelServices;
using ServicesLib.Services;
using ServicesLib.Services.Outputs;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
//[Authorize]
public class ResourceController: Controller
{
    private readonly ResourceService _resourceService;
    private readonly ExcelService _excelService;
    private static readonly List<UserRole> UserRoles = new List<UserRole>() { UserRole.Steward, UserRole.Admin, UserRole.Manager };
    
    public ResourceController(ResourceService resourceService, ExcelService excelService)
    {
        _resourceService = resourceService;
        _excelService = excelService;
    }

    [HttpPost]
    [Route("create-resource", Name = "createResource")]
    [ProducesResponseType(typeof(Resource), 200)]
    public async Task<IActionResult> CreateDish(ResourceDto resourceDto)
    {
        if (!Enum.TryParse<UserRole>(HttpContext.User.FindFirstValue(ClaimTypes.Role), out var role) ||
            !UserRoles.Contains(role)) return Unauthorized();
        try
        {
            return Ok(await _resourceService.CreateResource(resourceDto));
        }
        catch (Exception e)
        {
            return StatusCode(409, "{  \"status\": 409, \"payload\": \" Resource is already exists\" }");;;
        }
    }
    
    [HttpPost]
    [Route("update-resource", Name = "updateResource")]
    [ProducesResponseType(typeof(Resource), 200)]
    public async Task<IActionResult> UpdateResource(ResourceDto resourceDto)
    {
        if (!Enum.TryParse<UserRole>(HttpContext.User.FindFirstValue(ClaimTypes.Role), out var role) ||
            !UserRoles.Contains(role)) return Unauthorized();
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
            return StatusCode(400, "{  \"status\": 400, \"payload\": \" Resource not found \" }");;;
        }
    }
    
    [HttpGet]
    [Route("get-resource", Name = "getResource")]
    [ProducesResponseType(typeof(Resource), 200)]
    public async Task<IActionResult> GetResource(string id)
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
    
    [HttpPost]
    [Route("get-resources-excel", Name = "getResourcesExcel")]
    public async Task<IActionResult> GetResourcesExcel()
    {
        var resources = await _resourceService.FindAllAsync();
        var sheet = ExcelBuilder.FormatDataToExcel(resources.Select(x => new ExcelResourcesOutput(x)));
        sheet.SheetName = "Resource report";
        HttpContext.Response.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        return Ok(_excelService.CreateNewExcelBuilder().AddSheet(sheet).AsStream());
    }
    
    [HttpPost]
    [Route("import-resources", Name = "importResources")]
    public async Task<IActionResult> ImportResourcesFromExcel() {
        if (!Enum.TryParse<UserRole>(HttpContext.User.FindFirstValue(ClaimTypes.Role), out var role) ||
            !UserRoles.Contains(role)) return Unauthorized();
        try {
            var form = await Request.ReadFormAsync();
            var stream = form.Files.First().OpenReadStream();
            stream.Position = 0;
            await _resourceService.ImportResourcesFromExcel(stream);
            return Ok(true);
        }
        catch (Exception e) {
            return StatusCode(400, "{  \"status\": 400, \"payload\": \" " + e.Message + " \" }");
        }
    }
}