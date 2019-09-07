using System.IO;
using TLoZ.Players;
using WebmilioCommons.Networking.Packets;

namespace TLoZ.Notes
{
    public class NotePlayerPacket : ModPlayerNetworkPacket<TLoZPlayer>
    {
        public override bool PostReceive(BinaryReader reader, int fromWho)
        {
            ModPlayer.PlayNote(Note);

            return true;
        }


        public string Note { get; set; }
    }
}