namespace VOD.Token.API.Services
{
    public interface ITokenService
    {
        string? CreateToken(IList<string> roles, VODUser user);
        Task<string?> GenerateTokenAsync(TokenUserDTO tokenUserDTO);
        Task<AuthenticatedUserDTO> GetTokenAsync(LoginUserDTO loginUserDTO);
    }
}