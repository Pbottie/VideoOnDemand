namespace VOD.Membership.Database.Entities;

public class Section : IEntity
{
    public int Id { get; set; }
    [Required, MaxLength(80)] public string? Title { get; set; }
    public int CourseId { get; set; }
    public virtual ICollection<Video>? Videos { get; set; }
    public virtual Course? Course { get; set; }
}
