using RestSharp.Serializers;

namespace SpreedlyCoreSharp.Request
{
    [SerializeAs(Name = "gateway")]
    public class AuthorizeNetGatewayRequest : BaseGatewayRequest
    {
        public AuthorizeNetGatewayRequest()
        {
            GatewayType = "authorize_net";
        }

        [SerializeAs(Name = "login")]
        public string Login { get; set; }

        [SerializeAs(Name = "password")]
        public string Password { get; set; }
    }
}
