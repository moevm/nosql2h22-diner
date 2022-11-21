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
        try {
            var form = await Request.ReadFormAsync();
            var stream = form.Files.First().OpenReadStream();
            stream.Position = 0;
            await _resourceService.ImportResourcesFromExcel(stream);
            return Ok(true);
        }
        catch (Exception e) {
            return Problem(detail: e.Message, statusCode: 400);
        }
    }
}