namespace VOD.Common.DTOs;

public class VideoCreateDTO
{      
    public string Title { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public string Thumbnail { get; set; }
    public string Url { get; set; }
    public int SectionId { get; set; }
}
