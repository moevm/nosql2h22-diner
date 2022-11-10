using DomainLib.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UtilsLib.Configurations;

namespace ServicesLib;

public class BaseModelService<TModel> 
    where TModel: BaseModel
{
    protected readonly IMongoCollection<TModel> _modelCollection;
    private static Dictionary<string, string>? _collectionsNames { get; set; }
    
    public BaseModelService(IOptions<DbConfig> dbConfig)
    {
        var mongoClient = new MongoClient(dbConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbConfig.Value.DatabaseName);

        _collectionsNames ??= new Dictionary<string, string>
        {
            { typeof(User).FullName!, dbConfig.Value.UsersCollectionName },
            { typeof(Resource).FullName!, dbConfig.Value.ResourcesCollectionName },
            { typeof(Shift).FullName!, dbConfig.Value.ShiftsCollectionName },
            { typeof(Payment).FullName!, dbConfig.Value.PaymentsCollectionName },
            { typeof(Dish).FullName!, dbConfig.Value.DishesCollectionName },
            { typeof(DishResource).FullName!, dbConfig.Value.DishResourcesCollectionName },
            { typeof(Comment).FullName!, dbConfig.Value.CommentsCollectionName },
            { typeof(Week).FullName!, dbConfig.Value.WeeksCollectionName},
            { typeof(AuthInfo).FullName!, dbConfig.Value.AuthInfoCollectionName},
        };
        
        _modelCollection = mongoDatabase.GetCollection<TModel>(_collectionsNames[typeof(TModel).FullName!]);
    }
    
    public async Task<List<TModel>> FindAllAsync() =>
        await _modelCollection.Find(_ => true).ToListAsync();

    public async Task<TModel?> FindOneAsync(string id) =>
        await _modelCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<TModel?> WhereAsync(FilterDefinition<TModel> definition) =>
        await _modelCollection.Find(definition).FirstOrDefaultAsync();
    public async Task CreateAsync(TModel model)
    {
        model.CreatedAt = DateTime.Now;
        model.UpdatedAt = DateTime.Now;
        await _modelCollection.InsertOneAsync(model);
    }

    public async Task UpdateAsync(string id, TModel updatedModel)
    {
        updatedModel.UpdatedAt = DateTime.Now;
        await _modelCollection.ReplaceOneAsync(x => x.Id == id, updatedModel);
    }
    
    public async Task RemoveAsync(string id) =>
        await _modelCollection.DeleteOneAsync(x => x.Id == id);
    
}