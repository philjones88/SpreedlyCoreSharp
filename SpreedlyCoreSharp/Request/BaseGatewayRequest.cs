using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Request
{
    [XmlRoot("gateway")]
    public abstract class BaseGatewayRequest
    {
        [XmlElement("gateway_type")]
        public string GatewayType { get; set; }
    }
}
