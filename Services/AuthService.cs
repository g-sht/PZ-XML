using Auth.Persistance;
using Microsoft.AspNetCore.Identity;

namespace Auth.Services;

public class AuthenticationService(AuthenticationRepo _authRepo, JwtService _jwtService)
{
    public async Task<Organization?> RegisterOrg(string orgName, ulong inn, ulong ogrn, Guid uuid)
    {
        var isOrgExists = await _authRepo.GetOrgByINN(inn) != null;
        if (isOrgExists)
            return null;

        var orgData = new Organization
        {
            Id = uuid,
            OrgName = orgName,
            Inn = inn,
            Ogrn = ogrn,
        };

        var res = await _authRepo.AddOrganization(orgData);
        return res;
    }

    public async Task<ContactPerson?> RegisterPerson(string firstName, string lastName, string email, 
        string phone, string password, Guid orgId)
    {
        var isPersonExists = await _authRepo.GetByEmail(email) != null 
            || await _authRepo.GetByPhone(phone) != null;

        if (isPersonExists) 
            return null;

        var personData = new ContactPerson
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            OrgId = orgId,
        };

        personData.PasswordHash = new PasswordHasher<ContactPerson>().HashPassword(personData, password);

        var res = await _authRepo.AddPerson(personData);
        return res;
    }

    public async Task<string> Login(ContactPerson person, string password)
    {
        var verificationRes = new PasswordHasher<ContactPerson>().VerifyHashedPassword((ContactPerson)person, (string)person.PasswordHash, password);
        if (verificationRes == PasswordVerificationResult.Success)
        {
            var org = await _authRepo.GetOrgByPerson(person);
            if (org == null) 
                return string.Empty;
            return _jwtService.GenerateToken(org, person);
        }
            
        return string.Empty;
    }

    internal async Task<string> LoginWithPhone(string phone, string password)
    {
        var person = await _authRepo.GetByPhone(phone);
        if (person == null)
            return string.Empty;

        var jwt = await Login(person, password);
        return jwt;
    }

    internal async Task<string> LoginWithEmail(string email, string password)
    {
        var person = await _authRepo.GetByEmail(email);
        if (person == null)
            return string.Empty;

        var jwt = await Login(person, password);
        return jwt;
    }

    public async Task<UserInfo?> GetUserInfoByToken(string accessToken)
    {
        var claims = _jwtService.ValidateTokenAndExtractClaims(accessToken);
        if (claims == null)
            return null;

        var userInfo = await _authRepo.GetInfoByID(claims.Value.orgId, claims.Value.personId);
        return userInfo;
    }
}

