using RestSharp.Serializers;

namespace SpreedlyCoreSharp.Request
{
    [SerializeAs(Name = "gateway")]
    public class AddSagePayGatewayRequest : BaseGatewayRequest
    {
        public AddSagePayGatewayRequest()
        {
            GatewayType = "sage_pay";
        }

        [SerializeAs(Name = "login")]
        public string Login { get; set; }
    }
}
