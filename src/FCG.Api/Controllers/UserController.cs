using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Domain.Contants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Security.Claims;
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
    
    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UserUpdate request)
    {
        request.Id = id;
    
        var result = await _userService.UpdateUser(request);
        return Ok(result);
    }

    [HttpDelete("{idUserToDeleted:guid}")]
    [Authorize(Policy = FCGConstant.AdminRole)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid idUserToDeleted)
    {

        var userDeleted = await _userService.DeleteUser(idUserToDeleted);
        if (!userDeleted) return Unauthorized(new { Message = "Não foi possível deletar usuárrio, por que você não está autorizado" });
        return Ok(new {Message = "Usuário deletado com sucesso"});
    }
}
