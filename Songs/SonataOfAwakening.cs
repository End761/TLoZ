using System;
using Microsoft.Xna.Framework;
using TLoZ.Notes;
using TLoZ.Players;

namespace TLoZ.Songs
{
    public class SonataOfAwakening : Song
    {
        public SonataOfAwakening() : base("sonataOfAwakening", 
            new TimeSpan(0, 0, 0, 5, 491), new TimeSpan(0, 0, 0, 24, 576), 
            new NoteUp(), new NoteLeft(), new NoteUp(), new NoteLeft(), new NoteA(), new NoteRight(), new NoteA())
        {
        }
    }
}