using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.ModelServices;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class CommentController: Controller
{
    private readonly CommentService _commentService;
    
    public CommentController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    [Route("create-comment", Name = "createComment")]
    public async Task<IActionResult> CreateComment(СommentDto dishDto)
    {
        try
        {
            return Ok(await _commentService.CreateComment(dishDto));
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
    public async Task<List<Comment>> GetComments()
    {
        return await _commentService.FindAllAsync();
    }
}