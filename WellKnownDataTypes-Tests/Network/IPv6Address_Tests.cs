using org.dmxc.wkdt.Network;
using System.Collections.Concurrent;
using System.Numerics;

namespace org.dmxc.wkdt.Tests.Network
{
    public class IPv6Address_Tests
    {
        [Test]
        public void TestIPv6Address()
        {
            var address = IPv6Address.LocalHost;
            Assert.Multiple(() =>
            {
                Assert.That(IPv6Address.Empty.ToString(), Is.EqualTo("::"));
                Assert.That(address.ToString(), Is.EqualTo("::1"));
                Assert.That(address, Is.EqualTo(new IPv6Address(address.ToString())));
                Assert.Throws(typeof(FormatException), () => new IPv6Address("1.2.3."));
                Assert.DoesNotThrow(() => new IPv6Address("2::2"));
                Assert.DoesNotThrow(() => new IPv6Address("2::"));
                Assert.DoesNotThrow(() => new IPv6Address("::2"));
                Assert.DoesNotThrow(() => new IPv6Address("::20"));
                Assert.DoesNotThrow(() => new IPv6Address("::02"));
                Assert.DoesNotThrow(() => new IPv6Address("::200"));
                Assert.DoesNotThrow(() => new IPv6Address("::002"));
                Assert.DoesNotThrow(() => new IPv6Address("::2000"));
                Assert.DoesNotThrow(() => new IPv6Address("::0002"));
                Assert.DoesNotThrow(() => new IPv6Address("fff2::202"));
                Assert.DoesNotThrow(() => new IPv6Address("fff2:222::202"));
                Assert.DoesNotThrow(() => new IPv6Address("fff2:222::202"));
                Assert.DoesNotThrow(() => new IPv6Address("2001:0db8:85a3:0000:0000:8a2e:0370:7334"));
                Assert.DoesNotThrow(() => new IPv6Address(new BigInteger(12342152151345345)));
                Assert.DoesNotThrow(() => new IPv6Address(new BigInteger(0)));
                Assert.DoesNotThrow(() => new IPv6Address(new BigInteger(1)));

                var bi = new BigInteger(12342152151345345);
                Assert.That(new IPv6Address(bi), Is.EqualTo((IPv6Address)bi));
                Assert.That((BigInteger)new IPv6Address(bi), Is.EqualTo(bi));
                var bytes = new byte[16];
                Assert.That(new IPv6Address(bytes), Is.EqualTo((IPv6Address)bytes));
                Assert.That((byte[])new IPv6Address(bytes), Is.EqualTo(bytes));
                bytes = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
                Assert.That(new IPv6Address(bytes), Is.EqualTo((IPv6Address)bytes));
                Assert.That((byte[])new IPv6Address(bytes), Is.EqualTo(bytes));
                bytes = new byte[16] { 0xEE, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xFF, 0 };
                Assert.That(new IPv6Address(bytes), Is.EqualTo((IPv6Address)bytes));
                Assert.That((byte[])new IPv6Address(bytes), Is.EqualTo(bytes));
                Assert.That(new IPv6Address(bytes).ToString(), Is.EqualTo("ee00::ff00"));

                Assert.That(new IPv6Address(bytes.ToList()), Is.EqualTo((IPv6Address)bytes));

                Assert.Throws(typeof(ArgumentOutOfRangeException), () => new IPv6Address(new byte[] { 1 }));
                Assert.Throws(typeof(ArgumentOutOfRangeException), () => new IPv6Address(new List<byte>() { 1 }));


                System.Net.IPAddress ip = new System.Net.IPAddress(bytes);
                address = (IPv6Address)bytes;
                Assert.That(ip, Is.EqualTo((System.Net.IPAddress)address));
                Assert.That(address, Is.EqualTo((IPv6Address)ip));

                //    bytes = new byte[] { 1, 1, 1, 1 };
                //    Assert.That(bytes, Is.EqualTo((byte[])new IPv6Address(bytes)));

                Assert.That(new IPv6Address(new BigInteger(1)) == new IPv6Address("::1"), Is.True);
                Assert.That(new IPv6Address(new BigInteger(1)) == new IPv6Address("1::"), Is.False);
                Assert.That(new IPv6Address(new BigInteger(1)) != new IPv6Address("::1"), Is.False);
                Assert.That(((object)new IPv6Address(new BigInteger(1))).Equals(new IPv6Address("::1")), Is.True);
                Assert.That((new IPv6Address(new BigInteger(1))).Equals(new IPv6Address("::1")), Is.True);
                Assert.That(((object)new IPv6Address(new BigInteger(1))).Equals("::1"), Is.False);
                Assert.That((new IPv6Address(new BigInteger(1))).Equals("::1"), Is.False);


                ConcurrentDictionary<IPv6Address, string> dict = new ConcurrentDictionary<IPv6Address, string>();

                Random rnd = new Random();
                for (int i = 0; i < 1000; i++)
                {
                    var bigI = new BigInteger(rnd.NextInt64());
                    address = new IPv6Address(bigI);
                    var res = dict.TryAdd(address, address.ToString());
                    Assert.That(res, Is.True, address.String);
                    Assert.That(address.Raw, Is.EqualTo(bigI), address.String);
                }

                Assert.Throws(typeof(ArgumentException), () => { var ip = (IPv6Address)System.Net.IPAddress.Any; });
            });
        }

        [Test]
        public void TestSerializable()
        {
            IPv6Address ipv4Address = new IPv6Address("fe80::ad64:5a9a:8869:1c4f");
            var data = Tools.Serialize(ipv4Address);
            string json = System.Text.Encoding.Default.GetString(data);
            IPv6Address result = Tools.Deserialize<IPv6Address>(data);

            Assert.That(result, Is.EqualTo(ipv4Address), json);
        }
    }
}