namespace VOD.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstructorController : ControllerBase
{
    private IDbService _db;

    public InstructorController(IDbService db) => _db = db;
    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            var instructors = await _db.GetAsync<Instructor, InstructorDTO>();
            return Results.Ok(instructors);
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
            var instructor = await _db.SingleAsync<Instructor, InstructorDTO>(c => c.Id.Equals(id));
            if (instructor is null) return Results.NotFound();

            return Results.Ok(instructor);

        }
        catch (Exception)
        {

        }

        return Results.NotFound();

    }
    [HttpPost]
    public async Task<IResult> Post([FromBody] InstructorDTO dto)
    {

        try
        {
            if (dto is null)
                return Results.BadRequest();

            var instructor = _db.AddAsync<Instructor, InstructorDTO>(dto);

            var success = await _db.SaveChangesAsync();

            if (success)
                return Results.Created($"/Instructors/{instructor.Result.Id}", instructor);
            else
                return Results.BadRequest();


        }
        catch (Exception)
        {

        }

        return Results.BadRequest();

    }
    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, [FromBody] InstructorDTO dto)
    {

        try
        {
            if (dto is null || !dto.Id.Equals(id)) return Results.BadRequest();

            var exists = await _db.AnyAsync<Instructor>(i => i.Id.Equals(id));
            if (!exists)
                return Results.NotFound();
        
            _db.Update<Instructor, InstructorDTO>(id, dto);

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
            var success = await _db.DeleteAsync<Instructor>(id);

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
