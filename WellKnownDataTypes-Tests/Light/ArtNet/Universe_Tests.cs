using org.dmxc.wkdt.Light.ArtNet;

namespace org.dmxc.wkdt.Tests.Light.ArtNet
{
    public class Universe_Tests
    {
        [Test]
        public void TestUniverse()
        {
            HashSet<Universe> universes = new HashSet<Universe>();
            for (byte b = 0; b < byte.MaxValue; b++)
            {
                try
                {
                    Universe u = (Universe)b;
                    Assert.Multiple(() =>
                    {
                        Assert.That(b, Is.LessThanOrEqualTo(0xf));
                        Assert.That(u.Value, Is.EqualTo(b));
                        Assert.That(u.ToString(), Is.Not.Empty);
                    });
                    universes.Add(u);
                    Assert.That((byte)u, Is.EqualTo(b));
                }
                catch
                {
                    Assert.That(b, Is.GreaterThan(0xf));
                }
            }
            Assert.Multiple(() =>
            {
                Assert.That(universes, Has.Count.EqualTo(0xf + 1));
                Assert.That(universes.OrderByDescending(s => s).OrderBy(s => s.GetHashCode()).OrderBy(s => s).ToList(), Has.Count.EqualTo(0xf + 1));
            });

            Assert.Multiple(() =>
            {
                Assert.That(new Universe(1) == (Universe)1, Is.True);
                Assert.That(new Universe(1) != (Universe)1, Is.False);
                Assert.That(new Universe(1) == (Universe)2, Is.False);
                Assert.That(new Universe(1) != (Universe)2, Is.True);
                Assert.That(new Universe(1).GetHashCode(), Is.EqualTo(((Universe)1).GetHashCode()));
                Assert.That(new Universe(1).GetHashCode(), Is.Not.EqualTo(((Universe)2).GetHashCode()));
                Assert.That(new Universe(1).Equals(null), Is.False);
                Assert.That(new Universe(1).Equals((object)1), Is.False);
                Assert.That(new Universe(1).Equals((object)(Universe)1), Is.True);
                Assert.That(new Universe(1).Equals((Universe)1), Is.True);
                Assert.That(new Universe(1).Equals((Universe)2), Is.False);
                Assert.That(new Universe(0).Equals(Universe.Default), Is.True);
            });
        }
        [Test]
        public void TestSerializable()
        {
            Universe universe = new Universe(13);
            var data = Tools.Serialize(universe);
            string json = System.Text.Encoding.Default.GetString(data);
            Universe result = Tools.Deserialize<Universe>(data);

            Assert.That(result, Is.EqualTo(universe), json);
        }
    }
}