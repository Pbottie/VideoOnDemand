namespace VOD.Common.DTOs;

public class CourseCreateDTO
{
    public string ImageUrl { get; set; }
    public string MarqueeImageUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Free { get; set; }
    public int InstructorId { get; set; }
}
