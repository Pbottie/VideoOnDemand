namespace VOD.Membership.Database.Extensions;

public static class VODContextExtensions
{

    public static async Task SeedMembershipData(this IDbService service)
    {
        string description = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?";
        try
        {
            await service.AddAsync<Instructor, InstructorDTO>(new InstructorDTO
            {
                Name = "John Doe",
                Description = description.Substring(20, 50),
                Avatar = "/images/Ice-Age-Scrat-icon.png"
            });
            await service.AddAsync<Instructor, InstructorDTO>(new InstructorDTO
            {
                Name = "Jane Doe",
                Description = description.Substring(0, 30),
                Avatar = "/images/Ice-Age-Scrat-icon.png"
            });

            await service.SaveChangesAsync();

            var instructor1 = await service.SingleAsync<Instructor, InstructorDTO>(i => i.Name.Equals("John Doe"));
            var instructor2 = await service.SingleAsync<Instructor, InstructorDTO>(i => i.Name.Equals("Jane Doe"));

            await service.AddAsync<Course, CourseDTO>(new CourseDTO
            {
                Title = "Course 1",
                Description = description.Substring(23, 43),
                ImageUrl = "/images/course1.jpg",
                MarqueeImageUrl = "images/laptop.jpg",
                Free = false,
                InstructorId = instructor1.Id,

            });
            await service.AddAsync<Course, CourseDTO>(new CourseDTO
            {
                Title = "Course 2",
                Description = description.Substring(42, 68),
                ImageUrl = "/images/course2.jpg",
                MarqueeImageUrl = "images/laptop.jpg",
                Free = true,
                InstructorId = instructor2.Id,

            });
            await service.AddAsync<Course, CourseDTO>(new CourseDTO
            {
                Title = "Course 3",
                Description = description.Substring(50, 75),
                ImageUrl = "/images/course3.jpg",
                MarqueeImageUrl = "images/laptop.jpg",
                Free = false,
                InstructorId = instructor1.Id,
            });

            await service.SaveChangesAsync();

            var course1 = await service.SingleAsync<Course, CourseDTO>(c => c.Title.Equals("Course 1"));
            var course2 = await service.SingleAsync<Course, CourseDTO>(c => c.Title.Equals("Course 2"));
            var course3 = await service.SingleAsync<Course, CourseDTO>(c => c.Title.Equals("Course 3"));


            await service.AddAsync<Section, SectionDTO>(new SectionDTO
            {
                Title = "Section 1",
                CourseId = course1.Id

            });
            await service.AddAsync<Section, SectionDTO>(new SectionDTO
            {
                Title = "Section 2",
                CourseId = course1.Id

            }); await service.AddAsync<Section, SectionDTO>(new SectionDTO
            {
                Title = "Section 3",
                CourseId = course2.Id

            });

            await service.SaveChangesAsync();

            var section1 = await service.SingleAsync<Section, SectionDTO>(s => s.Title.Equals("Section 1"));
            var section2 = await service.SingleAsync<Section, SectionDTO>(s => s.Title.Equals("Section 2"));
            var section3 = await service.SingleAsync<Section, SectionDTO>(s => s.Title.Equals("Section 3"));

            await service.AddAsync<Video, VideoDTO>(new VideoDTO
            {
                Title = "Video Title 1",
                Description = description.Substring(43, 23),
                Duration = 50,
                Thumbnail = "/images/video1.jpg",
                Url = "https://www.youtube.com/watch?v=VgseWz45zGo",
                SectionId = section1.Id
            }); 
            await service.AddAsync<Video, VideoDTO>(new VideoDTO
            {
                Title = "Video Title 2",
                Description = description.Substring(43, 20),
                Duration = 45,
                Thumbnail = "/images/video2.jpg",
                Url = "https://www.youtube.com/watch?v=wrJ6_GAprFE",
                SectionId = section1.Id
            }); 
            await service.AddAsync<Video, VideoDTO>(new VideoDTO
            {
                Title = "Video Title 3",
                Description = description.Substring(43, 15),
                Duration = 41,
                Thumbnail = "/images/video3.jpg",
                Url = "https://www.youtube.com/watch?v=3M4q4JWEioQ",
                SectionId = section1.Id
            });
            await service.AddAsync<Video, VideoDTO>(new VideoDTO
            {
                Title = "Video Title 4",
                Description = description.Substring(0, 15),
                Duration = 41,
                Thumbnail = "/images/video4.jpg",
                Url = "https://www.youtube.com/watch?v=l-iHDj3EwdI",
                SectionId = section3.Id
            });
             await service.AddAsync<Video, VideoDTO>(new VideoDTO
            {
                Title = "Video Title 5",
                Description = description.Substring(0, 29),
                Duration = 42,
                Thumbnail = "/images/video5.jpg",
                Url = "https://www.youtube.com/watch?v=cjtMtxjjg8o",
                SectionId = section2.Id
            });

            await service.SaveChangesAsync();

        }

        catch (Exception)
        {

            throw;
        }

    }


}
