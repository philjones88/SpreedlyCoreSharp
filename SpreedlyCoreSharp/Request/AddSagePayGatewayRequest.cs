using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Request
{
    [XmlRoot("gateway")]
    public class AddSagePayGatewayRequest : BaseGatewayRequest
    {
        public AddSagePayGatewayRequest()
        {
            GatewayType = "sage_pay";
        }

        [XmlElement("login")]
        public string Login { get; set; }
    }
}
