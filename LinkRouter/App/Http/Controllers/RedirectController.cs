using LinkRouter.App.Configuration;
using LinkRouter.App.Models;
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

        if (Config.LinkTree.AddLinkTreePage && (path == Config.LinkTree.LinkTreePageUrl ||
                                                path + "/" == Config.LinkTree.LinkTreePageUrl ||
                                                "/" + path == Config.LinkTree.LinkTreePageUrl))
        {
            return View(new LinkTreeViewModel
            {
                Title = Config.LinkTree.LinkTreeHTML.Title,
                CustomCSSUrl = Config.LinkTree.LinkTreeHTML.CustomCSSUrl.Length <= 1
                    ? "css/linktree.css"
                    : Config.LinkTree.LinkTreeHTML.CustomCSSUrl,
                Author = Config.LinkTree.LinkTreeHTML.Author,
                Description = Config.LinkTree.LinkTreeHTML.Description,
                BackgroundColor = Config.LinkTree.LinkTreeHTML.BackgroundColor,
                AuthorIconUrl = Config.LinkTree.LinkTreeHTML.AuthorIconUrl,
                FaviconUrl = Config.LinkTree.LinkTreeHTML.FaviconUrl,
                Links = Config.Routes
            });
        }
        
        if (redirectRoute != null)
        {
            RouteCounter
                .WithLabels(redirectRoute.Route)
                .Inc();
            
            return Redirect(redirectRoute.RedirectUrl);
        }
            
        NotFoundCounter
            .WithLabels("/" + path)
            .Inc();
        
        return Config.LinkTree.RedirectNotFoundToLinkTree ? Redirect(Config.LinkTree.LinkTreePageUrl) : Redirect(path);
    }
    
    [HttpGet("/")]
    public IActionResult GetRootRoute()
    {
        RouteCounter
            .WithLabels("/")
            .Inc();
        
        string url = Config.RootRoute;
        
        return Config.LinkTree is { LinkTreePageUrl: "/", AddLinkTreePage: true } ? null : Redirect(url);
    }
}