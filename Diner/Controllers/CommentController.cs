using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ServicesLib.ModelServices;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class CommentController: Controller
{
    private readonly CommentService _commentService;
    private readonly UserService _userService;
    
    public CommentController(CommentService commentService, UserService userService)
    {
        _commentService = commentService;
        _userService = userService;
    }

    [HttpPost]
    [Route("create-comment", Name = "createComment")]
    public async Task<IActionResult> CreateComment(СommentDto dishDto)
    {
        try
        {
            var comment = await _commentService.CreateComment(dishDto);
            comment.User = await _userService.FindOneAsync(dishDto.UserId) ?? new User();
            return Ok(comment);
        }
        catch (Exception e)
        {
            if (e.Message.EndsWith("not found"))
            {
                return NotFound(e.Message);
            }
            return Problem(detail: e.Message, statusCode: 400);
        }
    }
    
    [HttpPost]
    [Route("update-comment", Name = "updateComment")]
    public async Task<IActionResult> UpdateComment(СommentDto dishDto)
    {
        try
        {
            return Ok(await _commentService.UpdateComment(dishDto));
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
    [Route("get-comment", Name = "getComment")]
    [ProducesResponseType(typeof(Comment), 200)]
    public async Task<IActionResult>? GetComment(string id)
    {
        var comment = await _commentService.FindOneAsync(id);
        return comment != null ? Ok(comment) : Json(null);
    }
    
    [HttpGet]
    [Route("get-comments", Name = "getComments")]
    public async Task<List<Comment>?> GetComments(string? dishId, string? resourceId)
    {
        var commentCollection = _commentService.GetCollection().AsQueryable();
        var userCollection = _userService.GetCollection().AsQueryable();
        var rawComments = from comment in commentCollection
            join user in userCollection on comment.UserId equals user.Id
            where (comment.ResourceId == (resourceId ?? ObjectId.Empty.ToString()) || comment.DishId == (dishId ?? ObjectId.Empty.ToString())) 
            select new Comment()
            {
                Id = comment.Id,
                User = new User()
                {
                    FullName = user.FullName,
                    Login = user.Login,
                },
                ResourceId = comment.ResourceId,
                DishId = comment.DishId,
                Content = comment.Content,
            };
        return rawComments.ToList();
    }
}