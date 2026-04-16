using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FCG.Api.Controllers;

[Route("api/[controller]")]
public class AccessController : BaseController
{
    private readonly IAccessService _accessService;
    
    public AccessController(IAccessService accessService)
    {
        _accessService = accessService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccessLogin login)
    {
        var token = await _accessService.Login(login);
        return CreatedResult(new {Token = token});
    }

}
