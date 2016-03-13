using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Request
{
    [XmlRoot("gateway")]
    public class AddAuthorizeNetGatewayRequest : BaseGatewayRequest
    {
        public AddAuthorizeNetGatewayRequest()
        {
            GatewayType = "authorize_net";
        }

        [XmlElement("login")]
        public string Login { get; set; }

        [XmlElement("password")]
        public string Password { get; set; }
    }
}
