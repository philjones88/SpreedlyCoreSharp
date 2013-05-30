using System.Xml.Serialization;

namespace SpreedlyCoreSharp.Domain
{
    public enum CardType
    {
        [XmlEnum("")]
        None = 0,

        [XmlEnum("visa")]
        Visa,

        [XmlEnum("master")]
        MasterCard,

        [XmlEnum("american_express")]
        AmericanExpress,

        [XmlEnum("discover")]
        Discover,

        [XmlEnum("dankort")]
        Dankort
    }
}
