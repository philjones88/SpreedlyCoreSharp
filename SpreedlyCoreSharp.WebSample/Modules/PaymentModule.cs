using System.Configuration;
using Nancy;
using SpreedlyCoreSharp.Domain;
using SpreedlyCoreSharp.Html;
using SpreedlyCoreSharp.Request;

namespace SpreedlyCoreSharp.WebSample.Modules
{
    public class PaymentModule : NancyModule
    {
        public PaymentModule(ICoreService service)
            : base("/payment")
        {
            Get["/"] = _ =>
            {
                return View["Payments/TakePayment", new TransactionViewModel()
                {
                    ApiEnvironment = service.APIEnvironment,
                    RedirectUrl = ConfigurationManager.AppSettings["PublicWebUrl"] + "/payment/redirect-back",
                    Country = "GB" // Default it to help your customers, it's a long list!
                }];
            };

            Get["/redirect-back"] = _ =>
                {
                    // You can fetch the payment method before charging the card to say check the card type
                    var paymentMethod = service.GetPaymentMethod(Request.Query.token);

                    var transaction = service.ProcessPayment(new ProcessPaymentRequest
                    {
                        Amount = 100, // 1.00 GBP
                        CurrencyCode = CurrencyCode.GBP,
                        PaymentMethodToken = Request.Query.token
                    });

                    if (transaction.Succeeded)
                    {
                        return View["Payments/Success"];
                    }

                    var viewModel = new TransactionViewModel();

                    viewModel.PopulateFromTransaction(transaction, ConfigurationManager.AppSettings["PublicWebUrl"] + "/payment/redirect-back", service.APIEnvironment);

                    return View["Payments/TakePayment", viewModel];
                };
        }
    }
}