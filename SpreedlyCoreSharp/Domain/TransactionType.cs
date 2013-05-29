namespace SpreedlyCoreSharp.Domain
{
    public enum TransactionType
    {
        Unknown, // Added as default
        Purchase,
        PurchaseViaReference,
        OffsitePurchase,
        Credit,
        Authorization,
        Capture,
        AddPaymentMethod,
        RetainPaymentMethod,
        RedactPaymentMethod,
        Void,
        AddGateway,
        RedactGateway
    }
}
