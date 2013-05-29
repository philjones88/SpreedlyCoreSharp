using Nancy;
using Nancy.Responses;
using SpreedlyCoreSharp.Request;

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

            Get["/add-test-gateway"] = _ =>
                {
                    service.AddGateway(new AddTestGatewayRequest());
                    
                    return new RedirectResponse("/gateways");
                };

            Get["/redact/{token}"] = parameters =>
                {
                    service.RedactGateway((string) parameters.token);

                    return new RedirectResponse("/gateways");
                };
        }
    }
}