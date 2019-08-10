using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TLoZ.Extensions;
using TLoZ.Players;

namespace TLoZ.Network.Packets
{
    public sealed class PlayerParagliderStatePacket : NetworkPacket
    {
        public override bool Receive(BinaryReader reader, int fromWho)
        {
            byte whichPlayer = reader.ReadByte();
            bool paragliding = reader.ReadBoolean();

            if (Main.netMode == NetmodeID.Server)
                SendPacketToAllClients(fromWho, whichPlayer, paragliding);

            TLoZPlayer tlozPlayer = TLoZPlayer.Get(Main.player[whichPlayer]);
            tlozPlayer.Paragliding = paragliding;

            return true;
        }


        public override void SendPacketToServer(int fromWho, params object[] args) => base.SendPacketToServer(fromWho, fromWho);

        protected override void SendPacket(ModPacket packet, int toWho, int fromWho, params object[] args)
        {
            packet.WriteByte(args[0]);
            packet.WriteBoolean(args[1]);
        }
    }
}
