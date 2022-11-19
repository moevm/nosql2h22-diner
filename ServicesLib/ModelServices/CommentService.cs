using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class CommentService: BaseModelService<Comment>
{
    private readonly ResourceService _resourceService;
    private readonly DishService _dishService;

    public CommentService(IOptions<DbConfig> dbConfig, ResourceService resourceService, DishService dishService) : base(dbConfig)
    {
        _dishService = dishService;
        _resourceService = resourceService;
    }
    
    public async Task<Comment> CreateComment(СommentDto dto) {

        var comment = new Comment() {
            Content = dto.Content,
            DishId = dto.DishId,
            ResourceId = dto.ResourceId
        };
        
        await CreateAsync(comment);
        
        if (dto.DishId is not null)
        {
            var dishFilter = Builders<Dish>.Filter.Where(x => x.Id == dto.Id);
            var dish = await _dishService.WhereOneAsync(dishFilter) ?? throw new Exception("Dish not found");
            dish.Comments.Add(comment.Id); 
        } else if (dto.ResourceId is not null)
        {
            var resourceFilter = Builders<Resource>.Filter.Where(x => x.Id == dto.Id);
            var resource = await _resourceService.WhereOneAsync(resourceFilter) ?? throw new Exception("Resource not found");
            resource.Comments.Add(comment.Id); 
        }
        
        return comment;
    }
    
    public async Task<Comment> UpdateComment(СommentDto dto) {

        if (dto.Id is null) throw new Exception("Id is null");
        var filter = Builders<Comment>.Filter.Where(x => x.Id == dto.Id);
        var comment = await WhereOneAsync(filter) ?? throw new Exception("Comment not found");

        comment.Content = dto.Content;

        await UpdateAsync(comment.Id,comment);
        return comment;
    }
}