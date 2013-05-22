using NUnit.Framework;
using SpreedlyCoreSharp.Domain;
using System;

namespace SpreedlyCoreSharp.Test
{
    [TestFixture]
    public class DomainTests
    {
        private CoreService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new CoreService("", "", "", "");
        }

        [Test]
        public void CurrencyCode_SerializesCorrectly()
        {
            var expected = CurrencyCode.GBP;

            var actual = _service.Deserialize<CurrencyCode>("<CurrencyCode>GBP</CurrencyCode>");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CurrencyCode_SerializeExceptionWhenUnknown()
        {
            _service.Deserialize<CurrencyCode>("<CurrencyCode>PPP</CurrencyCode>");
        }
    }
}
