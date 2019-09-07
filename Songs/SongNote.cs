using TLoZ.Notes;

namespace TLoZ.Songs
{
    public class SongNote
    {
        public SongNote(Note note, int duration)
        {
            Note = note;

            Duration = duration;
        }


        public Note Note { get; }
        
        public int Duration { get; }
    }
}