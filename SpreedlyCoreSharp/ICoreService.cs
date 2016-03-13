﻿using System.Collections.Generic;
using SpreedlyCoreSharp.Domain;
using SpreedlyCoreSharp.Request;

namespace SpreedlyCoreSharp
{
    public interface ICoreService
    {
        string APIEnvironment { get; }
        string APISecret { get; }
        string APISigningSecret { get; }
        string GatewayToken { get; }

        /// <summary>
        /// Turns an XML array of Transactions into IEnumerable of Transactions
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        IEnumerable<Transaction> DeserializeTransactions(string xml);
            
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

        Gateway AddGateway(BaseGatewayRequest gatewayRequest);
        void RedactGateway(string gatewayToken);
        List<Gateway> GetGateways();
        Transaction GetTransaction(string token);
        List<Transaction> GetTransactions(string sinceToken = "");
        Transaction.PaymentMethod GetPaymentMethod(string token);
        List<Transaction.PaymentMethod> GetPaymentMethods(string sinceToken = "");
        string GetTransactionTranscript(string token);
        Transaction ProcessPayment(ProcessPaymentRequest request);
        bool ValidateTransactionSignature(Transaction transaction);
        bool ValidateTransactionSignature(string xml);
    }
}