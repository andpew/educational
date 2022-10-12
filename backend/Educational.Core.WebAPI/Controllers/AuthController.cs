using Educational.Core.BLL.Services.Interfaces;
using Educational.Core.Common.DTO.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Educational.Core.WebAPI.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO userDTO)
    {
        await _authService.Register(userDTO);
        return Ok("Check your mailbox");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authorize([FromBody] UserLoginDTO userDTO)
    {
        return Ok(await _authService.Authorize(userDTO));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] AuthTokenDTO tokenDTO)
    {
        return Ok(await _authService.Refresh(tokenDTO));
    }
}
