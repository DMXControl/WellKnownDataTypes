using org.dmxc.wkdt.Light.ArtNet;

namespace org.dmxc.wkdt.Tests.Light.ArtNet
{
    public class Subnet_Tests
    {
        [Test]
        public void TestSubnet()
        {
            HashSet<Subnet> subNets = new HashSet<Subnet>();
            for (byte b = 0; b < byte.MaxValue; b++)
            {
                try
                {
                    Subnet s = (Subnet)b;

                    Assert.Multiple(() =>
                    {
                        Assert.That(b, Is.LessThanOrEqualTo(0xf));
                        Assert.That(s.Value, Is.EqualTo(b));
                        Assert.That(s.ToString(), Is.Not.Empty);
                    });
                    subNets.Add(s);
                    Assert.That((byte)s, Is.EqualTo(b));
                }
                catch
                {
                    Assert.That(b, Is.GreaterThan(0xf));
                }
            }
            Assert.Multiple(() =>
            {
                Assert.That(subNets, Has.Count.EqualTo(0xf + 1));
                Assert.That(subNets.OrderByDescending(s => s).OrderBy(s => s.GetHashCode()).OrderBy(s => s).ToList(), Has.Count.EqualTo(0xf + 1));
            });

            Assert.Multiple(() =>
            {
                Assert.That(new Subnet(1) == (Subnet)1, Is.True);
                Assert.That(new Subnet(1) != (Subnet)1, Is.False);
                Assert.That(new Subnet(1) == (Subnet)2, Is.False);
                Assert.That(new Subnet(1) != (Subnet)2, Is.True);
                Assert.That(new Subnet(1).GetHashCode(), Is.EqualTo(((Subnet)1).GetHashCode()));
                Assert.That(new Subnet(1).GetHashCode(), Is.Not.EqualTo(((Subnet)2).GetHashCode()));
                Assert.That(new Subnet(1).Equals(null), Is.False);
                Assert.That(new Subnet(1).Equals((object)1), Is.False);
                Assert.That(new Subnet(1).Equals((object)(Subnet)1), Is.True);
                Assert.That(new Subnet(1).Equals((Subnet)1), Is.True);
                Assert.That(new Subnet(1).Equals((Subnet)2), Is.False);
                Assert.That(new Subnet(0).Equals(Subnet.Default), Is.True);
                Assert.That(new Subnet(1).ToString, Is.EqualTo("1"));
                Assert.That(new Subnet(2).ToString, Is.EqualTo("2"));
                Assert.That(new Subnet(1).ToStringDetailed(), Is.EqualTo("Subnet: 1(0x1)"));
                Assert.That(new Subnet(2).ToStringDetailed(), Is.EqualTo("Subnet: 2(0x2)"));
                Assert.That(new Subnet(15).ToStringDetailed(), Is.EqualTo("Subnet: 15(0xf)"));
                Assert.Throws(typeof(ArgumentException), () => new Subnet(16).ToStringDetailed());
            });
        }
        [Test]
        public void TestSerializable()
        {
            Subnet subnet = new Subnet(13);
            var data = Tools.Serialize(subnet);
            string json = System.Text.Encoding.Default.GetString(data);
            Subnet result = Tools.Deserialize<Subnet>(data);

            Assert.That(result, Is.EqualTo(subnet), json);
        }
    }
}