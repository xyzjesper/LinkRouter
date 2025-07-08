using LinkRouter.App.Models;
using Newtonsoft.Json;

namespace LinkRouter.App.Configuration;

public class Config
{
    [JsonProperty("RouteOn/")] public string RootRoute { get; set; } = "https://example.com";

    public LinkTreeConfig LinkTree { get; set; } = new();

    public RedirectRoute[] Routes { get; set; } =
    [
        new RedirectRoute()
        {
            Route = "/instagram",
            RedirectUrl = "https://instagram.com/{yourname}"
        },
        new RedirectRoute()
        {
            Route = "/example",
            RedirectUrl = "https://example.com"
        },
    ];
}

public class LinkTreeConfig
{
    public bool AddLinkTreePage { get; set; } = false;
    public bool RedirectNotFoundToLinkTree { get; set; } = true;
    public string LinkTreePageUrl { get; set; } = "/";

    public LinkTreeHTML LinkTreeHTML { get; set; } = new();
}

public class LinkTreeHTML
{
    public string Title { get; set; } = "";
}