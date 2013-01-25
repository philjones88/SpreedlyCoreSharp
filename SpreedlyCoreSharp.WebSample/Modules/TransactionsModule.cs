using Nancy;

namespace SpreedlyCoreSharp.WebSample.Modules
{
    public class TransactionsModule : NancyModule
    {
        public TransactionsModule(ICoreService service) : base ("/transactions")
        {
            Get["/"] = parameters =>
            {
                if (Request.Query.since)
                {
                    return View["Transactions", service.GetTransactions(Request.Query.since)];
                }

                return View["Transactions", service.GetTransactions()];
            };

            Get["/{token}"] = parameters =>
            {
                var transaction = service.GetTransaction(parameters.token);

                return View["Transaction", transaction];
            };

            Get["/{token}/transcript"] = parameters =>
            {
                var transcript = service.GetTransactionTranscript(parameters.token);

                return View["TransactionTranscript", transcript];
            };
        }
    }
}