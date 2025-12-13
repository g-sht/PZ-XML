using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using YamlDotNet.Core.Tokens;

namespace Auth;

[Route("[controller]")]
[ApiController]
public class AuthController(AuthenticationService _authService, JwtService _jwtService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var orgId = Guid.NewGuid();

        var org = await _authService.RegisterOrg(model.OrgName, model.Inn, model.Ogrn, orgId);
        var person = await _authService.RegisterPerson(model.FirstName, model.LastName, model.Email, model.Phone, model.Password, orgId);
        
        if (org == null || person == null) {
            return BadRequest();
        }

        var jwt = _jwtService.GenerateToken(org, person);

        return Ok(new
        {
            access_token = jwt,
            token_type = "Bearer",
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var jwt = string.Empty;

        if (!model.IsEmailSet() && model.Phone == string.Empty)
            return BadRequest();
        else if (model.IsEmailSet())
            jwt = await _authService.LoginWithEmail(model.Email, model.Password);
        else 
            jwt = await _authService.LoginWithPhone(model.Phone, model.Password);

        return jwt == string.Empty ? BadRequest() : Ok(new { 
                                                        access_token = jwt, 
                                                        token_type = "Bearer", 
                                                    });
    }
}
