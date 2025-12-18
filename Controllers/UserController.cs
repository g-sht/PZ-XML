using Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace user;

[Route("[controller]")]
[ApiController]
public class UserController(AuthenticationService _authService) : ControllerBase
{
    [HttpPost("info")]
    public async Task<IActionResult> GetUserInfo([FromBody] JwtModel token)
    {
        if (string.IsNullOrWhiteSpace(token?.accessToken))
            return BadRequest("There's no JWT");

        var userInfo = await _authService.GetUserInfoByToken(token.accessToken);
        
        if (userInfo == null)
            return Unauthorized("Invalid or expired JWT");

        return Ok(userInfo);
    }
}