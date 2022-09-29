using DomainLib.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UtilsLib.Configurations;

namespace ServicesLib;

public class BaseModelService<TModel, TConfig> 
    where TModel: BaseModel
    where TConfig: BaseDbConfig
{
    protected readonly IMongoCollection<TModel> _modelCollection;

    public BaseModelService(IOptions<TConfig> dbConfig)
    {
        var mongoClient = new MongoClient(dbConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbConfig.Value.DatabaseName);

        _modelCollection = mongoDatabase.GetCollection<TModel>(dbConfig.Value.CollectionName);
    }
    
    public async Task<List<TModel>> FindAllAsync() =>
        await _modelCollection.Find(_ => true).ToListAsync();

    public async Task<TModel?> FindOneAsync(Guid id) =>
        await _modelCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(TModel model)
    {
        model.CreatedAt = DateTime.Now;
        model.UpdatedAt = DateTime.Now;
        await _modelCollection.InsertOneAsync(model);
    }

    public async Task UpdateAsync(Guid id, TModel updatedModel)
    {
        updatedModel.UpdatedAt = DateTime.Now;
        await _modelCollection.ReplaceOneAsync(x => x.Id == id, updatedModel);
    }
    
    public async Task RemoveAsync(Guid id) =>
        await _modelCollection.DeleteOneAsync(x => x.Id == id);
    
}