namespace org.dmxc.wkdt.Tests
{
    public static class Tools
    {
        public static byte[] Serialize(object value)
        {
#if !NET8_0_OR_GREATER
            byte[]? data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                //Format the object as Binary

                BinaryFormatter formatter = new BinaryFormatter();
                //It serialize the employee object
#pragma warning disable SYSLIB0011 // Typ oder Element ist veraltet
                formatter.Serialize(ms, value);
#pragma warning restore SYSLIB0011 // Typ oder Element ist veraltet
                data = ms.GetBuffer();

                ms.Close();
            }
            return data;
#else
            return JsonSerializer.SerializeToUtf8Bytes(value);
#endif
        }
        public static T? Deserialize<T>(byte[] data)
        {

#if !NET8_0_OR_GREATER
            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011 // Typ oder Element ist veraltet
                return (T)formatter.Deserialize(ms);
#pragma warning restore SYSLIB0011 // Typ oder Element ist veraltet
            }
#else
            return JsonSerializer.Deserialize<T>(new ReadOnlySpan<byte>(data));
#endif
        }
    }
}
