using Microsoft.EntityFrameworkCore;

namespace Auth.Persistance;

public class AuthenticationRepo(ApplicationDbContext _context)
{
    public async Task<Organization?> GetOrgByINN(ulong inn)
    {
        var res = await _context.Organizations
            .Where(o => o.Inn == inn)
            .FirstOrDefaultAsync();
        return res;
    }

    public async Task<ContactPerson?> GetByPhone(string phone)
    {
        var res = await _context.Persons
            .Where(p => p.Phone == phone)
            .FirstOrDefaultAsync(); 
        return res;
    }

    internal async Task<ContactPerson?> GetByEmail(string email)
    {
        var res = await _context.Persons
            .Where(p => p.Email == email)
            .FirstOrDefaultAsync();
        return res;
    }

    public async Task<Organization?> GetOrgByPerson(ContactPerson person)
    {
        var res = await _context.Organizations
            .Where(o => o.Id == person.OrgId)
            .FirstOrDefaultAsync();
        return res;
    }

    internal async Task<ContactPerson?> AddPerson(ContactPerson personData)
    {
        _context.Persons.Add(personData);
        await _context.SaveChangesAsync();
        return personData;
    }

    public async Task<Organization?> AddOrganization(Organization organization)
    {
        _context.Organizations.Add(organization);
        await _context.SaveChangesAsync();
        return organization;
    }
}
