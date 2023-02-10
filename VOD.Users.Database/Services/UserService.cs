namespace VOD.Users.Database.Services;

public class UserService : IUserService
{
    UserManager<VODUser> _userManager;

    public UserService(UserManager<VODUser> userManager)
    {
        _userManager = userManager;
    }


    public async Task<VODUser?> GetUserAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        catch (Exception)
        {
        }

        return default;

    }
    public async Task<VODUser?> GetUserAsync(LoginUserDTO loginUser)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);
            if (user == null)
                return default;

            var hash = new PasswordHasher<VODUser>();
            var result = hash.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);

            if (result.Equals(PasswordVerificationResult.Success))
                return user;
        }
        catch (Exception)
        {
        }

        return default;

    }

}
