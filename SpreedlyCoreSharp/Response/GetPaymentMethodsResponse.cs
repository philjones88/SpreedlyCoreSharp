using System.Collections.Generic;
using System.Xml.Serialization;
using SpreedlyCoreSharp.Domain;

namespace SpreedlyCoreSharp.Response
{
    [XmlRoot("payment_methods")]
    public class GetPaymentMethodsResponse
    {
        [XmlElement("payment_method")]
        public List<Transaction.PaymentMethod> PaymentMethods { get; set; }
    }
}
