﻿using Nancy;
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
                    service.AddGateway(new AddTestGatewayRequest { GatewayType = "test" });

                    return new RedirectResponse("/gateways");
                };
        }
    }
}