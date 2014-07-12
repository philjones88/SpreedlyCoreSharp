using Nancy;

namespace SpreedlyCoreSharp.WebSample.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["Home"];
        }
    }
}