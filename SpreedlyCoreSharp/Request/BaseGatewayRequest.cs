using RestSharp.Serializers;

namespace SpreedlyCoreSharp.Request
{
    [SerializeAs(Name = "gateway")]
    public abstract class BaseGatewayRequest
    {
        [SerializeAs(Name = "gateway_type")]
        public string GatewayType { get; set; }
    }
}
