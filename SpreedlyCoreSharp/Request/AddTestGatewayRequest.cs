using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Request
{
    [XmlRoot("gateway")]
    public class AddTestGatewayRequest
    {
        public AddTestGatewayRequest()
        {
            GatewayType = "test";
        }

        [XmlElement("gateway_type")]
        public string GatewayType { get; set; }
    }
}
