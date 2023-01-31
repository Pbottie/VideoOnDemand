namespace VOD.Membership.Database.Contexts;
public class VODContext : DbContext
{
    DbSet<Course> Courses { get; set; }
    DbSet<Instructor> Instructors { get; set; }
    DbSet<Section> Sections { get; set; }
    DbSet<Video> Videos { get; set; }


    public VODContext(DbContextOptions<VODContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var relationship in builder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        };
    }

}
