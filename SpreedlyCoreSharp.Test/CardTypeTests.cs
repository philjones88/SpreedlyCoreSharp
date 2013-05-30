using System;
using NUnit.Framework;
using SpreedlyCoreSharp.Domain;

namespace SpreedlyCoreSharp.Test
{
    [TestFixture]
    public class CardTypeTests
    {
        private CoreService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new CoreService("", "", "", "");
        }

        [Test]
        public void CardType_Null()
        {
            var actual = _service.Deserialize<CardType>("<CardType nill='true'></CardType>");

            Assert.AreEqual(CardType.None, actual);
        }

        [Test]
        public void CardType_Empty()
        {
            var actual = _service.Deserialize<CardType>("<CardType></CardType>");

            Assert.AreEqual(CardType.None, actual);
        }

        // I would ideally like it to serialise unknown enum values to something like None/Unknown but it
        // Seems to be a limitation, perhaps this is best to assure we get correct data and not always None/Unknown
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CardType_Unknown()
        {
            _service.Deserialize<CardType>("<CardType>Tesco</CardType>");
        }

        [Test]
        public void CardType_Visa()
        {
            var actual = _service.Deserialize<CardType>("<CardType>visa</CardType>");

            Assert.AreEqual(CardType.Visa, actual);
        }

        [Test]
        public void CardType_MasterCard()
        {
            var actual = _service.Deserialize<CardType>("<CardType>master</CardType>");

            Assert.AreEqual(CardType.MasterCard, actual);
        }

        [Test]
        public void CardType_AmericanExpress()
        {
            var actual = _service.Deserialize<CardType>("<CardType>american_express</CardType>");

            Assert.AreEqual(CardType.AmericanExpress, actual);
        }

        [Test]
        public void CardType_Discover()
        {
            var actual = _service.Deserialize<CardType>("<CardType>discover</CardType>");

            Assert.AreEqual(CardType.Discover, actual);
        }

        [Test]
        public void CardType_Dankort()
        {
            var actual = _service.Deserialize<CardType>("<CardType>dankort</CardType>");

            Assert.AreEqual(CardType.Dankort, actual);
        }
    }
}
