using System;

namespace Parser
{
    internal static class StaticTools
    {
        public static ulong NextULong(this Random ran)
        {
            byte[] output = new byte[sizeof(ulong)];
            ran.NextBytes(output);
            return BitConverter.ToUInt64(output);
        }
    }
}