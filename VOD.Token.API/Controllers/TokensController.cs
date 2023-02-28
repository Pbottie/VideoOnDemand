namespace VOD.Token.API.Controllers;

[ApiController]
public class TokensController : ControllerBase
{
    private readonly ITokenService _tokenService;
    public TokensController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [Route("api/tokens")]
    [HttpPost]
    public async Task<IResult> Get([FromBody] LoginUserDTO loginUser)
    {
        try
        {
            var result = await _tokenService.GetTokenAsync(loginUser);

            if (result.UserName is null || result.AccessToken is null)
                return Results.Unauthorized();

            return Results.Ok(result);
        }
        catch
        { }

        return Results.Unauthorized();
    }



    [Route("api/tokens/create")]
    [HttpPost]
    public async Task<IResult> Create([FromBody] TokenUserDTO tokenUser)
    {
        try
        {
            var jwt = await _tokenService.GenerateTokenAsync(tokenUser);

            if (string.IsNullOrEmpty(jwt)) return Results.Unauthorized();

            return Results.Created("Token", jwt);
        }
        catch { }

        return Results.Unauthorized();

    }
}
