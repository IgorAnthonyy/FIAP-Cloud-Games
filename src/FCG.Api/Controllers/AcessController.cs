using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FCG.Api.Controllers;

[Route("api/[controller]")]
public class AcessController : BaseController
{
    private readonly IAcessService _acessService;
    
    public AcessController(IAcessService acessService)
    {
        _acessService = acessService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AcessLogin login)
    {
        var token = await _acessService.Login(login);
        return CreatedResult(new {Token = token});
    }

}
