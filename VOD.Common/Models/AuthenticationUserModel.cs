namespace VOD.Common.Models;

public class AuthenticationUserModel
{
    [Required] public string Email { get; set; } = String.Empty;
    [Required] public string Password { get; set; } = String.Empty;
}
