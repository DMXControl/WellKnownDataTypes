namespace org.dmxc.wkdt.Light.ArtNet
{
    [Serializable]
    public readonly struct PortAddress : IEquatable<PortAddress>, IComparable<PortAddress>
    {
#if NET8_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
#endif
        public readonly Net Net;
#if NET8_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
#endif
        public readonly Subnet Subnet;
#if NET8_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
#endif
        public readonly Universe Universe;
#if NET8_0_OR_GREATER
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
#endif
        public readonly Address Address;

#if NET8_0_OR_GREATER
        [JsonInclude]
#endif
        public readonly ushort Combined;

        public PortAddress(in Net net, in Subnet subnet, in Universe universe)
        {
            Net = net;
            Subnet = subnet;
            Universe = universe;
            Address = new Address(subnet, universe);
            Combined = (ushort)((Net << 8) + Address.Combined);
        }

#if NET8_0_OR_GREATER
        [JsonConstructor]
#endif
        public PortAddress(ushort combined)
        {
            if ((ushort)(combined & 0x7fff) != combined)
                throw new ArgumentException($"Value (0x{combined:x}) out of range! A valid value is between 0x0000 and 0x7fff.");
            Net = (Net)((combined >> 8) & 0x7f);
            Subnet = (Subnet)((combined >> 4) & 0xf);
            Universe = (Universe)(combined & 0xf);
            Address = new Address(Subnet, Universe);
            Combined = combined;
        }
        public PortAddress(in Subnet subnet, in Universe universe) : this(0, subnet, universe)
        {
        }
        public PortAddress(in Address address) : this(0, address.Subnet, address.Universe)
        {
        }
        public PortAddress(in Net net, in Address address) : this(net, address.Subnet, address.Universe)
        {
        }

        public static implicit operator ushort(in PortAddress address)
        {
            return address.Combined;
        }
        public static implicit operator PortAddress(in ushort b)
        {
            return new PortAddress(b);
        }

        public override bool Equals(object obj)
        {
            return obj is PortAddress other &&
                   Equals(other.Combined);
        }

        public override int GetHashCode()
        {
            int hashCode = -971048872;
            hashCode = hashCode * -1521134295 + Combined.GetHashCode();
            return hashCode;
        }
        public override string ToString()
        {
            return $"{Combined}(0x{Combined:x4}) / {Net}, {Subnet}, {Universe}";
        }

        public static bool operator ==(in PortAddress a, in PortAddress b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(in PortAddress a, in PortAddress b)
        {
            return !a.Equals(b);
        }

        public bool Equals(PortAddress other)
        {
            return Combined == other.Combined;
        }

        public int CompareTo(PortAddress other)
        {
            return Combined.CompareTo(other.Combined);
        }
    }
}