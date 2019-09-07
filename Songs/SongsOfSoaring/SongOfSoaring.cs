using System;
using TLoZ.Notes;
using TLoZ.Players;

namespace TLoZ.Songs.SongsOfSoaring
{
    public class SongOfSoaring : Song
    {
        public SongOfSoaring() : base("songOfSoaring", 
            new TimeSpan(0, 0, 0, 4, 96), TimeSpan.Zero, 
            new NoteDown(), new NoteLeft(), new NoteUp(), new NoteDown(), new NoteLeft(), new NoteUp())
        {
        }


        public override bool CanPlay(TLoZPlayer tlozPlayer) => tlozPlayer.InvertedSongOfSoaringPosition != default;

        public override void PostPlay(TLoZPlayer tlozPlayer, SongVariant variant)
        {
            tlozPlayer.player.Teleport(tlozPlayer.InvertedSongOfSoaringPosition, 4);
            tlozPlayer.InvertedSongOfSoaringPosition = default;
        }
    }
}