using System.Xml.Serialization;
using SpreedlyCoreSharp.Domain;

namespace SpreedlyCoreSharp.Request
{
    [XmlRoot("transaction")]
    public class ProcessPaymentRequest
    {
        [XmlElement("attempt_3dsecure")]
        public bool Attempt3DSecure { get; set; }

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

        [XmlElement("payment_method_token")]
        public string PaymentMethodToken { get; set; }

        [XmlElement("redirect_url")]
        public string RedirectUrl { get; set; }

        [XmlElement("callback_url")]
        public string CallbackUrl { get; set; }

        [XmlElement("order_id")]
        public string OrderId { get; set; }
    }
}
