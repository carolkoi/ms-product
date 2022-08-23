using ProductMcService;
using ProductMcService.DBContexts;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

public class Program
{
    public static void Main(string[] args)
    {
        //CreateHostBuilder(args).Build().Run();
        //Read Configuration from appSettings
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()

             .MinimumLevel.Override("Default", LogEventLevel.Information)
             .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Error)
             .WriteTo.File(@"/app/logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: null, retainedFileCountLimit: null,
                shared: true
                , outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
               )

            .WriteTo.Console(theme: AnsiConsoleTheme.Grayscale)



            .CreateLogger();
        try
        {
            Log.Information("Application Starting.");

            var host = CreateHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            host.Run();

            //CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The Application failed to start.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void CreateDbIfNotExists(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            Log.Information("Database seeding.");
            var context = services.GetRequiredService<ProductContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error occurred creating the DB.");
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            //Uses Serilog instead of default .NET Logger
            //.UseSerilog() 
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}