using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using EasyHttp.Http;
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
                throw new ArgumentException("Redirect URL cannot be empty.");
            }

            byte[] byteArray = Encoding.ASCII.GetBytes(response.RawText);

            var stream = new MemoryStream(byteArray);

            // Seems if you send absolutely nothing it decides to return <errors> rather than full <transaction> doc...
            // Not sure how to append this to a Transaction document.
            if (response.RawText.StartsWith("<errors>"))
            {
                var errors = (TransactionErrors)new XmlSerializer(typeof(TransactionErrors)).Deserialize(stream);

                return new Transaction
                {
                    TransactionResponse = new Transaction.Response
                    {
                        Success = false
                    }
                };
            }

            return Deserialize<Transaction>(response.RawText);
        }

        /// <summary>
        /// Validates a transaction with a signature
        /// </summary>
        /// <param name="transactionXml"></param>
        /// <returns></returns>
        public bool ValidateTransactionSignature(string transactionXml)
        {
            // Translation of ruby code to C# from manual:
            // https://core.spreedly.com/manual/signing

            var doc = new XmlDocument();

            doc.LoadXml(transactionXml);

            // If xml is empty return false, something is wrong
            if (doc.DocumentElement == null)
            {
                return false;
            }

            var signedNode = doc.DocumentElement.SelectSingleNode("signed");

            // If xml doesn't have a signed element return false.
            // Undecided if this is final behaviour as might cause problems when
            // transactions don't have a signed xml node
            if (signedNode == null)
            {
                return false;
            }

            var signatureNode = signedNode.SelectSingleNode("signature");
            var signature = "";

            if (signatureNode != null)
            {
                signature = signatureNode.InnerText;
            }

            // Check we know what algorithm they are using, sample data indicated SHA1 only
            var algorithmNode = signedNode.SelectSingleNode("algorithm");
            var algorithm = "sha1";

            if (algorithmNode != null)
            {
                algorithm = algorithmNode.InnerText;
            }

            if (algorithm.Trim().ToUpper() != "SHA1")
            {
                throw new ArgumentException("Unknown transaction signature algorithm.");
            }

            var fieldsMushed = "";
            var signedFields = "";
            var signedFieldsNode = signedNode.SelectSingleNode("fields");

            if (signedFieldsNode != null)
            {
                signedFields = signedFieldsNode.InnerText;
            }

            foreach (var item in signedFields.Split(' '))
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                var node = doc.DocumentElement.SelectSingleNode(item);

                if (node != null)
                {
                    fieldsMushed += node.InnerText + "|";
                }
            }

            fieldsMushed = fieldsMushed.Substring(0, fieldsMushed.Length - 1);

            var myhmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes(APISecret));

            var byteArray = Encoding.ASCII.GetBytes(fieldsMushed);

            var stream = new MemoryStream(byteArray);

            var result = myhmacsha1.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);

            return result == signature;
        }
    }
}
