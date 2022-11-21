using System.Text;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OfficeOpenXml;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class ResourceService: BaseModelService<Resource>
{
    public ResourceService(IOptions<DbConfig> dbConfig) : base(dbConfig) {}

    public async Task<Resource> CreateResource(ResourceDto dto) {
        var filter = Builders<Resource>.Filter.Where(x => x.Name == dto.Name);
        if (await WhereOneAsync(filter) is not null) throw new Exception("Resource is already exists");

        var resource = new Resource() {
            Name = dto.Name,
            Amount = dto.Amount,
            Unit = dto.Unit,
        };
            
        await CreateAsync(resource);
        return resource;
    }
    
    public async Task<Resource> UpdateResource(ResourceDto dto) {
        if (dto.Id is null) throw new Exception("ResourceId is null");
        var filter = Builders<Resource>.Filter.Where(x => x.Id == dto.Id);
        var resource = await WhereOneAsync(filter) ?? throw new Exception("Resource not found");

        resource.Name = dto.Name;
        resource.Amount = dto.Amount;
        resource.Unit = dto.Unit;
            
        await UpdateAsync(resource.Id,resource);
        return resource;
    }

    public async Task ImportResourcesFromExcel(Stream file) {
        using (ExcelPackage xlPackage = new ExcelPackage(file))
        {
            var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
            var totalRows = myWorksheet.Dimension.End.Row;
            var totalColumns = myWorksheet.Dimension.End.Column;

            for (int rowNum = 1; rowNum <= totalRows; rowNum++) //select starting row here
            {
                var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns]
                    .Select(c => c.Value == null ? string.Empty : c.Value.ToString())
                    .ToList();
                var resourceDto = new ResourceDto() {
                    Name = row[0] ?? string.Empty,
                    Amount = row[1] is not null ? int.Parse(row[1]!) : 0,
                    Unit = row[1] is not null ? Enum.Parse<Unit>(row[2]!) : Unit.Kg,
                };

                await CreateResource(resourceDto);
            }
        }
    }
}