﻿namespace VOD.Common.Services;

public class MembershipService : IMembershipService
{
    protected readonly MembershipHttpClient _http;

    public MembershipService(MembershipHttpClient httpClient)
    {
        _http = httpClient;
    }




    public async Task<List<CourseDTO>> GetCoursesAsync()
    {
        try
        {

            bool freeOnly = false;

            using HttpResponseMessage response = await _http.Client.GetAsync($"course?freeOnly={freeOnly}");
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<List<CourseDTO>>(await
                response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new List<CourseDTO>();


        }
        catch (Exception)
        {

            return new List<CourseDTO>();
        }


    }
    public async Task<CourseDTO> GetCourseAsync(int? id)
    {
        try
        {

            if (id is null)
                return new CourseDTO();

            using HttpResponseMessage response = await _http.Client.GetAsync($"course/{id}");
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<CourseDTO>(await
                response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new CourseDTO();


        }
        catch (Exception)
        {

            return new CourseDTO();
        }
    }
    public async Task<VideoDTO> GetVideoAsync(int? id)
    {
        try
        {

            if (id is null)
                return new VideoDTO();

            using HttpResponseMessage response = await _http.Client.GetAsync($"video/{id}");
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<VideoDTO>(await
                response.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new VideoDTO();


        }
        catch (Exception)
        {

            return new VideoDTO();
        }


    }
}
