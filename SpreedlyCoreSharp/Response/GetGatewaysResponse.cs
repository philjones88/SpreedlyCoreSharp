using System.Collections.Generic;
using System.Xml.Serialization;
using SpreedlyCoreSharp.Domain;

namespace SpreedlyCoreSharp.Response
{
    [XmlRoot("gateways")]
    public class GetGatewaysResponse
    {
        [XmlElement("gateway")]
        public List<Gateway> Gateways { get; set; } 
    }
}
