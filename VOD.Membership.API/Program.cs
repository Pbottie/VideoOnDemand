namespace VOD.Membership.API;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddCors(policy =>
        {
            policy.
            AddPolicy("CorsAllAccessPolicy", opt => opt.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
        });




        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<VODContext>(
            options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("VODConnection")));



        var config = new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Video, VideoDTO>().ReverseMap();

            cfg.CreateMap<Instructor, InstructorDTO>().ReverseMap()
            .ForMember(dest => dest.Courses, src => src.Ignore());

            cfg.CreateMap<Course, CourseDTO>().ReverseMap()
            .ForMember(dest => dest.Instructor, src => src.Ignore()); //Only need for seed?

            cfg.CreateMap<Section, SectionDTO>()
            .ForMember(dest => dest.Course, src => src.MapFrom(s => s.Course.Title))
            .ReverseMap()
            .ForMember(dest => dest.Course, src => src.Ignore());

            cfg.CreateMap<CourseCreateDTO, Course>();
            cfg.CreateMap<CourseEditDTO, Course>();


            cfg.CreateMap<SectionCreateDTO, Section>();
            cfg.CreateMap<SectionEditDTO, Section>();    
            
            cfg.CreateMap<VideoCreateDTO, Video>();
            cfg.CreateMap<VideoEditDTO, Video>();





        });

        var mapper = config.CreateMapper();
        builder.Services.AddSingleton(mapper);

        builder.Services.AddScoped<IDbService, DbService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("CorsAllAccessPolicy");
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}