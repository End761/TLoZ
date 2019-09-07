using System;
using TLoZ.Notes;
using TLoZ.Players;

namespace TLoZ.Songs.SongsOfSoaring
{
    public class InvertedSongOfSoaring : Song
    {
        public InvertedSongOfSoaring() : base("invertedSongOfSoaring", 
            new TimeSpan(0, 0, 0, 4, 96), TimeSpan.Zero,
            new NoteUp(), new NoteLeft(), new NoteDown(), new NoteUp(), new NoteLeft(), new NoteDown())
        {
        }


        public override void PostPlay(TLoZPlayer tlozPlayer, SongVariant variant)
        {
            tlozPlayer.InvertedSongOfSoaringPosition = tlozPlayer.player.position;
        }


        public override string NormalSongPath { get; } = "Sounds/Music/Songs/" + nameof(SongOfSoaring);
    }
}