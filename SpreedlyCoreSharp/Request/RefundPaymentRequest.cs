using SpreedlyCoreSharp.Domain;
using System;
using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Request
{
    [XmlRoot("transaction")]
    public class RefundPaymentRequest
    {
        [XmlElement("amount")]
        public decimal Amount { get; set; }

        [XmlIgnore]
        public decimal AmountInDecimal
        {
            get
            {
                if (Amount > 0)
                    return Amount / (decimal)100;

                return 0;
            }
            set
            {
                if (value > 0)
                    Amount = (int)(value * 100);
            }
        }

        [XmlElement("currency_code")]
        public CurrencyCode CurrencyCode { get; set; }
        
        [XmlElement("order_id")]
        public string OrderId { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("ip")]
        public string Ip { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        [XmlElement("merchant_name_descriptor")]
        public string MerchantNameDescriptor { get; set; }

        [XmlElement("merchant_location_descriptor")]
        public string MerchantLocationDescriptor { get; set; }
    }
}
