using Backend.API.Services;
using Backend.API.Settings;
using Serilog;
using System.Net;

namespace Backend.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

        //await CreateHostBuilder(args).Build().RunAsync();       
        var host = CreateHostBuilder(args).Build();

        // 🔽 Run the admin seeding logic before app runs
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                await IdentityDataInitializer.SeedAdminUserAsync(services);
                await IdentityDataInitializer.SeedRolesAsync(services);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Admin seeding failed: " + ex.Message);
                throw;
            }
        }

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders())
            .UseSerilog(SerilogOptions.ConfigureLogger);
}