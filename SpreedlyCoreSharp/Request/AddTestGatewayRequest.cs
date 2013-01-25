using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Request
{
    [XmlRoot("gateway")]
    public class AddTestGatewayRequest
    {
        [XmlElement("gateway_type")]
        public string GatewayType { get; set; }
    }
}
