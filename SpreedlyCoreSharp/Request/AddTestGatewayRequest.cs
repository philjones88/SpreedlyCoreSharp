using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Request
{
    [XmlRoot("gateway")]
    public class AddTestGatewayRequest : BaseGatewayRequest
    {
        public AddTestGatewayRequest()
        {
            GatewayType = "test";
        }
    }
}
