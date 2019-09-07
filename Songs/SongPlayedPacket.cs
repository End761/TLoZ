using System.IO;
using Terraria;
using Terraria.ID;
using TLoZ.Players;
using TLoZ.Worlds;
using WebmilioCommons.Networking.Packets;

namespace TLoZ.Songs
{
    public class SongPlayedPacket : ModPlayerNetworkPacket<TLoZPlayer>
    {
        public string Name
        {
            get => Mod.GetModWorld<TLoZWorld>().CurrentSong.Song.UnlocalizedName;
            set
            {
                Song song = SongManager.Instance[Name];

                if (Main.dedServ)
                    song.OnPlay(ModPlayer, (SongVariant)Variant);
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                    song.Play(ModPlayer, (SongVariant)Variant);
            }
        }

        public int Variant { get; set; }
    }
}