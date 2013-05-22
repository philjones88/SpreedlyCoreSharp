using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SpreedlyCoreSharp.Domain;
using SpreedlyCoreSharp.Request;
using SpreedlyCoreSharp.Response;

namespace SpreedlyCoreSharp.Test
{
    [TestFixture]
    public class CoreServiceTests
    {
        private CoreService _service;
        private string _sampleDataPath = Path.Combine(Environment.CurrentDirectory, "Xml");

        private string PathFor(string sampleFile)
        {
            return Path.Combine(_sampleDataPath, sampleFile);
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new CoreService("", "", "", "");
        }

        [Test]
        public void posted_form_errors_deserialization()
        {
            var xmlpath = PathFor("TransactionErrors1.xml");

            var actual = _service.Deserialize<TransactionErrors>(File.ReadAllText(xmlpath));

            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Errors.Count);
            Assert.AreEqual("Test", actual.Errors[0].Message);
            Assert.AreEqual("errors.one", actual.Errors[0].Key);
        }

        [Test]
        public void posted_form_multiple_errors_deserialization()
        {
            var xmlpath = PathFor("TransactionErrorsMultiple.xml");

            var actual = _service.Deserialize<TransactionErrors>(File.ReadAllText(xmlpath));

            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Errors.Count);
            Assert.AreEqual("Test", actual.Errors[0].Message);
            Assert.AreEqual("errors.one", actual.Errors[0].Key);
            Assert.AreEqual("Test", actual.Errors[1].Message);
            Assert.AreEqual("errors.two", actual.Errors[1].Key);
        }

        [Test]
        public void posted_form_failure_deserialization()
        {
            var xmlpath = PathFor("FailedFormPost.xml");

            var actual = _service.Deserialize<Transaction>(File.ReadAllText(xmlpath));

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.TransactionPaymentMethod);
            Assert.AreEqual(4, actual.TransactionPaymentMethod.Errors.Count);
        }

        [Test]
        public void GetTransactions_Multiple_Deserialization()
        {
            var xmlpath = PathFor("GetTransactions2.xml");

            var actual = _service.Deserialize<GetTransactionsResponse>(File.ReadAllText(xmlpath));

            Assert.IsNotNull(actual);
            Assert.AreEqual(20, actual.Transactions.Count);
        }

        [Test]
        public void GetTransactions_Deserialization_AddPaymentMethod()
        {
            var xmlpath = PathFor("GetTransactions.xml");

            var temp = _service.Deserialize<GetTransactionsResponse>(File.ReadAllText(xmlpath));

            Assert.IsNotNull(temp);
            Assert.AreEqual(3, temp.Transactions.Count);

            var transaction = temp.Transactions[0];

            Assert.AreEqual("transaction_1_token", transaction.Token);
            Assert.AreEqual(true, transaction.Succeeded);
            Assert.AreEqual(TransactionType.AddPaymentMethod, transaction.TransactionType);
            Assert.AreEqual("Succeeded!", transaction.Message);
            Assert.AreEqual(new DateTime(2012, 11, 16, 20, 36, 03), transaction.CreatedAt);
            Assert.AreEqual(new DateTime(2012, 11, 16, 20, 36, 03), transaction.UpdatedAt);
            Assert.IsNotNull(transaction.TransactionPaymentMethod);

            var paymentMethod = transaction.TransactionPaymentMethod;

            Assert.AreEqual("transaction1_payment_token", paymentMethod.Token);
            Assert.AreEqual(new DateTime(2012, 11, 16, 20, 36, 03), paymentMethod.CreatedAt);
            Assert.AreEqual(new DateTime(2012, 11, 16, 20, 36, 04), paymentMethod.UpdatedAt);
            Assert.AreEqual(3886, paymentMethod.LastFourDigits);
            Assert.AreEqual("visa", paymentMethod.CardType);
            Assert.AreEqual("Bob", paymentMethod.FirstName);
            Assert.AreEqual("Smith", paymentMethod.LastName);
            Assert.AreEqual(2020, paymentMethod.Year);
            Assert.AreEqual(1, paymentMethod.Month);
            Assert.AreEqual("credit_card", paymentMethod.PaymentMethodType);
            Assert.AreEqual("XXXX-XXXX-XXXX-3886", paymentMethod.Number);
        }

        [Test]
        public void GetTransactions_Deserialization_Purchase()
        {
            var xmlpath = PathFor("Transaction_Purchase.xml");

            var transaction = _service.Deserialize<Transaction>(File.ReadAllText(xmlpath));

            Assert.IsNotNull(transaction);
            Assert.AreEqual(100, transaction.Amount);
            Assert.AreEqual(true, transaction.OnTestGateway);
            Assert.AreEqual(new DateTime(2012, 11, 26, 21, 34, 19), transaction.CreatedAt);
            Assert.AreEqual(new DateTime(2012, 11, 26, 21, 34, 19), transaction.UpdatedAt);
            Assert.AreEqual(CurrencyCode.USD, transaction.CurrencyCode);
            Assert.AreEqual(true, transaction.Succeeded);
            Assert.AreEqual("S8ttcpdkNNBGM6eK97ZXb3iOkT2", transaction.Token);
            Assert.AreEqual(TransactionType.Purchase, transaction.TransactionType);
            Assert.AreEqual("Succeeded!", transaction.Message);
            Assert.AreEqual("8QXrcrIU0Rc1pBYawu9fJsuEEgk", transaction.GatewayToken);

            Assert.IsNotNull(transaction.TransactionResponse);
            Assert.AreEqual(true, transaction.TransactionResponse.Success);
            Assert.AreEqual("Successful purchase!", transaction.TransactionResponse.Message);
            Assert.AreEqual(new DateTime(2012, 11, 26, 21, 34, 19), transaction.TransactionResponse.CreatedAt);
            Assert.AreEqual(new DateTime(2012, 11, 26, 21, 34, 19), transaction.TransactionResponse.UpdatedAt);

            Assert.IsNotNull(transaction.TransactionPaymentMethod);
            Assert.AreEqual("PJH85K9VZ2iTgqPr2yoafwjEewG", transaction.TransactionPaymentMethod.Token);
            Assert.AreEqual(new DateTime(2012, 11, 09, 14, 05, 56), transaction.TransactionPaymentMethod.CreatedAt);
            Assert.AreEqual(new DateTime(2012, 11, 26, 21, 34, 19), transaction.TransactionPaymentMethod.UpdatedAt);
            Assert.AreEqual(1111, transaction.TransactionPaymentMethod.LastFourDigits);
            Assert.AreEqual("visa", transaction.TransactionPaymentMethod.CardType);
            Assert.AreEqual("Juvenal", transaction.TransactionPaymentMethod.FirstName);
            Assert.AreEqual("Volkman", transaction.TransactionPaymentMethod.LastName);
            Assert.AreEqual(2020, transaction.TransactionPaymentMethod.Year);
            Assert.AreEqual(4, transaction.TransactionPaymentMethod.Month);
            Assert.AreEqual("credit_card", transaction.TransactionPaymentMethod.PaymentMethodType);
            Assert.AreEqual("XXXX-XXXX-XXXX-1111", transaction.TransactionPaymentMethod.Number);
        }

        [Test]
        public void GetTransactions_3DSecure_Transaction()
        {
            var xmlpath = PathFor("3DSecureTransaction.xml");

            var transaction = _service.Deserialize<Transaction>(File.ReadAllText(xmlpath));

            Assert.IsNotNull(transaction);
        }

        [Test]
        public void GetGateways_Deserialization()
        {
            var xmlpath = PathFor("GetGateways.xml");

            var temp = _service.Deserialize<GetGatewaysResponse>(File.ReadAllText(xmlpath));
            var output = temp.Gateways;

            Assert.IsNotNull(output);
            Assert.AreEqual(1, output.Count);

            var gateway = output[0];

            Assert.AreEqual("token_goes_here", gateway.Token);
            Assert.AreEqual("test", gateway.GatewayType);
            Assert.AreEqual(new DateTime(2012, 11, 16, 21, 17, 02), gateway.CreatedAt);
            Assert.AreEqual(new DateTime(2012, 11, 16, 21, 17, 02), gateway.UpdatedAt);
            Assert.AreEqual(2, gateway.PaymentMethods.Count);
            Assert.AreEqual("credit_card", gateway.PaymentMethods[0]);
            Assert.AreEqual("sprel", gateway.PaymentMethods[1]);
            Assert.AreEqual(true, gateway.GatewayCharacteristics.Supports3DSecureAuthorize);
            Assert.AreEqual(true, gateway.GatewayCharacteristics.Supports3DSecurePurchase);
            Assert.AreEqual(true, gateway.GatewayCharacteristics.SupportsAuthorize);
            Assert.AreEqual(true, gateway.GatewayCharacteristics.SupportsCapture);
            Assert.AreEqual(true, gateway.GatewayCharacteristics.SupportsCredit);
            Assert.AreEqual(true, gateway.GatewayCharacteristics.SupportsOffsiteAuthorize);
            Assert.AreEqual(true, gateway.GatewayCharacteristics.SupportsOffsitePurchase);
            Assert.AreEqual(true, gateway.GatewayCharacteristics.SupportsPurchase);
            Assert.AreEqual(true, gateway.GatewayCharacteristics.SupportsReferencePurchase);
            Assert.AreEqual(true, gateway.GatewayCharacteristics.SupportsVoid);
        }

        [Test]
        public void GetGateway_Serialization()
        {
            var expected = new GetGatewaysResponse
            {
                Gateways = new List<Gateway>
                    {
                        new Gateway
                            {
                                Token = "token_goes_here",
                                GatewayType = "test",
                                CreatedAt = new DateTime(2012, 11, 16, 21, 17, 02),
                                UpdatedAt = new DateTime(2012, 11, 16, 21, 17, 02),
                                PaymentMethods = new List<string>
                                    {
                                        "credit_card",
                                        "sprel"
                                    },
                                GatewayCharacteristics = new Gateway.Characteristics
                                    {
                                        Supports3DSecureAuthorize = true,
                                        Supports3DSecurePurchase = true,
                                        SupportsAuthorize = true,
                                        SupportsCapture = true,
                                        SupportsCredit = true,
                                        SupportsOffsiteAuthorize = true,
                                        SupportsOffsitePurchase = true,
                                        SupportsPurchase = true,
                                        SupportsReferencePurchase = true,
                                        SupportsVoid = true
                                    }
                            }
                    }
            };

            var xml = _service.Serialize<GetGatewaysResponse>(expected);

            var temp = _service.Deserialize<GetGatewaysResponse>(xml);
            var actual = temp.Gateways[0];


            Assert.AreEqual("token_goes_here", actual.Token);
            Assert.AreEqual("test", actual.GatewayType);
            Assert.AreEqual(new DateTime(2012, 11, 16, 21, 17, 02), actual.CreatedAt);
            Assert.AreEqual(new DateTime(2012, 11, 16, 21, 17, 02), actual.UpdatedAt);
            Assert.AreEqual(2, actual.PaymentMethods.Count);
            Assert.AreEqual("credit_card", actual.PaymentMethods[0]);
            Assert.AreEqual("sprel", actual.PaymentMethods[1]);
            Assert.AreEqual(true, actual.GatewayCharacteristics.Supports3DSecureAuthorize);
            Assert.AreEqual(true, actual.GatewayCharacteristics.Supports3DSecurePurchase);
            Assert.AreEqual(true, actual.GatewayCharacteristics.SupportsAuthorize);
            Assert.AreEqual(true, actual.GatewayCharacteristics.SupportsCapture);
            Assert.AreEqual(true, actual.GatewayCharacteristics.SupportsCredit);
            Assert.AreEqual(true, actual.GatewayCharacteristics.SupportsOffsiteAuthorize);
            Assert.AreEqual(true, actual.GatewayCharacteristics.SupportsOffsitePurchase);
            Assert.AreEqual(true, actual.GatewayCharacteristics.SupportsPurchase);
            Assert.AreEqual(true, actual.GatewayCharacteristics.SupportsReferencePurchase);
            Assert.AreEqual(true, actual.GatewayCharacteristics.SupportsVoid);
        }

        [Test]
        public void ProcessPayment_Serialization()
        {
            var expected = new ProcessPaymentRequest
            {
                Amount = 100,
                CurrencyCode = CurrencyCode.USD,
                PaymentMethodToken = "Prxf2Ohv40L5j6SdTytuHCklwyF"
            };

            var xml = _service.Serialize<ProcessPaymentRequest>(expected);

            var actual = _service.Deserialize<ProcessPaymentRequest>(xml);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Amount, actual.Amount);
            Assert.AreEqual(expected.CurrencyCode, actual.CurrencyCode);
            Assert.AreEqual(expected.PaymentMethodToken, actual.PaymentMethodToken);
        }

        [Test]
        public void ProcessPayment_Deserialization()
        {
            var xmlpath = PathFor("ProcessPaymentRequest.xml");

            var actual = _service.Deserialize<ProcessPaymentRequest>(File.ReadAllText(xmlpath));

            Assert.IsNotNull(actual);

            Assert.AreEqual(100, actual.Amount);
            Assert.AreEqual(CurrencyCode.USD, actual.CurrencyCode);
            Assert.AreEqual("Prxf2Ohv40L5j6SdTytuHCklwyF", actual.PaymentMethodToken);
        }

        [Test]
        public void ProcessPayment_3DSecure_Serialization()
        {
            var expected = new ProcessPaymentRequest
                {
                    Attempt3DSecure = true,
                    Amount = 100,
                    CurrencyCode = CurrencyCode.USD,
                    PaymentMethodToken = "payment_method_token",
                    RedirectUrl = "http://example.com/handle_redirect",
                    CallbackUrl = "http://example.com/handle_callback"
                };

            var xml = _service.Serialize<ProcessPaymentRequest>(expected);

            var actual = _service.Deserialize<ProcessPaymentRequest>(xml);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Attempt3DSecure, actual.Attempt3DSecure);
            Assert.AreEqual(expected.Amount, actual.Amount);
            Assert.AreEqual(expected.PaymentMethodToken, actual.PaymentMethodToken);
            Assert.AreEqual(expected.RedirectUrl, actual.RedirectUrl);
            Assert.AreEqual(expected.CallbackUrl, actual.CallbackUrl);
        }

        [Test]
        public void ProcessPayment_3DSecure_Deserialization()
        {
            var xmlpath = PathFor("ProcessPaymentRequest3DSecure.xml");

            var actual = _service.Deserialize<ProcessPaymentRequest>(File.ReadAllText(xmlpath));

            Assert.IsNotNull(actual);
            Assert.AreEqual(true, actual.Attempt3DSecure);
            Assert.AreEqual(100, actual.Amount);
            Assert.AreEqual(CurrencyCode.USD, actual.CurrencyCode);
            Assert.AreEqual("payment_method_token", actual.PaymentMethodToken);
            Assert.AreEqual("http://example.com/handle_redirect", actual.RedirectUrl);
            Assert.AreEqual("http://example.com/handle_callback", actual.CallbackUrl);
        }

        [Test]
        public void SignedTransaction_Deserialization()
        {
            var xmlpath = PathFor("SignedTransaction.xml");

            var actual = _service.Deserialize<Transaction>(File.ReadAllText(xmlpath));

            Assert.IsNotNull(actual);
            Assert.AreEqual("b81436daf0d695404c5bf7a2aecf049d460bb6e1", actual.Signed.Signature);
            Assert.AreEqual("amount callback_url created_at currency_code ip on_test_gateway order_id state succeeded token transaction_type updated_at", actual.Signed.RawFields);
            Assert.AreEqual(new List<string> { "amount", "callback_url", "created_at", "currency_code", "ip", "on_test_gateway", "order_id", "state", "succeeded", "token", "transaction_type", "updated_at" }, actual.Signed.Fields);
            Assert.AreEqual("sha1", actual.Signed.Algorithm);
        }

        [Test]
        public void SignedTransactions_Deserialization()
        {
            var xmlpath = PathFor("SignedTransactions.xml");

            var actual = _service.DeserializeTransactions(File.ReadAllText(xmlpath)).ToList();

            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(100, actual[0].Amount);
            Assert.IsNotNullOrEmpty(actual[0].RawTransactionXml);
            Assert.AreEqual(100, actual[1].Amount);
            Assert.IsNotNullOrEmpty(actual[1].RawTransactionXml);
        }

        [Test]
        public void SignedTransaction_Xml()
        {
            var transactionXml = File.ReadAllText(PathFor("SignedTransaction.xml"));

            _service = new CoreService("", "", "RKOCG5D8D3fZxDSg504D0IxU2XD4Io5VXmyzdCtTivHFTTSylzM2ZzTWFwVH4ucG", "");

            var result = _service.ValidateTransactionSignature(transactionXml);

            Assert.IsTrue(result);
        }

        [Test]
        public void SignedTransaction_Transaction()
        {
            var xmlpath = PathFor("SignedTransaction.xml");

            _service = new CoreService("", "", "RKOCG5D8D3fZxDSg504D0IxU2XD4Io5VXmyzdCtTivHFTTSylzM2ZzTWFwVH4ucG", "");

            var transaction = _service.Deserialize<Transaction>(File.ReadAllText(xmlpath));

            var actual = _service.ValidateTransactionSignature(transaction);

            Assert.IsTrue(actual);
        }

        [Test]
        public void SignedTransactions_Transaction()
        {
            var xmlpath = PathFor("SignedTransactions.xml");

            _service = new CoreService("", "", "RKOCG5D8D3fZxDSg504D0IxU2XD4Io5VXmyzdCtTivHFTTSylzM2ZzTWFwVH4ucG", "");

            var transactions = _service.DeserializeTransactions(File.ReadAllText(xmlpath));

            foreach (var transaction in transactions)
            {
                var actual = _service.ValidateTransactionSignature(transaction);

                Assert.IsTrue(actual);
            }
        }
    }
}
