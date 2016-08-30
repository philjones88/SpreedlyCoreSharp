using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Request
{
    [XmlRoot("gateway")]
    public class AddStripeGatewayRequest : BaseGatewayRequest
    {
        public AddStripeGatewayRequest()
        {
            GatewayType = "stripe";
        }

        [XmlElement("login")]
        public string Login { get; set; }
    }
}
