using System;
using TLoZ.Notes;
using TLoZ.Players;

namespace TLoZ.Songs.SongsOfTime
{
    public class SongOfTime : Song
    {
        public SongOfTime() : base("songOfTime", 
            new TimeSpan(0, 0, 0, 12, 288), TimeSpan.Zero, 
            new NoteDown(), new NoteA(), new NoteRight(), new NoteDown(), new NoteA(), new NoteRight())
        {
        }


        public override void OnPlay(TLoZPlayer tlozPlayer, SongVariant variant)
        {
            
        }
    }
}