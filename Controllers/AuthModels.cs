using System.ComponentModel.DataAnnotations;

namespace Auth;

public class LoginModel
{
    public required string Password { get; set; }

    private string? _email;

    [EmailAddress]
    public string Email { 
        get => string.IsNullOrEmpty(_email) ? "default@example.com" : _email; 
        set => _email = value; 
    }

    public string Phone { get; set; } = string.Empty;

    public bool IsEmailSet() => !string.IsNullOrEmpty(_email);
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