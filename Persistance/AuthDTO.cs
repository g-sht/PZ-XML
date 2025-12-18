using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Persistance;

public class Organization
{
    [Key]
    [Column("organization_id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Column("organization_name")]
    public string OrgName { get; set; } = string.Empty;
    [Column("inn")]
    public required ulong Inn { get; set; }
    [Column("ogrn")]
    public required ulong Ogrn { get; set; }
}

public class ContactPerson
{
    [Key]
    [Column("contact_person_id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;

    [Column("phone")]
    public string Phone { get; set; } = string.Empty;
    
    [Column("email")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("organization_id")]
    public Guid OrgId { get; set; } 
}

public class UserInfo
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string OrgName { get; set; }
    public string OrgID { get; set; }
    public string PersonID { get; set; }
}