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
        Dankort,

        [XmlEnum("diners_club")]
        DinnersClub,

        [XmlEnum("jcb")]
        JCB,

        [XmlEnum("switch")]
        Switch,

        [XmlEnum("solo")]
        Solo,

        [XmlEnum("maestro")]
        Maestro,

        [XmlEnum("forbrugsforeningen")]
        Forbrugsforeningen,

        [XmlEnum("laser")]
        Laser
    }
}
