using LinkRouter.App.Configuration;
using Microsoft.AspNetCore.Mvc;
using MoonCore.Services;

namespace LinkRouter.App.Http.Controllers;

[ApiController]
[Route("[controller]")]
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
        path = "/" + path.Trim('/');

        var redirectRoute = ConfigService.Get().Routes.FirstOrDefault(x => x.Route == path || x.Route == path + "/");

        if (redirectRoute == null)
        {
            return NotFound();
        }
        
        return Redirect(redirectRoute.RedirectUrl);
    }
    
    [HttpGet("/")]
    public IActionResult GetRootRoute()
    {
        string url = ConfigService.Get().RootRoute;
        return Redirect(url);
    }
}