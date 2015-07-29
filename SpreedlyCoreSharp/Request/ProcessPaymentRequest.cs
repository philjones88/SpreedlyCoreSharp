using RestSharp.Serializers;
using SpreedlyCoreSharp.Domain;
using System;

namespace SpreedlyCoreSharp.Request
{
    [SerializeAs(Name = "transaction")]
    public class ProcessPaymentRequest
    {
        [SerializeAs(Name = "attempt_3dsecure")]
        public bool Attempt3DSecure { get; set; }

        [SerializeAs(Name = "amount")]
        public decimal Amount { get; set; }

        [SerializeAs(Name = "currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [SerializeAs(Name = "payment_method_token")]
        public string PaymentMethodToken { get; set; }

        [SerializeAs(Name = "redirect_url")]
        public string RedirectUrl { get; set; }

        [SerializeAs(Name = "callback_url")]
        public string CallbackUrl { get; set; }

        [SerializeAs(Name = "order_id")]
        public string OrderId { get; set; }

        [SerializeAs(Name = "description")]
        public string Description { get; set; }

        [SerializeAs(Name = "ip")]
        public string Ip { get; set; }
    }
}
