using System.Configuration;
using System.IO;
using System.Linq;
using Nancy;
using SpreedlyCoreSharp.Domain;
using SpreedlyCoreSharp.Html;
using SpreedlyCoreSharp.Request;

namespace SpreedlyCoreSharp.WebSample.Modules
{
    public class Payment3DSecureModule : NancyModule
    {
        public Payment3DSecureModule(ICoreService service)
            : base("/3d-secure")
        {
            Get["/"] = parameters =>
            {
                return View["TakePayment", new TransactionViewModel()
                {
                    ApiEnvironment = service.APIEnvironment,
                    RedirectUrl = ConfigurationManager.AppSettings["PublicWebUrl"] + "/3d-secure/redirect",
                    Country = "GB" // Default it to help your customers, it's a long list!
                }];
            };

            // First redirect from Core saying further processing
            // We then redirect or show the 3d secure form
            Get["/redirect"] = parameters =>
            {
                // You can fetch the payment method before charging the card to say check the card type
                var paymentMethod = service.GetPaymentMethod(Request.Query.token);

                var transaction = service.ProcessPayment(new ProcessPaymentRequest
                {
                    Attempt3DSecure = true,
                    Amount = 100,
                    CurrencyCode = CurrencyCode.GBP,
                    PaymentMethodToken = Request.Query.token,
                    RedirectUrl = ConfigurationManager.AppSettings["PublicWebUrl"] + "/3d-secure/redirect-after",
                    CallbackUrl = ConfigurationManager.AppSettings["PublicWebUrl"] + "/3d-secure/callback"
                });

                if (transaction.State == "pending" && !string.IsNullOrWhiteSpace(transaction.CheckoutUrl))
                {
                    return Response.AsRedirect(transaction.RedirectUrl);
                }

                if (transaction.State == "pending" && !string.IsNullOrWhiteSpace(transaction.CheckoutForm))
                {
                    return View["Transaction3DSecureForm", transaction.CheckoutForm];
                }

                if (transaction.Succeeded)
                {
                    return View["Success"];
                }

                var viewModel = new TransactionViewModel();

                viewModel.PopulateFromTransaction(transaction, ConfigurationManager.AppSettings["PublicWebUrl"] + "/3d-secure/redirect", service.APIEnvironment);

                return View["TakePayment", viewModel];
            };

            // Redirect after 3d secure
            Get["/redirect-after"] = parameters =>
            {
                Transaction transaction = service.GetTransaction(Request.Query.transaction_token);

                if (transaction.Succeeded)
                {
                    return View["Success"];
                }

                var viewModel = new TransactionViewModel();

                viewModel.PopulateFromTransaction(transaction, ConfigurationManager.AppSettings["PublicWebUrl"] + "/3d-secure/redirect", service.APIEnvironment);

                return View["TakePayment", viewModel];
            };

            // Callback after 3d secure, happens if something goes wrong/cancels also
            Post["/callback"] = parameters =>
            {
                var xmlBody = new StreamReader(Request.Body).ReadToEnd();

                var transactions = service.DeserializeTransactions(xmlBody).ToList();

                if (!transactions.Any())
                    return "Pingback";

                foreach (var transaction in transactions)
                {
                    if (service.ValidateTransactionSignature(transaction))
                    {
                        var token = transaction.Token;

                        // Do stuff in your database
                    }
                    else
                    {
                        var token = transaction.Token;

                        // Perhaps log we have an invalid transaction
                    }
                }

                return "Pingback";
            };
        }
    }
}