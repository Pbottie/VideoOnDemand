namespace VOD.Membership.UI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


        builder.Services.AddHttpClient<MembershipHttpClient>(client =>
        client.BaseAddress = new Uri("https://localhost:6001/api/"));

        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<IMembershipService, MembershipService>();


        await builder.Build().RunAsync();
    }
}