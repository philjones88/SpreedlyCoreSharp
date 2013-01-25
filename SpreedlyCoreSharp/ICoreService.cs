using System.Collections.Generic;
using SpreedlyCoreSharp.Domain;
using SpreedlyCoreSharp.Request;

namespace SpreedlyCoreSharp
{
    public interface ICoreService
    {
        string APILogin { get; }
        string APISecret { get; }
        string GatewayToken { get; }

        /// <summary>
        /// Turns an XML string into T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        T Deserialize<T>(string xml);

        /// <summary>
        /// Turns T into an XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        string Serialize<T>(object item);

        Gateway AddGateway(object gatewayRequest);
        void RedactGateway(string gatewayToken);
        List<Gateway> GetGateways();
        Transaction GetTransaction(string token);
        List<Transaction> GetTransactions(string sinceToken = "");
        string GetTransactionTranscript(string token);
        Transaction ProcessPayment(ProcessPaymentRequest request);
    }
}