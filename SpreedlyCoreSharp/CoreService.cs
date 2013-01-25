using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using SpreedlyCoreSharp.Domain;
using SpreedlyCoreSharp.Request;
using SpreedlyCoreSharp.Response;

namespace SpreedlyCoreSharp
{
    public class CoreService : ICoreService
    {
        private const string BaseUrl = "https://spreedlycore.com/v1/";
        private const string GatewaysUrl = "gateways.xml";
        private const string RedactGatewayUrl = "gateways/{0}/redact.xml";
        private const string ProcessPaymentUrl = "gateways/{0}/purchase.xml";
        private const string TransactionsUrl = "transactions.xml";
        private const string TransactionUrl = "transactions/{0}.xml";
        private const string TransactionTranscriptUrl = "transactions/{0}/transcript";

        private readonly HttpClient _client;

        private readonly string _apiLogin;
        private readonly string _apiSecret;
        private readonly string _gatewayToken;

        public string APILogin { get { return _apiLogin; } }
        public string APISecret { get { return _apiSecret; } }
        public string GatewayToken { get { return _gatewayToken; } }

        internal CoreService(HttpClient client, string apiLogin, string apiSecret)
        {
            _apiLogin = apiLogin;
            _apiSecret = apiSecret;
            _client = client;
        }

        public CoreService(string apiLogin, string apiSecret, string gatewayToken)
        {
            _apiLogin = apiLogin;
            _apiSecret = apiSecret;
            _gatewayToken = gatewayToken;

            _client = new HttpClient();
            _client.Request.SetBasicAuthentication(_apiLogin, _apiSecret);
            _client.Request.Accept = HttpContentTypes.ApplicationXml;
        }

        /// <summary>
        /// Turns an XML string into T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public T Deserialize<T>(string xml)
        {
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(xml));

            var serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(stream);
        }

        /// <summary>
        /// Turns T into an XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public string Serialize<T>(object item)
        {
            var serializer = new XmlSerializer(typeof(T));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var stream = new MemoryStream();

            serializer.Serialize(stream, item, ns);

            stream.Position = 0;

            return new StreamReader(stream).ReadToEnd();
        }

        public Gateway AddGateway(object gatewayRequest)
        {
            var response = _client.Post(BaseUrl + GatewaysUrl, gatewayRequest, "application/xml");

            byte[] byteArray = Encoding.ASCII.GetBytes(response.RawText);

            var stream = new MemoryStream(byteArray);

            return (Gateway)new XmlSerializer(typeof(Gateway)).Deserialize(stream);
        }

        public void RedactGateway(string gatewayToken)
        {
            // TODO: do something with response?
            _client.Put(BaseUrl + string.Format(RedactGatewayUrl, gatewayToken), null, "application/xml");
        }

        public List<Gateway> GetGateways()
        {
            var response = _client.Get(BaseUrl + GatewaysUrl);

            byte[] byteArray = Encoding.ASCII.GetBytes(response.RawText);

            var stream = new MemoryStream(byteArray);

            var responseObject = (GetGatewaysResponse)new XmlSerializer(typeof(GetGatewaysResponse)).Deserialize(stream);

            return responseObject.Gateways;
        }

        public Transaction GetTransaction(string token)
        {
            string url = BaseUrl + string.Format(TransactionUrl, token);

            var response = _client.Get(url);


            byte[] byteArray = Encoding.ASCII.GetBytes(response.RawText);

            var stream = new MemoryStream(byteArray);

            return (Transaction)new XmlSerializer(typeof(Transaction)).Deserialize(stream);
        }

        public List<Transaction> GetTransactions(string sinceToken = "")
        {
            string url;

            if (!string.IsNullOrWhiteSpace(sinceToken))
            {
                url = string.Format("{0}{1}?since_token={2}", BaseUrl, TransactionsUrl, sinceToken);
            }
            else
            {
                url = string.Format("{0}{1}", BaseUrl, TransactionsUrl);
            }

            var response = _client.Get(url);

            byte[] byteArray = Encoding.ASCII.GetBytes(response.RawText);

            var stream = new MemoryStream(byteArray);

            var responseObject = (GetTransactionsResponse)new XmlSerializer(typeof(GetTransactionsResponse)).Deserialize(stream);

            return responseObject.Transactions;
        } 

        public string GetTransactionTranscript(string token)
        {
            string url = BaseUrl + string.Format(TransactionTranscriptUrl, token);

            var response = _client.Get(url);

            return response.RawText;
        }

        public Transaction ProcessPayment(ProcessPaymentRequest request)
        {
            var response = _client.Post(BaseUrl + string.Format(ProcessPaymentUrl, _gatewayToken), request, "application/xml");

            if (request.Attempt3DSecure && string.IsNullOrWhiteSpace(request.CallbackUrl))
            {
                throw new ArgumentException("Callback URL cannot be empty.");
            }

            if (request.Attempt3DSecure && string.IsNullOrWhiteSpace(request.RedirectUrl))
            {
                throw new ArgumentException("Redirect URL cannot be empty");
            }

            byte[] byteArray = Encoding.ASCII.GetBytes(response.RawText);

            var stream = new MemoryStream(byteArray);

            return (Transaction)new XmlSerializer(typeof(Transaction)).Deserialize(stream);
        }
    }
}
