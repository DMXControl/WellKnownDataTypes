using org.dmxc.wkdt.Network;
using System.Collections.Concurrent;

namespace org.dmxc.wkdt.Tests.Network
{
    public class MACAddress_Tests
    {
        [Test]
        public void TestMACAddress()
        {
            Assert.Multiple(() =>
            {
                var address = MACAddress.Empty;
                Assert.That(address.ToString(), Is.EqualTo("00:00:00:00:00:00"));
                var bytes = new byte[6] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66 };
                address = new MACAddress(bytes);
                Assert.That((byte[])address, Is.EqualTo(bytes));
                Assert.That((MACAddress)bytes, Is.EqualTo(address));
                Assert.Throws(typeof(ArgumentOutOfRangeException), () => new MACAddress(new byte[] { 1, 2, 3, 4, 5, 6, 7 }));
                Assert.That(address, Is.EqualTo(new MACAddress(new List<byte>(bytes))));
                Assert.Throws(typeof(ArgumentOutOfRangeException), () => new MACAddress(new List<byte>() { 1, 2, 3, 4, 5 }));
                Assert.That(address, Is.EqualTo(new MACAddress("11:22:33:44:55:66")));
                Assert.That(address, Is.EqualTo(new MACAddress("11-22-33-44-55-66")));
                Assert.That(address, Is.EqualTo(new MACAddress("1122.3344.5566")));
                Assert.That(address, Is.EqualTo(new MACAddress("112233445566")));
                Assert.Throws(typeof(FormatException), () => new MACAddress("0123456789AG"));

                Assert.That(new MACAddress(0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff) == new MACAddress("AA:BB:CC:DD:EE:FF"), Is.True);
                Assert.That(new MACAddress(0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff) != new MACAddress("AA:BB:CC:DD:EE:FF"), Is.False);
                Assert.That(((object)new MACAddress(0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff)).Equals(new MACAddress("aa:bb:cc:dD:Ee:fF")), Is.True);
                Assert.That(((object)new MACAddress(0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff)).Equals("3"), Is.False);

                address = new MACAddress(8, 8, 8, 8, 8, 8);
                Assert.That(address, Is.Not.EqualTo(new MACAddress(2, 3, 4, 5, 6, 7)));
                Assert.That(address, Is.Not.EqualTo(new MACAddress(8, 3, 4, 5, 6, 7)));
                Assert.That(address, Is.Not.EqualTo(new MACAddress(8, 8, 4, 5, 6, 7)));
                Assert.That(address, Is.Not.EqualTo(new MACAddress(8, 8, 8, 5, 6, 7)));
                Assert.That(address, Is.Not.EqualTo(new MACAddress(8, 8, 8, 8, 6, 7)));
                Assert.That(address, Is.Not.EqualTo(new MACAddress(8, 8, 8, 8, 8, 7)));
                Assert.That(address, Is.EqualTo(new MACAddress(8, 8, 8, 8, 8, 8)));

                ConcurrentDictionary<MACAddress, string> dict = new ConcurrentDictionary<MACAddress, string>();
                for (byte i1 = 1; i1 < 200; i1 += 24)
                    for (byte i2 = 1; i2 < 200; i2 += 24)
                        for (byte i3 = 1; i3 < 200; i3 += 24)
                            for (byte i4 = 1; i4 < 200; i4 += 24)
                                for (byte i5 = 1; i5 < 200; i5 += 24)
                                    for (byte i6 = 1; i6 < 200; i6 += 24)
                                    {
                                        address = new MACAddress(i1, i2, i3, i4, i5, i6);
                                        var res = dict.TryAdd(address, address.ToString());
                                        Assert.That(res, Is.True);
                                    }
            });
        }
    }
}