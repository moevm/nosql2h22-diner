using DomainLib.Models;
using Microsoft.AspNetCore.Mvc;
using ServicesLib.ModelServices;

namespace Diner.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TestController: Controller
{
    private readonly UserService _userService;

    public TestController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create_user")]
    public async Task CreateUser(User userModel)
    {
        await _userService.CreateAsync(userModel);
    }
    
    [HttpGet("get_users")]
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userService.FindAllAsync();
    }
}