using org.dmxc.wkdt.Light.ArtNet;

namespace org.dmxc.wkdt.Tests.Light.ArtNet
{
    public class PortAddress_Tests
    {
        [Test]
        public void TestPortAddress()
        {
            HashSet<PortAddress> portAddresses = new HashSet<PortAddress>();
            ushort count = 0;
            for (ushort b = 0; b < ushort.MaxValue; b++)
            {
                try
                {
                    PortAddress pa = (PortAddress)b;

                    Assert.Multiple(() =>
                    {
                        Assert.That(b, Is.LessThanOrEqualTo(0x7fff));
                        Assert.That(pa.Combined, Is.EqualTo(b));
                        Assert.That(pa.ToString(), Is.Not.Empty);
                    });
                    portAddresses.Add(pa);
                    Assert.That((ushort)pa, Is.EqualTo(b));
                    count++;
                }
                catch
                {
                    Assert.That(b, Is.GreaterThan(0x7fff));
                }
            }
            Assert.Multiple(() =>
            {
                Assert.That(portAddresses, Has.Count.EqualTo(count));
                Assert.That(portAddresses.OrderByDescending(s => s).OrderBy(s => s.GetHashCode()).OrderBy(s => s).ToList(), Has.Count.EqualTo(count));
            });

            Assert.Multiple(() =>
            {
                Assert.That(new PortAddress((ushort)1) == new PortAddress(0, 0, 1), Is.True);
                Assert.That(new PortAddress((ushort)1) != new PortAddress(0, (Universe)1), Is.False);
                Assert.That(new PortAddress((ushort)1) != new PortAddress(0, new Address(0, 1)), Is.False);
                Assert.That(new PortAddress(1) == new PortAddress(0, 0, 2), Is.False);
                Assert.That(new PortAddress(1) != new PortAddress(0, (Universe)2), Is.True);
                Assert.That(new PortAddress(1).GetHashCode(), Is.EqualTo(((PortAddress)1).GetHashCode()));
                Assert.That(new PortAddress((ushort)1).GetHashCode(), Is.Not.EqualTo(((PortAddress)2).GetHashCode()));
                Assert.That(new PortAddress(1).Equals(null), Is.False);
                Assert.That(new PortAddress(1).Equals((object)1), Is.False);
                Assert.That(new PortAddress(1).Equals((object)(PortAddress)1), Is.True);
                Assert.That(new PortAddress(1).Equals((PortAddress)1), Is.True);
                Assert.That(new PortAddress(1).Equals((PortAddress)2), Is.False);
                Assert.That(new PortAddress(1).ToString, Is.EqualTo("1"));
                Assert.That(new PortAddress(2).ToString, Is.EqualTo("2"));
                Assert.That(new PortAddress(1).ToStringDetailed(), Is.EqualTo("PortAddress: 1(0x0001) | Net: 0(0x00), Subnet: 0(0x0), Universe: 1(0x1)"));
                Assert.That(new PortAddress(2).ToStringDetailed(), Is.EqualTo("PortAddress: 2(0x0002) | Net: 0(0x00), Subnet: 0(0x0), Universe: 2(0x2)"));
                Assert.That(new PortAddress(15).ToStringDetailed(), Is.EqualTo("PortAddress: 15(0x000f) | Net: 0(0x00), Subnet: 0(0x0), Universe: 15(0xf)"));
                Assert.That(new PortAddress(16).ToStringDetailed(), Is.EqualTo("PortAddress: 16(0x0010) | Net: 0(0x00), Subnet: 1(0x1), Universe: 0(0x0)"));
                Assert.That(new PortAddress(255).ToStringDetailed(), Is.EqualTo("PortAddress: 255(0x00ff) | Net: 0(0x00), Subnet: 15(0xf), Universe: 15(0xf)"));
                Assert.That(new PortAddress(0xdd).ToStringDetailed(), Is.EqualTo("PortAddress: 221(0x00dd) | Net: 0(0x00), Subnet: 13(0xd), Universe: 13(0xd)"));
                Assert.That(new PortAddress(0x7ddd).ToStringDetailed(), Is.EqualTo("PortAddress: 32221(0x7ddd) | Net: 125(0x7d), Subnet: 13(0xd), Universe: 13(0xd)"));
            });
        }

        [Test]
        public void TestSerializable()
        {
            PortAddress portAddress = new PortAddress(2, 3, 4);
            var data = Tools.Serialize(portAddress);
            string json = System.Text.Encoding.Default.GetString(data);
            PortAddress result = Tools.Deserialize<PortAddress>(data);

            Assert.That(result, Is.EqualTo(portAddress), json);
        }
    }
}