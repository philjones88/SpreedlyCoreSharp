using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;

namespace SpreedlyCoreSharp.Test
{
    [TestFixture]
    public class EasyHttpTests
    {
        [XmlRoot("foos")]
        public class FooList
        {
            [XmlElement("foo")]
            public List<Foo> Foos { get; set; }
        }

        [XmlRoot("foo")]
        public class Foo
        {
            [XmlElement("name")]
            public string Name { get; set; }
        }

        [Test]
        public void Test1()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><foo><name>Bob</name></foo>";

            byte[] byteArray = Encoding.ASCII.GetBytes(xml);
            var stream = new MemoryStream(byteArray);

            var serializer = new XmlSerializer(typeof (Foo));
            var output = (Foo) serializer.Deserialize(stream);

            Assert.IsNotNull(output);
            Assert.AreEqual("Bob", output.Name);
        }

        [Test]
        public void Test2()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><foos><foo><name>Bob</name></foo><foo><name>John</name></foo></foos>";

            byte[] byteArray = Encoding.ASCII.GetBytes(xml);
            var stream = new MemoryStream(byteArray);

            var serializer = new XmlSerializer(typeof(FooList));
            var output = (FooList)serializer.Deserialize(stream);

            Assert.IsNotNull(output);
            Assert.AreEqual(2, output.Foos.Count);
            Assert.AreEqual("Bob", output.Foos[0].Name);
            Assert.AreEqual("John", output.Foos[1].Name);
        }
    }
}
