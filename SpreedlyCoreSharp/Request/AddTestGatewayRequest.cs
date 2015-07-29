using RestSharp.Serializers;

namespace SpreedlyCoreSharp.Request
{
    [SerializeAs(Name = "gateway")]
    public class AddTestGatewayRequest : BaseGatewayRequest
    {
        public AddTestGatewayRequest()
        {
            GatewayType = "test";
        }
    }
}
