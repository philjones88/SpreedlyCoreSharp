using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Domain
{
    [XmlRoot("gateway")]
    public class Gateway
    {
        public class Characteristics
        {
            [XmlElement("supports_purchase")]
            public bool SupportsPurchase { get; set; }

            [XmlElement("supports_authorize")]
            public bool SupportsAuthorize { get; set; }

            [XmlElement("supports_capture")]
            public bool SupportsCapture { get; set; }

            [XmlElement("supports_credit")]
            public bool SupportsCredit { get; set; }

            [XmlElement("supports_void")]
            public bool SupportsVoid { get; set; }

            [XmlElement("supports_reference_purchase")]
            public bool SupportsReferencePurchase { get; set; }

            [XmlElement("supports_offsite_purchase")]
            public bool SupportsOffsitePurchase { get; set; }

            [XmlElement("supports_offsite_authorize")]
            public bool SupportsOffsiteAuthorize { get; set; }

            [XmlElement("supports_3dsecure_purchase")]
            public bool Supports3DSecurePurchase { get; set; }

            [XmlElement("supports_3dsecure_authorize")]
            public bool Supports3DSecureAuthorize { get; set; }
        }

        public Gateway()
        {
            GatewayCharacteristics = new Characteristics();
            PaymentMethods = new List<string>();
        }

        [XmlElement("token")]
        public string Token { get; set; }

        [XmlElement("gateway_type")]
        public string GatewayType { get; set; }

        [XmlElement("characteristics")]
        public Characteristics GatewayCharacteristics { get; set; }

        [XmlArray(ElementName = "payment_methods")]
        [XmlArrayItem(ElementName = "payment_method")]
        public List<string> PaymentMethods { get; set; }

        [XmlElement("redacted")]
        public bool Redacted { get; set; }

        [XmlElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [XmlElement("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
