namespace org.dmxc.wkdt.Light.ArtNet
{
    [Serializable]
    public readonly struct Net : IEquatable<Net>, IComparable<Net>
    {
#if NET8_0_OR_GREATER
        [JsonInclude]
#endif
        public readonly byte Value;


#if NET8_0_OR_GREATER
        [JsonConstructor]
#endif
        public Net(byte value)
        {
            if ((byte)(value & 0x7f) != value)
                throw new ArgumentException($"Value (0x{value:x}) out of range! A valid value is between 0x00 and 0x7f.");
            Value = value;
        }

        public static implicit operator byte(in Net net)
        {
            return net.Value;
        }
        public static implicit operator Net(in byte b)
        {
            return new Net(b);
        }

        public override bool Equals(object obj)
        {
            return obj is Net other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            int hashCode = -971048872;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        public string ToStringDetailed()
        {
            return $"Net: {Value}(0x{Value:x2})";
        }

        public static bool operator ==(in Net a, in Net b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(in Net a, in Net b)
        {
            return !a.Equals(b);
        }

        public bool Equals(Net other)
        {
            return Value == other.Value;
        }

        public int CompareTo(Net other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}