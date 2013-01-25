using System.Collections.Generic;
using System.Linq;
using SpreedlyCoreSharp.Domain;

namespace SpreedlyCoreSharp.Html
{
    public class TransactionViewModel
    {
        // To save having to install AutoMapper
        public void PopulateFromTransaction(Transaction transaction, string redirectUrl, string apiLogin)
        {
            if (transaction == null || transaction.TransactionPaymentMethod == null)
                return;

            ApiLogin = apiLogin;
            RedirectUrl = redirectUrl;
            PaymentMethodToken = transaction.TransactionPaymentMethod.Token;

            FirstName = transaction.TransactionPaymentMethod.FirstName;
            LastName = transaction.TransactionPaymentMethod.LastName;
            Email = transaction.TransactionPaymentMethod.Email;
            PhoneNumber = transaction.TransactionPaymentMethod.PhoneNumber;
            Address1 = transaction.TransactionPaymentMethod.Address1;
            Address2 = transaction.TransactionPaymentMethod.Address2;
            City = transaction.TransactionPaymentMethod.City;
            State = transaction.TransactionPaymentMethod.State;
            Zip = transaction.TransactionPaymentMethod.Zip;
            Country = transaction.TransactionPaymentMethod.Country;
            Number = transaction.TransactionPaymentMethod.Number;
            VerificationValue = transaction.TransactionPaymentMethod.VerificationValue;
            Month = transaction.TransactionPaymentMethod.Month;
            Year = transaction.TransactionPaymentMethod.Year;
            Errors = transaction.TransactionPaymentMethod.Errors;
        }

        private string CleanFieldName(string name)
        {
            if (name.StartsWith("credit_card[") && name.EndsWith("]"))
            {
                name = name.Replace("credit_card[", "");
                name = name.Substring(0, name.Length - 1);
            }

            return name;
        }

        public bool IsErrorFor(string fieldName)
        {
            if (Errors == null || !Errors.Any())
                return false;

            var cleanName = CleanFieldName(fieldName);

            if (Errors.Any(x => x.Field == cleanName))
                return true;

            return false;
        }

        public string CssErrorFor(string fieldName, string cssClass = "error")
        {
            if (IsErrorFor(fieldName))
            {
                return cssClass;
            }

            return "";
        }

        public string ErrorFor(string fieldName)
        {
            if (Errors == null || !Errors.Any())
                return "";

            var cleanName = CleanFieldName(fieldName);

            if (Errors.Any(x => x.Field == cleanName))
                return Errors.FirstOrDefault(x => x.Field == cleanName).Message;

            return "";
        }

        public string _FirstName = "credit_card[first_name]";
        public string _LastName = "credit_card[last_name]";
        public string _Month = "credit_card[month]";
        public string _Year = "credit_card[year]";
        public string _Number = "credit_card[number]";
        public string _VerificationValue = "credit_card[verification_value]";
        public string _Email = "credit_card[email]";
        public string _Address1 = "credit_card[address1]";
        public string _Address2 = "credit_card[address2]";
        public string _City = "credit_card[city]";
        public string _State = "credit_card[state]";
        public string _Zip = "credit_card[zip]";
        public string _Country = "credit_card[country]";
        public string _PhoneNumber = "credit_card[phone_number]";

        public string _RedirectUrl = "redirect_url";
        public string _ApiLogin = "api_login";
        public string _PaymentMethodToken = "payment_method_token";

        public string RedirectUrl { get; set; }

        public string ApiLogin { get; set; }

        public string PaymentMethodToken { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Number { get; set; }

        public string VerificationValue { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Email { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }
        
        public string PhoneNumber { get; set; }

        public List<Transaction.PaymentMethod.Error> Errors { get; set; }
    }
}
