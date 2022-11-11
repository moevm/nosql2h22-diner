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
        if (await WhereOneAsync(filter) is null) throw new Exception("Resource is already exists");

        var resource = new Resource() {
            Name = dto.Name,
            Amount = dto.Amount,
            Unit = dto.Unit,
            // TODO сделать автоинкремент
            NumericId = 0
        };
            
        await CreateAsync(resource);
        return resource;
    }
}