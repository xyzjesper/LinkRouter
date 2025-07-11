using System.Text.Json;
using LinkRouter.App.Configuration;
using LinkRouter.App.Models;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.Protocol;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace LinkRouter.App.Services;

public class AdminService
{
    private Config Config;

    public AdminService(Config config)
    {
        Config = config;
    }

    public async Task<bool> IsValidAdminPassword(string password)
    {
        return password == Config.AdminPassword;
    }

    public async Task<bool> AddNewLink(RedirectRoute route)
    {
        if (Config.Routes.Any(redirectRoute => redirectRoute.Route == route.Route)) return false;

        Config.Routes = Config.Routes.Append(route).ToArray();

        var configPath = Path.Combine("data", "config.json");
        await File.WriteAllTextAsync(
            configPath,
            Config.ToJson());

        return true;
    }

    public async Task<bool> RemoveLink(string route)
    {
        if (Config.Routes.All(redirectRoute => redirectRoute.Route != route)) return false;

        var routeList = Config.Routes.ToList();

        var configRoute = routeList.FirstOrDefault(redirectRoute => redirectRoute.Route == route);
        routeList.Remove(configRoute!);

        Config.Routes = routeList.ToArray();

        var configPath = Path.Combine("data", "config.json");
        await File.WriteAllTextAsync(
            configPath,
            Config.ToJson());

        return Config.Routes.All(redirectRoute => redirectRoute.Route != route);
    }
}