using System.Collections.Generic;
using System.Xml.Serialization;
using SpreedlyCoreSharp.Domain;

namespace SpreedlyCoreSharp.Response
{
    [XmlRoot("transactions")]
    public class GetTransactionsResponse
    {
        [XmlElement("transaction")]
        public List<Transaction> Transactions { get; set; } 
    }
}
