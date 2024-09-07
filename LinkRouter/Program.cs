

using LinkRouter.App.Configuration;
using MoonCore.Helpers;
using MoonCore.Services;

namespace LinkRouter;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        Directory.CreateDirectory(PathBuilder.Dir("data"));

        var configFilePath = PathBuilder.File("data", "config.json");

        ConfigService<Config> configService = new(configFilePath);
        
        builder.Services.AddSingleton(configService);
        
        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}