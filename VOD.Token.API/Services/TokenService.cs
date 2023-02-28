namespace VOD.Token.API.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    private readonly UserManager<VODUser> _userManager;
    public TokenService(IConfiguration configuration, IUserService userService, UserManager<VODUser> userManager)
    {
        _configuration = configuration;
        _userService = userService;
        _userManager = userManager;

    }




    public string? CreateToken(IList<string> roles, VODUser user)
    {
        try
        {
            if (roles == null
                || user == null
                || _configuration["Jwt:SigningSecret"] is null
                || _configuration["Jwt:Duration"] is null
                || _configuration["Jwt:Issuer"] is null
                || _configuration["Jwt:Audience"] is null)
            {
                throw new ArgumentException("JWT configuartion missing");
            }

            var signingKey = Convert.FromBase64String(_configuration["Jwt:SigningSecret"] ?? "");
            var credentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature);
            var duration = int.Parse(_configuration["Jwt:Duration"] ?? "");
            var now = DateTimeOffset.UtcNow;
            var expires = now.AddDays(duration);


            List<Claim> claims = new() {new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"] ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"] ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Nbf, now.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expires.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = new JwtSecurityToken(new JwtHeader(credentials), new JwtPayload(claims));

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
        catch
        {

            throw;
        }
    }


    public async Task<string?> GenerateTokenAsync(TokenUserDTO tokenUserDTO)
    {

        try
        {
            var user = await _userService.GetUserAsync(tokenUserDTO.Email);


            if (user == null) throw new UnauthorizedAccessException();

            var roles = await _userManager.GetRolesAsync(user);

            var token = CreateToken(roles, user);

            if (tokenUserDTO.Save)
            {
                var result = await _userManager.SetAuthenticationTokenAsync(user, "VOD", "UserToken", token);

                if (!result.Succeeded) throw new SecurityTokenException("Could not add token to user");
            }

            return token;
        }
        catch (Exception)
        {

            throw;
        }
    }


    public async Task<AuthenticatedUserDTO> GetTokenAsync(LoginUserDTO loginUserDTO)
    {
        try
        {
            if (loginUserDTO is null)
                throw new UnauthorizedAccessException();

            var user = await _userService.GetUserAsync(loginUserDTO);

            if (user is null) throw new UnauthorizedAccessException();

            var token = await _userManager.GetAuthenticationTokenAsync(user, "VOD", "UserToken");

            var compareToken = await GenerateTokenAsync(new TokenUserDTO(loginUserDTO.Email, false));


            var success = JwtParser.CompareTokenClaims(token, compareToken);
            if (!success) token = await GenerateTokenAsync(new TokenUserDTO(loginUserDTO.Email));

            return new AuthenticatedUserDTO(token, user.UserName);

        }
        catch (Exception)
        {

            throw;
        }


    }

}
