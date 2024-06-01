using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace org.dmxc.wkdt.Network
{
    [Serializable]
    public readonly struct IPv6Address : IEquatable<IPv6Address>
    {
        public static IPv6Address Empty { get => new IPv6Address("::"); }
        public static IPv6Address LocalHost { get => new IPv6Address("::1"); }

        private static readonly Regex regex = new Regex(@"(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$");

        public readonly BigInteger Raw;
#if NET8_0_OR_GREATER
        [JsonInclude]
#endif
        public readonly string String;

#if NET8_0_OR_GREATER
        [JsonConstructor]
#endif
        public IPv6Address(string @string)
        {
            if (!regex.Match(@string).Success)
                throw new FormatException("The given string is not a IPv6Address");
            // Expand the IPv6 address if it uses the :: shorthand
            string expandedAddress = ExpandIPv6Address(@string);

            String = @string;

            // Split the expanded address into its component hextets
            string[] hextets = expandedAddress.Split(':');

            // Convert each hextet to its corresponding integer value and combine into a BigInteger
            Raw = BigInteger.Zero;
            foreach (string hextet in hextets)
                Raw = (Raw << 16) + BigInteger.Parse(hextet, System.Globalization.NumberStyles.HexNumber);

        }
        public IPv6Address(BigInteger bigInteger) : this()
        {
            Raw = bigInteger;
            byte[] bytes = new byte[16];
            var bigBytes = bigInteger.ToByteArray();
            Array.Copy(bigBytes, bytes, bigBytes.Length);
            String = StringFromBytes(bytes);
        }
        public IPv6Address(byte[] bytes) : this()
        {
            if (bytes.Length != 16)
                throw new ArgumentOutOfRangeException("bytes should be an array with a length of 16");

            Raw = new BigInteger(bytes);
            String = StringFromBytes(bytes);
        }
        public IPv6Address(IEnumerable<byte> enumerable) : this(enumerable.ToArray())
        {
        }
        private static string StringFromBytes(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                sb.Append($"{bytes[i]:x2}{bytes[i + 1]:x2}");
                if (i < 14)
                    sb.Append(':');
            }
            var str = sb.ToString();
            while (str.Contains(":0"))
                str = str.Replace(":0", ":");
            while (str.Contains(":::"))
                str = str.Replace(":::", "::");
            return str;
        }
        private static string ExpandIPv6Address(string ipv6Address)
        {
            if (ipv6Address == "::") return "0000:0000:0000:0000:0000:0000:0000:0000";

            string[] parts = ipv6Address.Split(new string[] { "::" }, StringSplitOptions.None);
            string[] leftParts = parts[0].Split(':');
            string[] rightParts = parts.Length > 1 ? parts[1].Split(':') : new string[0];

            if (string.IsNullOrWhiteSpace(leftParts[0]))
                leftParts = leftParts.Skip(1).ToArray();
            if (rightParts.Length != 0 && string.IsNullOrWhiteSpace(rightParts.Last()))
                rightParts = rightParts.Take(rightParts.Length - 1).ToArray();

            int numZeroesToInsert = 8 - (leftParts.Length + rightParts.Length);

            string[] expandedAddress = new string[8];
            Array.Copy(leftParts, expandedAddress, leftParts.Length);
            for (int i = leftParts.Length; i < leftParts.Length + numZeroesToInsert; i++)
            {
                expandedAddress[i] = "0000";
            }
            Array.Copy(rightParts, 0, expandedAddress, leftParts.Length + numZeroesToInsert, rightParts.Length);
            for (int i = 0; i < expandedAddress.Length; i++)
            {
                switch (expandedAddress[i].Length)
                {
                    case 1:
                        expandedAddress[i] = "000" + expandedAddress[i];
                        break;
                    case 2:
                        expandedAddress[i] = "00" + expandedAddress[i];
                        break;
                    case 3:
                        expandedAddress[i] = "0" + expandedAddress[i];
                        break;
                    default:
                        break;
                }
            }

            return string.Join(":", expandedAddress);
        }


        public static implicit operator IPAddress(IPv6Address address)
        {
            byte[] bytes = new byte[16];
            var bigBytes = address.Raw.ToByteArray();
            Array.Copy(bigBytes, bytes, bigBytes.Length);
            return new IPAddress(bytes);
        }
        public static implicit operator IPv6Address(IPAddress ip)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                return new IPv6Address(ip.GetAddressBytes());
            throw new ArgumentException($"{ip} is not a Valid IPv4 and cant be converted");
        }
        public static implicit operator BigInteger(IPv6Address address)
        {
            return address.Raw;
        }
        public static implicit operator IPv6Address(BigInteger bigInteger)
        {
            return new IPv6Address(bigInteger);
        }
        public static implicit operator byte[](IPv6Address address)
        {
            byte[] bytes = new byte[16];
            var bigBytes = address.Raw.ToByteArray();
            Array.Copy(bigBytes, bytes, bigBytes.Length);
            return bytes;
        }
        public static implicit operator IPv6Address(byte[] bytes)
        {
            return new IPv6Address(bytes);
        }
        public override string ToString()
        {
            return String;
        }

        public static bool operator ==(IPv6Address a, IPv6Address b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(IPv6Address a, IPv6Address b)
        {
            return !a.Equals(b);
        }

        public bool Equals(IPv6Address other)
        {
            if (this.Raw != other.Raw)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is IPv6Address other &&
                   Raw == other.Raw;
        }

        public override int GetHashCode()
        {
            int hashCode = 1916557166;
            hashCode = hashCode * -1521134295 + Raw.GetHashCode();
            return hashCode;
        }
    }
}