using Auth.Persistance;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Services;

public class JwtService(IOptions<AuthSettings> _settings)
{
    public string GenerateToken(Organization org, ContactPerson person)
    {
        var claims = new List<Claim>()
        {
            new Claim("orgName", org.OrgName),
            new Claim("firstName", person.FirstName),
            new Claim("lastName", person.LastName),
            new Claim("personId", person.Id.ToString()),
            new Claim("orgId", org.Id.ToString()),
        };

        var jwt = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(_settings.Value.Expires),
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_settings.Value.SecretKey)), 
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public (string personId, string orgId)? ValidateTokenAndExtractClaims(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.Value.SecretKey);
            
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var personId = jwtToken.Claims.FirstOrDefault(c => c.Type == "personId")?.Value;
            var orgId = jwtToken.Claims.FirstOrDefault(c => c.Type == "orgId")?.Value;

            if (personId != null && orgId != null)
                return (personId, orgId);

            return null;
        }
        catch
        {
            return null;
        }
    }
}

