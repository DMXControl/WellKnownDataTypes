using org.dmxc.wkdt.Network;
using System.Collections.Concurrent;

namespace org.dmxc.wkdt.Tests.Network
{
    public class IPv4Address_Tests
    {
        [Test]
        public void TestIPv4Address()
        {
            var address = IPv4Address.LocalHost;
            Assert.Multiple(() =>
            {
                Assert.That(IPv4Address.Empty.ToString(), Is.EqualTo("0.0.0.0"));
                Assert.That(address.ToString(), Is.EqualTo("127.0.0.1"));
                Assert.That(address, Is.EqualTo(new IPv4Address(address.ToString())));
                Assert.Throws(typeof(FormatException), () => new IPv4Address("1.2.3."));

                address = new IPv4Address(1, 1, 1, 1);

                Assert.That(address, Is.EqualTo(new IPv4Address(new byte[] { 1, 1, 1, 1 })));
                Assert.Throws(typeof(ArgumentOutOfRangeException), () => new IPv4Address(new byte[] { 1 }));
                Assert.That(address, Is.EqualTo(new IPv4Address(new List<byte>() { 1, 1, 1, 1 })));
                Assert.Throws(typeof(ArgumentOutOfRangeException), () => new IPv4Address(new List<byte>() { 1 }));


                byte[] bytes = new byte[] { 8, 7, 6, 5 };
                System.Net.IPAddress ip = new System.Net.IPAddress(bytes);
                address = (IPv4Address)bytes;
                Assert.That(ip, Is.EqualTo((System.Net.IPAddress)address));
                Assert.That(address, Is.EqualTo((IPv4Address)ip));

                bytes = new byte[] { 1, 1, 1, 1 };
                Assert.That(bytes, Is.EqualTo((byte[])new IPv4Address(bytes)));

                Assert.That(new IPv4Address(3, 4, 5, 6) == new IPv4Address("3.4.5.6"), Is.True);
                Assert.That(new IPv4Address(3, 4, 5, 6) != new IPv4Address("3.4.5.6"), Is.False);
                Assert.That(((object)new IPv4Address(3, 4, 5, 6)).Equals(new IPv4Address("3.4.5.6")), Is.True);
                Assert.That(((object)new IPv4Address(3, 4, 5, 6)).Equals("3"), Is.False);


                address = new IPv4Address(8, 8, 8, 8);
                Assert.That(address, Is.Not.EqualTo(new IPv4Address(2, 3, 4, 5)));
                Assert.That(address, Is.Not.EqualTo(new IPv4Address(8, 3, 4, 5)));
                Assert.That(address, Is.Not.EqualTo(new IPv4Address(8, 8, 4, 5)));
                Assert.That(address, Is.Not.EqualTo(new IPv4Address(8, 8, 8, 5)));
                Assert.That(address, Is.EqualTo(new IPv4Address(8, 8, 8, 8)));

                ConcurrentDictionary<IPv4Address, string> dict = new ConcurrentDictionary<IPv4Address, string>();

                for (byte i1 = 1; i1 < 246; i1 += 8)
                    for (byte i2 = 1; i2 < 246; i2 += 8)
                        for (byte i3 = 1; i3 < 246; i3 += 8)
                            for (byte i4 = 1; i4 < 246; i4 += 8)
                            {
                                address = new IPv4Address(i1, i2, i3, i4);
                                var res = dict.TryAdd(address, address.ToString());
                                Assert.That(res, Is.True);
                            }

                Assert.Throws(typeof(ArgumentException), () => { var ip = (IPv4Address)System.Net.IPAddress.IPv6Any; });
            });
        }

        [Test]
        public void TestSerializable()
        {
            IPv4Address ipv4Address = new IPv4Address("192.168.178.33");
            var data = Tools.Serialize(ipv4Address);
            string json = System.Text.Encoding.Default.GetString(data);
            IPv4Address result = Tools.Deserialize<IPv4Address>(data);

            Assert.That(result, Is.EqualTo(ipv4Address), json);
        }
    }
}