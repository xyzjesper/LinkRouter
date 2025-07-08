using LinkRouter.App.Configuration;
using LinkRouter.App.Models;
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

    [HttpGet("/{path}")]
    public IActionResult? RedirectToExternalUrl(string path)
    {
        var redirectRoute =
            Config.Routes.FirstOrDefault(x => x.Route == path || x.Route == path + "/" || x.Route == "/" + path);

        if (Config.LinkTree.AddLinkTreePage && (path == Config.LinkTree.LinkTreePageUrl ||
                                                path + "/" == Config.LinkTree.LinkTreePageUrl ||
                                                "/" + path == Config.LinkTree.LinkTreePageUrl))
        {
            return View(new LinkTreeViewModel
            {
                Title = Config.LinkTree.LinkTreeHTML.Title,
                Author = Config.LinkTree.LinkTreeHTML.Author,
                Description = Config.LinkTree.LinkTreeHTML.Description,
                BackgroundColor = Config.LinkTree.LinkTreeHTML.BackgroundColor,
                AuthorIconUrl = Config.LinkTree.LinkTreeHTML.AuthorIconUrl,
                FaviconUrl = Config.LinkTree.LinkTreeHTML.FaviconUrl,
                TextColor = Config.LinkTree.LinkTreeHTML.TextColor,
                Links = Config.Routes
            });
        }

        if (redirectRoute != null) return Redirect(redirectRoute.RedirectUrl);
        return Config.LinkTree.RedirectNotFoundToLinkTree ? Redirect(Config.LinkTree.LinkTreePageUrl) : Redirect(path);
    }

    [HttpGet("/")]
    public IActionResult? GetRootRoute()
    {
        string url = Config.RootRoute;

        return Config.LinkTree is { LinkTreePageUrl: "/", AddLinkTreePage: true } ? null : Redirect(url);
    }
}