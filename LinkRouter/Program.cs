

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

        if (!File.Exists(configFilePath))
            File.WriteAllText(configFilePath, "{}");
        
        ConfigService<Config> configService = new(configFilePath);
        
        builder.Services.AddSingleton(configService);
        
        builder.Services.AddControllers();
        
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenAnyIP(80);
        });

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}