using LinkRouter.App.Configuration;
using Microsoft.AspNetCore.Mvc;
using Prometheus;


namespace LinkRouter.App.Http.Controllers;

[ApiController]
public class RedirectController : Controller
{

    private readonly Config Config;
    
      
    private readonly Counter RouteCounter = Metrics.CreateCounter(
        "linkrouter_requests",
        "Counts the number of requests to the link router",
        new CounterConfiguration
        {
            LabelNames = new[] { "route" }
        }
    );
    
    
    private readonly Counter NotFoundCounter = Metrics.CreateCounter(
        "linkrouter_404_requests",
        "Counts the number of not found requests to the link router",
        new CounterConfiguration
        {
            LabelNames = new[] { "route" }
        }
    );

    public RedirectController(Config config)
    {
        Config = config;
    }

    [HttpGet("/{*path}")]
    public IActionResult RedirectToExternalUrl(string path)
    {
        var redirectRoute = Config.Routes.FirstOrDefault(x => x.Route == path || x.Route == path + "/" || x.Route == "/" + path);

        if (redirectRoute != null)
        {
            RouteCounter
                .WithLabels(redirectRoute.Route)
                .Inc();
            
            return Redirect(redirectRoute.RedirectUrl);
        }
            
        NotFoundCounter
            .WithLabels(path)
            .Inc();
        
        if (Config.NotFoundBehavior.RedirectOn404)
            return Redirect(Config.NotFoundBehavior.RedirectUrl);
        
        return NotFound();
    }
    
    [HttpGet("/")]
    public IActionResult GetRootRoute()
    {
        RouteCounter
            .WithLabels("/")
            .Inc();
        
        string url = Config.RootRoute;
        
        return Redirect(url);
    }
}