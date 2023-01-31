namespace VOD.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedController : ControllerBase
{

    IDbService _db;

    public SeedController(IDbService db) => _db = db;

    [HttpPost, AllowAnonymous]
    public async Task<IResult> Seed()
    {
        try
        {
            await _db.SeedMembershipData();
            return Results.NoContent();
        }
        catch (Exception)
        {
        }
        return Results.BadRequest();
    }

}

