using System.Text.Json;
using LinkRouter.App.Configuration;
using LinkRouter.App.Http.Pages.Admin;
using LinkRouter.App.Services;
using MoonCore.Extensions;
using MoonCore.Helpers;
using MoonCore.Services;

namespace LinkRouter;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Directory.CreateDirectory(PathBuilder.Dir("data"));

        builder.Services.AddHostedService<ConfigWatcher>();

        var configPath = Path.Combine("data", "config.json");

        if (!File.Exists(configPath))
            File.WriteAllText(
                configPath,
                JsonSerializer.Serialize(new Config(), new JsonSerializerOptions { WriteIndented = true }
                ));

        var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath)) ?? new Config();

        builder.Services.AddSingleton(config);

        builder.Services.AddScoped<AdminService>();

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddControllers();
        builder.Services.AddAntiforgery();

        builder.Services.AddControllersWithViews()
            .AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("~/App/Http/Views/{1}/{0}.cshtml");
                options.ViewLocationFormats.Add("~/App/Http/Views/Shared/{0}.cshtml");
            });


        builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.ListenAnyIP(80); });

        var loggerProviders = LoggerBuildHelper.BuildFromConfiguration(configuration =>
        {
            configuration.Console.Enable = true;
            configuration.Console.EnableAnsiMode = true;
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddProviders(loggerProviders);

        var app = builder.Build();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAntiforgery();

        app.MapControllers();

        app.MapRazorComponents<App.Http.Pages.App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}