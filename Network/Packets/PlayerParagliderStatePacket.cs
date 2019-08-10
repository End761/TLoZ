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
            int whichPlayer = reader.ReadInt32();
            bool paragliding = reader.ReadBoolean();

            if (Main.netMode == NetmodeID.Server)
                SendPacketToAllClients(fromWho, whichPlayer, paragliding);

            TLoZPlayer tlozPlayer = TLoZPlayer.Get(Main.player[whichPlayer]);
            tlozPlayer.Paragliding = paragliding;

            return true;
        }

        protected override void SendPacket(ModPacket packet, int toWho, int fromWho, params object[] args)
        {
            packet.Write((int) args[0]);
            packet.Write((bool) args[1]);

            packet.Send(toWho, fromWho);
        }
    }
}
