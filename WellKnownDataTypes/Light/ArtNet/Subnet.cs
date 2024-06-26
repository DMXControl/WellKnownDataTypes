﻿namespace org.dmxc.wkdt.Light.ArtNet
{
    [Serializable]
    public readonly struct Subnet : IEquatable<Subnet>, IComparable<Subnet>
    {
        public static readonly Subnet Default = new Subnet();

#if NET8_0_OR_GREATER
        [JsonInclude]
#endif
        public readonly byte Value;

#if NET8_0_OR_GREATER
        [JsonConstructor]
#endif
        public Subnet(byte value)
        {
            if ((byte)(value & 0x0f) != value)
                throw new ArgumentException($"Value (0x{value:x}) out of range! A valid value is between 0x00 and 0x0f.");
            Value = value;
        }

        public static implicit operator byte(in Subnet subnet)
        {
            return subnet.Value;
        }
        public static implicit operator Subnet(in byte b)
        {
            return new Subnet(b);
        }

        public override bool Equals(object obj)
        {
            return obj is Subnet other &&
                   Equals(other.Value);
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
            return $"Subnet: {Value}(0x{Value:x1})";
        }

        public static bool operator ==(in Subnet a, in Subnet b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(in Subnet a, in Subnet b)
        {
            return !a.Equals(b);
        }

        public bool Equals(Subnet other)
        {
            return Value == other.Value;
        }

        public int CompareTo(Subnet other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}