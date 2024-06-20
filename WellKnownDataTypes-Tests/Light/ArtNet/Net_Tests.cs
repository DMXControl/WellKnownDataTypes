using org.dmxc.wkdt.Light.ArtNet;

namespace org.dmxc.wkdt.Tests.Light.ArtNet
{
    public class Net_Tests
    {
        [Test]
        public void TestNet()
        {
            HashSet<Net> nets = new HashSet<Net>();
            for (byte b = 0; b < byte.MaxValue; b++)
            {
                try
                {
                    Net n = (Net)b;
                    Assert.Multiple(() =>
                    {
                        Assert.That(b, Is.LessThanOrEqualTo(0x7f));
                        Assert.That(n.Value, Is.EqualTo(b));
                        Assert.That(n.ToString(), Is.Not.Empty);
                    });
                    nets.Add(n);
                    Assert.That((byte)n, Is.EqualTo(b));
                }
                catch
                {
                    Assert.That(b, Is.GreaterThan(0x7f));
                }
            }
            Assert.Multiple(() =>
            {
                Assert.That(nets, Has.Count.EqualTo(0x7f + 1));
                Assert.That(nets.OrderByDescending(s => s).OrderBy(s => s.GetHashCode()).OrderBy(s => s).ToList(), Has.Count.EqualTo(0x7f + 1));
            });

            Assert.Multiple(() =>
            {
                Assert.That(new Net(1) == (Net)1, Is.True);
                Assert.That(new Net(1) != (Net)1, Is.False);
                Assert.That(new Net(1) == (Net)2, Is.False);
                Assert.That(new Net(1) != (Net)2, Is.True);
                Assert.That(new Net(1).GetHashCode(), Is.EqualTo(((Net)1).GetHashCode()));
                Assert.That(new Net(1).GetHashCode(), Is.Not.EqualTo(((Net)2).GetHashCode()));
                Assert.That(new Net(1).Equals(null), Is.False);
                Assert.That(new Net(1).Equals((object)1), Is.False);
                Assert.That(new Net(1).Equals((object)(Net)1), Is.True);
                Assert.That(new Net(1).Equals((Net)1), Is.True);
                Assert.That(new Net(1).Equals((Net)2), Is.False);
                Assert.That(new Net(1).ToString, Is.EqualTo("1"));
                Assert.That(new Net(2).ToString, Is.EqualTo("2"));
                Assert.That(new Net(1).ToStringDetailed(), Is.EqualTo("Net: 1(0x01)"));
                Assert.That(new Net(2).ToStringDetailed(), Is.EqualTo("Net: 2(0x02)"));
                Assert.That(new Net(15).ToStringDetailed(), Is.EqualTo("Net: 15(0x0f)"));
                Assert.That(new Net(16).ToStringDetailed(), Is.EqualTo("Net: 16(0x10)"));
            });
        }

        [Test]
        public void TestSerializable()
        {
            Net net = new Net(2);
            var data = Tools.Serialize(net);
            string json = System.Text.Encoding.Default.GetString(data);
            Net result = Tools.Deserialize<Net>(data);

            Assert.That(result, Is.EqualTo(net), json);
        }
    }
}