using LinkRouter.App.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace LinkRouter.App.Http.Controllers;

[ApiController]
public class RedirectController : Controller
{

    private readonly Config Config;

    public RedirectController(Config config)
    {
        Config = config;
    }

    [HttpGet("/{*path}")]
    public IActionResult RedirectToExternalUrl(string path)
    {
        var redirectRoute = Config.Routes.FirstOrDefault(x => x.Route == path || x.Route == path + "/" || x.Route == "/" + path);

        if (redirectRoute != null)
            return Redirect(redirectRoute.RedirectUrl);
            
        
        if (Config.NotFoundBehavior.RedirectOn404)
            return Redirect(Config.NotFoundBehavior.RedirectUrl);
        
        return NotFound();
    }
    
    [HttpGet("/")]
    public IActionResult GetRootRoute()
    {
        string url = Config.RootRoute;
        
        return Redirect(url);
    }
}