using System;
using System.Collections.Generic;
using TLoZ.Notes;
using WebmilioCommons.Managers;

namespace TLoZ.Songs
{
    public abstract class Song : IHasUnlocalizedName
    {
        protected Song(string unlocalizedName, params Note[] composedNotes)
        {
            UnlocalizedName = unlocalizedName;

            ComposedNotes = new List<Note>(composedNotes).AsReadOnly();
        }


        public bool Matches(IEnumerable<Note> notes) => Matches(new List<Note>(notes));

        public bool Matches(List<Note> notes) => Matches(notes.ToArray());

        public virtual bool Matches(params Note[] notes)
        {
            if (notes.Length != ComposedNotes.Count)
                return false;

            for (int i = 0; i < notes.Length; i++)
                if (notes[i] == null || notes[i] != ComposedNotes[i])
                    return false;

            return true;
        }


        public string UnlocalizedName { get; }

        public IReadOnlyList<Note> ComposedNotes { get; }
    }
}