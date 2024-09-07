using LinkRouter.App.Models;
using Newtonsoft.Json;

namespace LinkRouter.App.Configuration;

public class Config
{
    [JsonProperty("RouteOn/")]
    public string RootRoute { get; set; } = "https://example.com";

    public List<RedirectRoute> Routes { get; set; } = new (
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
    ]);
}