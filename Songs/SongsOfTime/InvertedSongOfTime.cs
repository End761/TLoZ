using System;
using TLoZ.Notes;
using TLoZ.Players;
using TLoZ.Worlds;

namespace TLoZ.Songs.SongsOfTime
{
    public class InvertedSongOfTime : Song
    {
        public InvertedSongOfTime() : base("invertedSongOfTime",
            new TimeSpan(0, 0, 0, 10, 922), TimeSpan.Zero,
            new NoteDown(), new NoteA(), new NoteRight(), new NoteDown(), new NoteA(), new NoteRight())
        {
        }


        public override void OnPlay(TLoZPlayer tlozPlayer, SongVariant variant)
        {
            Mod.GetModWorld<TLoZWorld>().ToggleInvertedSongOfTime();
        }
    }
}