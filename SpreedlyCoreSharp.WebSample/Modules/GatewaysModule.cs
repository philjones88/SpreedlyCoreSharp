using Nancy;

namespace SpreedlyCoreSharp.WebSample.Modules
{
    public class GatewaysModule : NancyModule
    {
        public GatewaysModule(ICoreService service) : base ("/gateways")
        {
            Get["/"] = _ =>
                {
                    var gateways = service.GetGateways();

                    return View["Gateways", gateways];
                };
        }
    }
}