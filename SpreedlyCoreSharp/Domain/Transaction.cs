using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Domain
{
    [XmlRoot("transaction")]
    public class Transaction
    {
        public class PaymentMethod
        {
            public class Error
            {
                [XmlAttribute("attribute")]
                public string Field { get; set; }

                [XmlAttribute("key")]
                public string Key { get; set; }

                [XmlText]
                public string Message { get; set; }
            }

            [XmlElement("token")]
            public string Token { get; set; }

            [XmlElement("created_at")]
            public DateTime CreatedAt { get; set; }

            [XmlElement("updated_at")]
            public DateTime UpdatedAt { get; set; }

            [XmlElement("payment_method_type")]
            public string PaymentMethodType { get; set; }

            [XmlElement("last_four_digits")]
            public string LastFourDigits { get; set; }

            [XmlElement("number")]
            public string Number { get; set; }

            [XmlElement("verification_value")]
            public string VerificationValue { get; set; }

            [XmlElement("card_type")]
            [DefaultValue(CardType.None)]
            public CardType CardType { get; set; }

            [XmlElement("first_name")]
            public string FirstName { get; set; }

            [XmlElement("last_name")]
            public string LastName { get; set; }

            [XmlElement("month")]
            [DefaultValue(0)]
            public int Month { get; set; }

            [XmlElement("year")]
            [DefaultValue(0)]
            public int Year { get; set; }

            [XmlElement("email")]
            public string Email { get; set; }

            [XmlElement("address1")]
            public string Address1 { get; set; }

            [XmlElement("address2")]
            public string Address2 { get; set; }

            [XmlElement("city")]
            public string City { get; set; }

            [XmlElement("state")]
            public string State { get; set; }

            [XmlElement("zip")]
            public string Zip { get; set; }

            [XmlElement("country")]
            public string Country { get; set; }

            [XmlElement("phone_number")]
            public string PhoneNumber { get; set; }

            [XmlArray("errors")]
            [XmlArrayItem("error")]
            public List<Error> Errors { get; set; }

            public object Data { get; set; }
        }

        public class Response
        {
            [XmlElement("success")]
            public bool Success { get; set; }

            [XmlElement("message")]
            public string Message { get; set; }

            [XmlElement("avs_code")]
            public string AvsCode { get; set; }

            [XmlElement("avs_message")]
            public string AvsMessage { get; set; }

            [XmlElement("cvv_code")]
            public string CvvCode { get; set; }

            [XmlElement("ccv_message")]
            public string CvvMessage { get; set; }

            [XmlElement("error_code")]
            public string ErrorCode { get; set; }

            [XmlElement("error_details")]
            public string ErrorDetail { get; set; }

            [XmlElement("created_at")]
            public DateTime CreatedAt { get; set; }

            [XmlElement("updated_at")]
            public DateTime UpdatedAt { get; set; }
        }

        public class SetupResponse
        {
            [XmlElement("success")]
            public bool Success { get; set; }

            [XmlElement("message")]
            public string Message { get; set; }

            [XmlElement("error_code")]
            public string ErrorCode { get; set; }

            [XmlElement("checkout_url")]
            public string CheckoutUrl { get; set; }

            [XmlElement("created_at")]
            public DateTime CreatedAt { get; set; }

            [XmlElement("updated_at")]
            public DateTime UpdatedAt { get; set; }
        }

        public class RedirectResponse
        {
            [XmlElement("success")]
            public bool Success { get; set; }

            [XmlElement("message")]
            public string Message { get; set; }

            [XmlElement("error_code")]
            public string ErrorCode { get; set; }

            [XmlElement("error_detail")]
            public string ErrorDetail { get; set; }

            [XmlElement("cancelled")]
            public bool Cancelled { get; set; }

            [XmlElement("created_at")]
            public DateTime CreatedAt { get; set; }

            [XmlElement("updated_at")]
            public DateTime UpdatedAt { get; set; }
        }

        public class OffsiteResponse
        {
            [XmlElement("success")]
            public bool Success { get; set; }

            [XmlElement("message")]
            public string Message { get; set; }

            [XmlElement("error_code")]
            public string ErrorCode { get; set; }

            [XmlElement("error_detail")]
            public string ErrorDetail { get; set; }

            [XmlElement("cancelled")]
            public bool Cancelled { get; set; }

            [XmlElement("created_at")]
            public DateTime CreatedAt { get; set; }

            [XmlElement("updated_at")]
            public DateTime UpdatedAt { get; set; }
        }

        public class SignedResponse
        {
            [XmlElement("signature")]
            public string Signature { get; set; }

            [XmlElement("fields")]
            public string RawFields { get; set; }

            public List<string> Fields
            {
                get
                {
                    return string.IsNullOrWhiteSpace(RawFields) ? new List<string>() : RawFields.Split(' ').ToList();
                }
            }

            [XmlElement("algorithm")]
            public string Algorithm { get; set; }
        }

        [XmlElement("token")]
        public string Token { get; set; }

        [XmlElement("gateway_token")]
        public string GatewayToken { get; set; }

        /// <summary>
        /// Amount to charge in cents, pence etc. $83.45 means 8345
        /// </summary>
        [XmlElement("amount")]
        public int Amount { get; set; }

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
                    Amount = (int) (value * 100);
            }
        }

        [XmlElement("currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [XmlElement("state")]
        public string State { get; set; }

        [XmlElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [XmlElement("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [XmlElement("succeeded")]
        public bool Succeeded { get; set; }

        [XmlElement("on_test_gateway")]
        public bool OnTestGateway { get; set; }

        [XmlElement("transaction_type")]
        public TransactionType TransactionType { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }

        [XmlElement("callback_url")]
        public string CallbackUrl { get; set; }

        [XmlElement("redirect_url")]
        public string RedirectUrl { get; set; }

        [XmlElement("checkout_url")]
        public string CheckoutUrl { get; set; }

        [XmlElement("checkout_form")]
        public string CheckoutForm { get; set; }

        [XmlElement("response")]
        public Response TransactionResponse { get; set; }

        [XmlElement("payment_method")]
        public PaymentMethod TransactionPaymentMethod { get; set; }

        [XmlElement("setup_response")]
        public SetupResponse TransactionSetupResponse { get; set; }

        [XmlElement("redirect_response")]
        public RedirectResponse TransactionRedirectResponse { get; set; }

        [XmlElement("signed")]
        public SignedResponse Signed { get; set; }

        [XmlArray(ElementName = "api_urls")]
        [XmlArrayItem(ElementName = "callback_conversations")]
        public List<string> ApiUrls { get; set; }

        // These seem to be null in all the samples?
        [XmlElement("order_id")]
        public string OrderId { get; set; }

        [XmlElement("ip")]
        public string Ip { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// The raw XML for this Transaction
        /// </summary>
        public string RawTransactionXml { get; set; }
    }
}
