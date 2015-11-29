﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using SpreedlyCoreSharp.Domain;
using SpreedlyCoreSharp.Request;
using SpreedlyCoreSharp.Response;
using RestSharp;

namespace SpreedlyCoreSharp
{
    public class CoreService : ICoreService
    {
        private const string BaseUrl = "https://core.spreedly.com/v1/";
        private const string GatewaysUrl = "gateways.xml";
        private const string RedactGatewayUrl = "gateways/{0}/redact.xml";
        private const string ProcessPaymentUrl = "gateways/{0}/purchase.xml";
        private const string TransactionsUrl = "transactions.xml";
        private const string TransactionUrl = "transactions/{0}.xml";
        private const string PaymentMethodsUrl = "payment_methods.xml";
        private const string PaymentMethodUrl = "payment_methods/{0}.xml";
        private const string PaymentMethodRetainUrl = "payment_methods/{0}/retain.xml";
        private const string TransactionTranscriptUrl = "transactions/{0}/transcript";

        private readonly RestClient _client;

        private readonly string _apiEnvironment;
        private readonly string _apiSecret;
        private readonly string _apiSigningSecret;
        private readonly string _gatewayToken;

        public string APIEnvironment { get { return _apiEnvironment; } }
        public string APISecret { get { return _apiSecret; } }
        public string APISigningSecret { get { return _apiSigningSecret; } }
        public string GatewayToken { get { return _gatewayToken; } }

        public CoreService(string apiEnvironment, string apiSecret, string apiSigningSecret, string gatewayToken)
        {
            _apiEnvironment = apiEnvironment;
            _apiSecret = apiSecret;
            _apiSigningSecret = apiSigningSecret;
            _gatewayToken = gatewayToken;

            _client = new RestClient(BaseUrl);
            _client.Authenticator = new HttpBasicAuthenticator(_apiEnvironment, _apiSecret);
            _client.AddDefaultHeader("accept", "application/xml");
            _client.AddDefaultHeader("content-type", "application/xml");
        }

        /// <summary>
        /// Exposes a method to deserialize a group of transactions, used mainly in
        /// the 3D secure call back functionality
        /// </summary>
        /// <param name="xml">raw transactions xml</param>
        /// <returns></returns>
        public IEnumerable<Transaction> DeserializeTransactions(string xml)
        {
            var doc = new XmlDocument();

            doc.LoadXml(xml);

            if (doc.DocumentElement == null) yield break;

            var nodes = doc.DocumentElement.SelectNodes("transaction");

            if (nodes == null) yield break;

            foreach (XmlNode node in nodes)
            {
                yield return Deserialize<Transaction>(node.OuterXml);
            }
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

            var obj = (T)serializer.Deserialize(stream);

            if (typeof(T) == typeof(Transaction))
            {
                obj.GetType().GetProperty("RawTransactionXml").SetValue(obj, xml);
            }

            return obj;
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

        /// <summary>
        /// Adds a gateway
        /// </summary>
        /// <param name="gatewayRequest">gateway request object</param>
        /// <returns></returns>
        public Gateway AddGateway(BaseGatewayRequest gatewayRequest)
        {
            var request = new RequestWrapper(GatewaysUrl);
            request.AddBody(gatewayRequest);

            var response = _client.Post(request);

            return Deserialize<Gateway>(response.Content);
        }

        /// <summary>
        /// Redacts a gateway, this is permanent.
        /// </summary>
        /// <param name="gatewayToken">token of gateway</param>
        public void RedactGateway(string gatewayToken)
        {            
            // TODO: do something with response?
            _client.Put(new RequestWrapper(string.Format(RedactGatewayUrl, gatewayToken)));
        }

        /// <summary>
        /// Fetches a list of gateways
        /// </summary>
        /// <returns></returns>
        public List<Gateway> GetGateways()
        {
            var response = _client.Get(new RequestWrapper(GatewaysUrl));

            var gateways = Deserialize<GetGatewaysResponse>(response.Content);

            return gateways.Gateways;
        }

        /// <summary>
        /// Fetches a single transaction
        /// </summary>
        /// <param name="token">token of transaction</param>
        /// <returns></returns>
        public Transaction GetTransaction(string token)
        {
            var response = _client.Get(new RequestWrapper(string.Format(TransactionUrl, token)));

            return Deserialize<Transaction>(response.Content);
        }

        /// <summary>
        /// Fetches a list of transactions
        /// </summary>
        /// <param name="sinceToken">token of transaction to start from</param>
        /// <returns></returns>
        public List<Transaction> GetTransactions(string sinceToken = "")
        {
            var request = new RequestWrapper(TransactionsUrl);

            if (!string.IsNullOrWhiteSpace(sinceToken))
            {
                request.AddParameter("since_token", sinceToken);
            }

            var response = _client.Get(request);

            var transactions = Deserialize<GetTransactionsResponse>(response.Content);

            return transactions.Transactions;
        }

        /// <summary>
        /// Fetches a single payment method for a transaction token
        /// </summary>
        /// <param name="token">token of transaction</param>
        /// <returns>transaction's payment method</returns>
        public Transaction.PaymentMethod GetPaymentMethod(string token)
        {
            var response = _client.Get(new RequestWrapper(string.Format(PaymentMethodUrl, token)));

            return Deserialize<Transaction.PaymentMethod>(response.Content);
        }

        /// <summary>
        /// Fetches a list of payment methods
        /// </summary>
        /// <param name="sinceToken">token of transaction to start from</param>
        /// <returns></returns>
        public List<Transaction.PaymentMethod> GetPaymentMethods(string sinceToken = "")
        {
            var request = new RequestWrapper(PaymentMethodsUrl);

            if (!string.IsNullOrWhiteSpace(sinceToken))
            {
                request.AddParameter("since_token", sinceToken);
            }

            var response = _client.Get(request);

            var transactions = Deserialize<GetPaymentMethodsResponse>(response.Content);

            return transactions.PaymentMethods;
        }

        /// <summary>
        /// Fetches a transaction raw transaction
        /// This will be empty for test gateway transactions
        /// </summary>
        /// <param name="token">token of transaction</param>
        /// <returns></returns>
        public string GetTransactionTranscript(string token)
        {
            var response = _client.Get(new RequestWrapper(string.Format(TransactionTranscriptUrl, token)));

            return response.Content;
        }

        /// <summary>
        /// Sends a purchase request to the active gateway
        /// </summary>
        /// <param name="request">purchase request</param>
        /// <returns></returns>
        public Transaction ProcessPayment(ProcessPaymentRequest requestBody)
        {
            if (requestBody.Attempt3DSecure && string.IsNullOrWhiteSpace(requestBody.CallbackUrl))
            {
                throw new ArgumentException("Callback URL cannot be empty.");
            }

            if (requestBody.Attempt3DSecure && string.IsNullOrWhiteSpace(requestBody.RedirectUrl))
            {
                throw new ArgumentException("Redirect URL cannot be empty.");
            }

            var request = new RequestWrapper(string.Format(ProcessPaymentUrl, _gatewayToken));
            request.AddBody(requestBody);

            var response = _client.Post(request);

            byte[] byteArray = Encoding.ASCII.GetBytes(response.Content);

            var stream = new MemoryStream(byteArray);

            // Seems if you send absolutely nothing it decides to return <errors> rather than full <transaction> doc...
            // Not sure how to append this to a Transaction document.
            if (response.Content.StartsWith("<errors>"))
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

            return Deserialize<Transaction>(response.Content);
        }

        /// <summary>
        /// Validates a transaction with a signature
        /// </summary>
        /// <param name="transaction">Transaction</param>
        /// <returns></returns>
        public bool ValidateTransactionSignature(Transaction transaction)
        {
            return ValidateTransactionSignature(transaction.RawTransactionXml);
        }

        /// <summary>
        /// Validates a transaction with a signature
        /// </summary>
        /// <param name="transactionXml">Transaction xml</param>
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

            var myhmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes(APISigningSecret));

            var byteArray = Encoding.ASCII.GetBytes(fieldsMushed);

            var stream = new MemoryStream(byteArray);

            var result = myhmacsha1.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);

            return result == signature;
        }
    }
}
