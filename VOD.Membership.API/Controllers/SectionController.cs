namespace VOD.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SectionController : ControllerBase
{
    private IDbService _db;

    public SectionController(IDbService db) => _db = db;
    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            _db.Include<Video>();

            var sections = await _db.GetAsync<Section, SectionDTO>();

            return Results.Ok(sections);
        }
        catch (Exception)
        {

        }
        return Results.NotFound();

    }

    [HttpGet("{id}")]
    public async Task<IResult> Get(int id)
    {
        try
        {
            //_db.Include<Instructor>();
            //_db.Include<Section>();
            _db.Include<Video>();

            var section = await _db.SingleAsync<Section, SectionDTO>(s => s.Id.Equals(id));
            return Results.Ok(section);

        }
        catch (Exception)
        {

        }

        return Results.NotFound();

    }
    [HttpPost]
    public async Task<IResult> Post([FromBody] SectionCreateDTO dto)
    {

        try
        {
            if (dto is null)
                return Results.BadRequest();

            var section = _db.AddAsync<Section, SectionCreateDTO>(dto);

            var success = await _db.SaveChangesAsync();

            if (success)
                return Results.Created($"/Section/{section.Result.Id}", section);
            else
                return Results.BadRequest();


        }
        catch (Exception)
        {

        }

        return Results.BadRequest();

    }
    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, [FromBody] SectionEditDTO dto)
    {

        try
        {
            if (dto is null)
                return Results.BadRequest("Null Dto");
            if (!dto.Id.Equals(id))
                return Results.BadRequest("Not same Id");

            var exists = await _db.AnyAsync<Course>(c => c.Id.Equals(dto.CourseId));
            if (!exists)
                return Results.NotFound();


            exists = await _db.AnyAsync<Section>(s => s.Id.Equals(id));
            if (!exists)
                return Results.NotFound();

            _db.Update<Section, SectionEditDTO>(id, dto);

            var success = await _db.SaveChangesAsync();

            if (success) return Results.NoContent();
            else
                return Results.BadRequest();


        }
        catch (Exception)
        {

        }

        return Results.BadRequest();

    }
    [HttpDelete("{id}")]
    public async Task<IResult> Delete(int id)
    {
        try
        {
            var success = await _db.DeleteAsync<Section>(id);

            if (!success)
                return Results.NotFound();

            success = await _db.SaveChangesAsync();

            if (success) return Results.NoContent();
            else
                return Results.BadRequest();

        }
        catch (Exception)
        {

        }
        return Results.BadRequest();

    }
}
