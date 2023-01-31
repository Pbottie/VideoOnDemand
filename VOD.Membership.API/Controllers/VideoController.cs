namespace VOD.Membership.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VideoController : ControllerBase
{
    private IDbService _db;

    public VideoController(IDbService db) => _db = db;
    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            List<VideoDTO> videos = await _db.GetAsync<Video, VideoDTO>();

            return Results.Ok(videos);
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

            var video = await _db.SingleAsync<Video, VideoDTO>(v => v.Id.Equals(id));
            if (video is null)
                return Results.NotFound();

            var section = await _db.SingleAsync<Section, SectionDTO>(s => s.Id.Equals(video.SectionId));

            if (section != null)
            {
                video.Section = section.Title;

                var course = await _db.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(section.CourseId));
                if (course != null)
                {
                    video.Course = course.Title;
                    video.CourseId = course.Id;
                }
            }


            return Results.Ok(video);

        }
        catch (Exception)
        {

        }
        return Results.NotFound();

    }
    [HttpPost]
    public async Task<IResult> Post([FromBody] VideoCreateDTO dto)
    {

        try
        {
            if (dto is null)
                return Results.BadRequest();

            var video = _db.AddAsync<Video, VideoCreateDTO>(dto);

            var success = await _db.SaveChangesAsync();

            if (success)
                return Results.Created($"/Videos/{video.Result.Id}", video);
            else
                return Results.BadRequest();


        }
        catch (Exception)
        {

        }
        return Results.BadRequest();

    }
    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, [FromBody] VideoEditDTO dto)
    {

        try
        {
            if (dto is null || !dto.Id.Equals(id)) return Results.BadRequest();

            var exists = await _db.AnyAsync<Section>(s => s.Id.Equals(dto.SectionId));
            if (!exists)
                return Results.NotFound();


            exists = await _db.AnyAsync<Video>(v => v.Id.Equals(id));
            if (!exists)
                return Results.NotFound();

            _db.Update<Video, VideoEditDTO>(id, dto);

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
            var success = await _db.DeleteAsync<Video>(id);

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
