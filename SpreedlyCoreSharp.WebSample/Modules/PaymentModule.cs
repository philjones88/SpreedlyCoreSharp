using System.Configuration;
using Nancy;
using SpreedlyCoreSharp.Domain;
using SpreedlyCoreSharp.Html;
using SpreedlyCoreSharp.Request;

namespace SpreedlyCoreSharp.WebSample.Modules
{
    public class PaymentModule : NancyModule
    {
        public PaymentModule(ICoreService service) : base("/payment")
        {
            Get["/"] = _ =>
            {
                return View["Payments/TakePayment", new TransactionViewModel()
                {
                    ApiEnvironment = service.APIEnvironment,
                    RedirectUrl = ConfigurationManager.AppSettings["PublicWebUrl"] + "/payment/redirect-back"
                }];
            };

            Get["/redirect-back"] = _ =>
            {
                var transaction = service.ProcessPayment(new ProcessPaymentRequest
                {
                    AmountInDecimal = 1.0m, // Same as saying Amount = 100
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