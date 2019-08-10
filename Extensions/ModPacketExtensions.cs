using Terraria.ModLoader;

namespace TLoZ.Extensions
{
    public static class ModPacketExtensions
    {
        public static void WriteBoolean(this ModPacket modPacket, object value) => modPacket.Write((bool) value);
        public static void WriteByte(this ModPacket modPacket, object value) => modPacket.Write((byte) value);
    }
}