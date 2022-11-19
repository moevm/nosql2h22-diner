using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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
}