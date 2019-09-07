using System;
using System.IO;
using TLoZ.Players;
using WebmilioCommons.Networking.Packets;

namespace TLoZ.Notes
{
    public class NotePlayedPacket : ModPlayerNetworkPacket<TLoZPlayer>
    {
        public override bool PostReceive(BinaryReader reader, int fromWho)
        {
            ModPlayer.PlayNote(NoteLoader.NewNote(Note));

            return true;
        }


        public string Note { get; set; }
    }
}