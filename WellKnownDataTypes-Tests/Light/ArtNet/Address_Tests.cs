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
    }
}