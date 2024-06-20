using org.dmxc.wkdt.Light.ArtNet;

namespace org.dmxc.wkdt.Tests.Light.ArtNet
{
    public class Address_Tests
    {
        [Test]
        public void TestAddress()
        {
            Address a = new Address(1);
            Assert.Multiple(() =>
            {
                Assert.That(a.Universe.Value, Is.EqualTo(1));
                Assert.That(a.Combined, Is.EqualTo(1));
            });

            a = new Address(15);
            Assert.Multiple(() =>
            {
                Assert.That(a.Universe.Value, Is.EqualTo(15));
                Assert.That(a.Combined, Is.EqualTo(15));
            });

            a = new Address(16);
            Assert.Multiple(() =>
            {
                Assert.That(a.Universe.Value, Is.EqualTo(0));
                Assert.That(a.Subnet.Value, Is.EqualTo(1));
                Assert.That(a.Combined, Is.EqualTo(16));
            });

            a = new Address(0xff);
            Assert.Multiple(() =>
            {
                Assert.That(a.Universe.Value, Is.EqualTo(15));
                Assert.That(a.Subnet.Value, Is.EqualTo(15));
                Assert.That(a.Combined, Is.EqualTo(0xff));
                Assert.That(new Address(16) == new Address(1, 0));
                Assert.That(new Address(16) != new Address(0, 1));
                Assert.That(new Address(16), Is.EqualTo(new Address(1, 0)));
                Assert.That(new Address(16), Is.Not.EqualTo(new Address(0, 1)));
                Assert.That(new Address(16), Is.EqualTo((object)new Address(1, 0)));
                Assert.That(new Address(16).GetHashCode(), Is.Not.EqualTo(new Address(0, 1).GetHashCode()));
                Assert.That(new Address(16).Equals((object)new Address(1, 0)), Is.True);
                Assert.That(new Address(16).Equals((object)new Address(0, 1)), Is.False);
                Assert.That(new Address(16).Equals(null), Is.False);
                Assert.That(new Address(16), Is.Not.EqualTo(null));
                Assert.That(new Address(16).ToString(), Is.Not.Empty);
                Assert.That(new Address(1).ToString, Is.EqualTo("1"));
                Assert.That(new Address(2).ToString, Is.EqualTo("2"));
                Assert.That(new Address(1).ToStringDetailed(), Is.EqualTo("Address: 1(0x01) | Subnet: 0(0x0), Universe: 1(0x1)"));
                Assert.That(new Address(2).ToStringDetailed(), Is.EqualTo("Address: 2(0x02) | Subnet: 0(0x0), Universe: 2(0x2)"));
                Assert.That(new Address(15).ToStringDetailed(), Is.EqualTo("Address: 15(0x0f) | Subnet: 0(0x0), Universe: 15(0xf)"));
                Assert.That(new Address(16).ToStringDetailed(), Is.EqualTo("Address: 16(0x10) | Subnet: 1(0x1), Universe: 0(0x0)"));
                Assert.That(new Address(255).ToStringDetailed(), Is.EqualTo("Address: 255(0xff) | Subnet: 15(0xf), Universe: 15(0xf)"));
                Assert.That(new Address(0xdd).ToStringDetailed(), Is.EqualTo("Address: 221(0xdd) | Subnet: 13(0xd), Universe: 13(0xd)"));
            });

            HashSet<Address> addresses = new HashSet<Address>();
            for (byte b = 0; b < byte.MaxValue; b++)
            {
                Address address = (Address)b;
                addresses.Add(address);
                Assert.That((byte)address, Is.EqualTo(b));
            }
            Assert.That(addresses, Has.Count.EqualTo(byte.MaxValue));

            Assert.That(addresses.OrderByDescending(s => s.Universe).ThenBy(s => s).ToArray(), Has.Length.EqualTo(byte.MaxValue));
        }

        [Test]
        public void TestSerializable()
        {
            Address address = new Address(2, 3);
            var data = Tools.Serialize(address);
            string json = System.Text.Encoding.Default.GetString(data);
            Address result = Tools.Deserialize<Address>(data);

            Assert.That(result, Is.EqualTo(address), json);
        }
    }
}