namespace VOD.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private IDbService _db;

    public CourseController(IDbService db) => _db = db;
    [HttpGet]
    public async Task<IResult> Get(bool freeOnly)
    {
        try
        {
            _db.Include<Instructor>();

            List<CourseDTO> courses = null;

            if (freeOnly)
            {
                courses = await _db.GetAsync<Course, CourseDTO>(c => c.Free.Equals(freeOnly));

            }
            else
            {
                courses = await _db.GetAsync<Course, CourseDTO>();
            }

            return Results.Ok(courses);
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
            _db.Include<Instructor>();
            _db.Include<Section>();
            _db.Include<Video>();

            var course = await _db.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(id));
            if (course is null)
                return Results.NotFound();

            return Results.Ok(course);

        }
        catch (Exception)
        {

        }

        return Results.NotFound();

    }
    [HttpPost]
    public async Task<IResult> Post([FromBody] CourseCreateDTO dto)
    {

        try
        {
            if (dto is null)
                return Results.BadRequest();

            var course = await _db.AddAsync<Course, CourseCreateDTO>(dto);

            var success = await _db.SaveChangesAsync();

            if (success)
                return Results.Created(_db.GetURI<Course>(course), course);
            else
                return Results.BadRequest();


        }
        catch (Exception)
        {

        }

        return Results.BadRequest();

    }
    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, [FromBody] CourseEditDTO dto)
    {

        try
        {
            if (dto is null || !dto.Id.Equals(id)) return Results.BadRequest();

            var exists = await _db.AnyAsync<Instructor>(i => i.Id.Equals(dto.InstructorId));
            if (!exists)
                return Results.NotFound();


            exists = await _db.AnyAsync<Course>(c => c.Id.Equals(id));
            if (!exists)
                return Results.NotFound();

            _db.Update<Course, CourseEditDTO>(id, dto);

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
            var success = await _db.DeleteAsync<Course>(id);

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
