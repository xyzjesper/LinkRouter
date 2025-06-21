using System.Text.Json;
using System.IO;
using LinkRouter.App.Configuration;
using Microsoft.AspNetCore.Mvc;
using MoonCore.Services;

namespace LinkRouter.App.Http.Controllers;

[ApiController]
public class RedirectController : Controller
{

    private readonly ConfigService<Config> ConfigService;

    public RedirectController(ConfigService<Config> configService)
    {
        ConfigService = configService;
    }

    [HttpGet("/{*path}")]
    public IActionResult RedirectToExternalUrl(string path)
    {
        var configPath = Path.Combine("data", "config.json");
        
        if (!System.IO.File.Exists(configPath) || string.IsNullOrEmpty(System.IO.File.ReadAllText(configPath)))
        {
            return NotFound();
        }
        
        var config = JsonSerializer.Deserialize<Config>(System.IO.File.ReadAllText(configPath));

        if (config == null)
            return NotFound();
        
        var redirectRoute = config.Routes.FirstOrDefault(x => x.Route == path || x.Route == path + "/");

        if (redirectRoute == null)
            return NotFound();
        
        
        return Redirect(redirectRoute.RedirectUrl);
    }
    
    [HttpGet("/")]
    public IActionResult GetRootRoute()
    {
        string url = ConfigService.Get().RootRoute;
        return Redirect(url);
    }
}