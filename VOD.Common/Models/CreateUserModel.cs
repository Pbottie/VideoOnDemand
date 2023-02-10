namespace VOD.Common.Models;

public class CreateUserModel
{
    [Required, EmailAddress, MinLength(6)] public string Email { get; set; } = String.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = String.Empty;
    [Required, MinLength(6), DisplayName("Confirm Password"), Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ComfirmPassword { get; set; } = String.Empty;
    public bool IsCustomer { get; set; } = false;
    public bool IsAdmin { get; set; } = false;


}
