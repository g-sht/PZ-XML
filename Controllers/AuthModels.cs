using System.ComponentModel.DataAnnotations;

namespace Auth;
public class LoginModel
{
    [Required]
    public required string Password { get; set; }

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;
}

public class RegisterModel
{
    [Required]
    public string OrgName { get; set; } = string.Empty;

    [Required]
    public ulong Inn { get; set; }

    [Required]
    public ulong Ogrn { get; set; } 

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}