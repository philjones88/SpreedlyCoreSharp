using System.Collections.Generic;
using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Domain
{
    [XmlRoot("errors")]
    public class TransactionErrors
    {
        [XmlType("error")]
        public class Error
        {
            [XmlAttribute("key")]
            public string Key { get; set; }

            [XmlText]
            public string Message { get; set; }
        }

        public TransactionErrors()
        {
            Errors = new List<Error>();
        }

        [XmlElement("error")]
        public List<Error> Errors { get; set; }
    }
}
