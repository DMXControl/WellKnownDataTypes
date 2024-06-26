﻿namespace org.dmxc.wkdt.Light.ArtNet
{
    [Serializable]
    public readonly struct Universe : IEquatable<Universe>, IComparable<Universe>
    {
        public static readonly Universe Default = new Universe();

#if NET8_0_OR_GREATER
        [JsonInclude]
#endif
        public readonly byte Value;

#if NET8_0_OR_GREATER
        [JsonConstructor]
#endif
        public Universe(byte value)
        {
            if ((byte)(value & 0x0f) != value)
                throw new ArgumentException($"Value (0x{value:x}) out of range! A valid value is between 0x00 and 0x0f.");
            Value = value;
        }

        public static implicit operator byte(in Universe universe)
        {
            return universe.Value;
        }
        public static implicit operator Universe(in byte b)
        {
            return new Universe(b);
        }

        public override bool Equals(object obj)
        {
            return obj is Universe other &&
                   this.Equals(other);
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
            return $"Universe: {Value}(0x{Value:x1})";
        }

        public static bool operator ==(in Universe a, in Universe b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(in Universe a, in Universe b)
        {
            return !a.Equals(b);
        }

        public bool Equals(Universe other)
        {
            return Value == other.Value;
        }

        public int CompareTo(Universe other)
        {
            return Value.CompareTo(other.Value);
        }
    }
}