using org.dmxc.wkdt.Light.RDM;

namespace org.dmxc.wkdt.Tests.Light.RDM
{
    public class UID_Tests
    {
        [Test]
        public void TestUID()
        {
            Assert.Multiple(() =>
            {
                Assert.That(UID.Empty.ToString(), Is.EqualTo("0000:00000000"));
                Assert.That(UID.Broadcast.ToString(), Is.EqualTo("FFFF:FFFFFFFF"));
                Assert.That(new UID("1234:56789ABC").ToString(), Is.EqualTo("1234:56789ABC"));
                Assert.That(new UID("123456789ABC").ToString(), Is.EqualTo("1234:56789ABC"));
                Assert.Throws(typeof(FormatException), () => new UID("0123:12345678f"));
                Assert.Throws(typeof(FormatException), () => new UID("0123:123456S"));
                Assert.Throws(typeof(FormatException), () => new UID("0123~123456S"));
            });
            Assert.Multiple(() =>
            {
                Assert.That(UID.Empty.ManufacturerID, Is.EqualTo(0));
                Assert.That(new UID(44, 1234567).ManufacturerID, Is.EqualTo(44));

                Assert.That(UID.Empty.IsValidDeviceUID, Is.False);
                Assert.That(new UID(0, 122334).IsValidDeviceUID, Is.False);
                Assert.That(UID.Broadcast.IsValidDeviceUID, Is.False);
                Assert.That(new UID("0123:456789ab").IsValidDeviceUID, Is.True);

            });

            Assert.That((byte[])new UID(0, 0x12345678), Is.EqualTo(new byte[] { 0x00, 0x00, 0x12, 0x34, 0x56, 0x78 }));


            List<UID> list = new List<UID>();
            list.Add(new UID(0x1FFF, 0x00000031));
            list.Add(new UID(0x2FFF, 0x00000022));
            list.Add(new UID(0x3FFF, 0x00000013));
            list.Add(new UID(0x4FFF, 0x00000001));
            list.Add(new UID(0x4FFF, 0x00000002));
            list.Add(new UID(0x4FFF, 0x00000003));
            list.Add(new UID(0x0FFF, 0x00000001));
            list.Add(new UID(0x0FFF, 0x00000002));
            list.Add(new UID(0x0FFF, 0x00000003));

            var orderd = list.OrderBy(uid => uid).ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(orderd[0], Is.EqualTo(new UID(0x0FFF, 0x00000001)));
                Assert.That(orderd[1], Is.EqualTo(new UID(0x0FFF, 0x00000002)));
                Assert.That(orderd[2], Is.EqualTo(new UID(0x0FFF, 0x00000003)));
                Assert.That(orderd[3], Is.EqualTo(new UID(0x1FFF, 0x00000031)));
                Assert.That(orderd[4], Is.EqualTo(new UID(0x2FFF, 0x00000022)));
                Assert.That(orderd[5], Is.EqualTo(new UID(0x3FFF, 0x00000013)));
                Assert.That(orderd[6], Is.EqualTo(new UID(0x4FFF, 0x00000001)));
                Assert.That(orderd[7], Is.EqualTo(new UID(0x4FFF, 0x00000002)));
                Assert.That(orderd[8], Is.EqualTo(new UID(0x4FFF, 0x00000003)));
            });
            Assert.Multiple(() =>
            {
                Assert.That(new UID(10, 1) * 2, Is.EqualTo(new UID(20, 2)));
                Assert.That(new UID(10, 1) * 3, Is.EqualTo(new UID(30, 3)));
                Assert.That(new UID(10, 1) * 4, Is.EqualTo(new UID(40, 4)));

                Assert.That(new UID(20, 2) / 2, Is.EqualTo(new UID(10, 1)));
                Assert.That(new UID(30, 3) / 3, Is.EqualTo(new UID(10, 1)));
                Assert.That(new UID(40, 4) / 4, Is.EqualTo(new UID(10, 1)));

                Assert.That(new UID(10, 1) + 2, Is.EqualTo(new UID(10, 3)));
                Assert.That(new UID(10, 1) + 3, Is.EqualTo(new UID(10, 4)));
                Assert.That(new UID(10, 1) + 4, Is.EqualTo(new UID(10, 5)));
                Assert.That(new UID(10, 1) + new UID(10, 1), Is.EqualTo(new UID(20, 2)));
                Assert.That(new UID(10, 1) + new UID(20, 2), Is.EqualTo(new UID(30, 3)));
                Assert.That(new UID(10, 1) + new UID(30, 3), Is.EqualTo(new UID(40, 4)));

                Assert.That(new UID(10, 10) - 1, Is.EqualTo(new UID(10, 9)));
                Assert.That(new UID(10, 1) - new UID(2, 1), Is.EqualTo(new UID(8, 0)));
            });

            Assert.Multiple(() =>
            {
                Assert.That(new UID(5) == (UID)5, Is.True);
                Assert.That(new UID(1) != (UID)1, Is.False);
                Assert.That(new UID(1) == (UID)2, Is.False);
                Assert.That(new UID(1) != (UID)2, Is.True);
                Assert.That(new UID(1) <= (UID)2, Is.True);
                Assert.That(new UID(1) < (UID)2, Is.True);
                Assert.That(new UID(2) <= (UID)1, Is.False);
                Assert.That(new UID(2) < (UID)1, Is.False);
                Assert.That(new UID(2) >= (UID)1, Is.True);
                Assert.That(new UID(2) > (UID)1, Is.True);
                Assert.That(new UID(1) >= (UID)2, Is.False);
                Assert.That(new UID(1) > (UID)2, Is.False);
                Assert.That(new UID(1, 0) <= new UID(2, 0), Is.True);
                Assert.That(new UID(1, 0) < new UID(2, 0), Is.True);
                Assert.That(new UID(2, 0) <= new UID(1, 0), Is.False);
                Assert.That(new UID(2, 0) < new UID(1, 0), Is.False);
                Assert.That(new UID(2, 0) >= new UID(1, 0), Is.True);
                Assert.That(new UID(2, 0) > new UID(1, 0), Is.True);
                Assert.That(new UID(1, 0) >= new UID(2, 0), Is.False);
                Assert.That(new UID(1, 0) > new UID(2, 0), Is.False);
                Assert.That(new UID(1).GetHashCode(), Is.EqualTo(((UID)1).GetHashCode()));
                Assert.That(new UID(1).GetHashCode(), Is.Not.EqualTo(((UID)2).GetHashCode()));
                Assert.That(new UID(1).Equals(null), Is.False);
                Assert.That(new UID(1).Equals((object)1), Is.False);
                Assert.That(new UID(1).Equals((object)(UID)1), Is.True);
                Assert.That(new UID(1).Equals((UID)1), Is.True);
                Assert.That(new UID(1).Equals((UID)2), Is.False);
                Assert.That(new UID(0).Equals(UID.Empty), Is.True);
            });
        }
    }
}