using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FCG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] UserDTO user)
        {
            try
            {
                var userCreated = await _userService.CriarUsuario(user);
                return StatusCode(StatusCodes.Status201Created, userCreated);
            }
            catch (System.Exception e)
            {
                return BadRequest(e);
                
            }
        }
    }
}
