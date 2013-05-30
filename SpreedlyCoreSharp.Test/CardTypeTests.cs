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
            var actual = _service.Deserialize<Transaction>("<?xml version='1.0' encoding='utf-8' ?>" +
                                                            "<transaction><payment_method>" +
                                                           "<card_type nill='true'></card_type>" +
                                                           "</payment_method></transaction>");

            Assert.AreEqual(CardType.None, actual.TransactionPaymentMethod.CardType);
        }

        [Test]
        public void CardType_Empty()
        {
            var actual = _service.Deserialize<Transaction>("<?xml version='1.0' encoding='utf-8' ?>" +
                                                    "<transaction><payment_method>" +
                                                   "<card_type></card_type>" +
                                                   "</payment_method></transaction>");

            Assert.AreEqual(CardType.None, actual.TransactionPaymentMethod.CardType);
        }

        // I would ideally like it to serialise unknown enum values to something like None/Unknown but it
        // Seems to be a limitation, perhaps this is best to assure we get correct data and not always None/Unknown
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CardType_Unknown()
        {
            var actual = _service.Deserialize<Transaction>("<?xml version='1.0' encoding='utf-8' ?>" +
                                                    "<transaction><payment_method>" +
                                                   "<card_type>Tesco</card_type>" +
                                                   "</payment_method></transaction>");
        }

        [Test]
        public void CardType_Visa()
        {
            var actual = _service.Deserialize<Transaction>("<?xml version='1.0' encoding='utf-8' ?>" +
                                                            "<transaction><payment_method>" +
                                                           "<card_type>visa</card_type>" +
                                                           "</payment_method></transaction>");

            Assert.AreEqual(CardType.Visa, actual.TransactionPaymentMethod.CardType);
        }

        [Test]
        public void CardType_MasterCard()
        {
            var actual = _service.Deserialize<Transaction>("<?xml version='1.0' encoding='utf-8' ?>" +
                                                            "<transaction><payment_method>" +
                                                           "<card_type>master</card_type>" +
                                                           "</payment_method></transaction>");

            Assert.AreEqual(CardType.MasterCard, actual.TransactionPaymentMethod.CardType);
        }

        [Test]
        public void CardType_AmericanExpress()
        {
            var actual = _service.Deserialize<Transaction>("<?xml version='1.0' encoding='utf-8' ?>" +
                                                "<transaction><payment_method>" +
                                               "<card_type>american_express</card_type>" +
                                               "</payment_method></transaction>");

            Assert.AreEqual(CardType.AmericanExpress, actual.TransactionPaymentMethod.CardType);
        }

        [Test]
        public void CardType_Discover()
        {
            var actual = _service.Deserialize<Transaction>("<?xml version='1.0' encoding='utf-8' ?>" +
                                                "<transaction><payment_method>" +
                                               "<card_type>discover</card_type>" +
                                               "</payment_method></transaction>");

            Assert.AreEqual(CardType.Discover, actual.TransactionPaymentMethod.CardType);
        }

        [Test]
        public void CardType_Dankort()
        {
            var actual = _service.Deserialize<Transaction>("<?xml version='1.0' encoding='utf-8' ?>" +
                                    "<transaction><payment_method>" +
                                   "<card_type>dankort</card_type>" +
                                   "</payment_method></transaction>");

            Assert.AreEqual(CardType.Dankort, actual.TransactionPaymentMethod.CardType);
        }
    }
}
