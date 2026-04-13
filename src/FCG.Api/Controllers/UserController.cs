using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FCG.Api.Controllers;

[Route("api/[controller]")]
public class UserController : BaseController
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreate user)
    {
        var userCreated = await _userService.CreateUser(user);
        return CreatedResult(userCreated);
    }

    [HttpPost("admin")]
    public async Task<IActionResult> CreateAdmin([FromBody] AdminCreate user)
    {
        var userCreated = await _userService.CreateAdmin(user);
        return CreatedResult(userCreated);
    }
}
