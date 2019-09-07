using TLoZ.Players;
using WebmilioCommons.Networking.Packets;

namespace TLoZ.Network.Packets
{
    public sealed class PlayerParagliderStatePacket : ModPlayerNetworkPacket<TLoZPlayer>
    {
        public bool IsParagliding
        {
            get => ModPlayer.Paragliding;
            set => ModPlayer.Paragliding = value;
        }
    }
}
