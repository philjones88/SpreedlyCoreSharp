using RestSharp.Serializers;

namespace SpreedlyCoreSharp.Request
{
    [SerializeAs(Name = "gateway")]
    public class StripeGatewayRequest : BaseGatewayRequest
    {
        public StripeGatewayRequest()
        {
            GatewayType = "stripe";
        }

        [SerializeAs(Name = "login")]
        public string Login { get; set; }
    }
}
