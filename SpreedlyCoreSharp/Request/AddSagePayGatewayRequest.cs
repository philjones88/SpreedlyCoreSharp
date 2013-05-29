using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Request
{
    [XmlRoot("gateway")]
    public class AddSagePayGatewayRequest
    {
        public AddSagePayGatewayRequest()
        {
            GatewayType = "sage_pay";
        }

        [XmlElement("gateway_type")]
        public string GatewayType { get; set; }

        [XmlElement("login")]
        public string Login { get; set; }
    }
}
