using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.ModLoader;
using TLoZ.Notes;

namespace TLoZ.Players
{
    public partial class TLoZPlayer : ModPlayer
    {
        private List<Note> _currentNotes;


        public void PlayNote(Note note)
        {
            if (_currentNotes.Count > _currentNotes.Capacity - 1)
            {
                _currentNotes.RemoveAt(7);
                _currentNotes.Insert(0, note);

                return;
            }

            _currentNotes.Add(note);

            note.Play();
        }


        public void InitializeOcarina()
        {
            _currentNotes = new List<Note>(8);
        }

        public void SetOcarinaControls()
        { 
            // Blocks movement, but you can still cancel out of it by using ocarina again
            if (IsPlayingInstrument)
                BlockInputs(true, true, false, true);
        }

        public void PostUpdateOcarina()
        {
            if (!IsPlayingInstrument)
                _currentNotes.Clear();
        }

        public void ProcessOcarinaTriggers(TriggersSet triggersSet)
        {
            if (!IsPlayingInstrument)
                return;

            if (TLoZInput.HasTriggeredKey(Keys.A))
                PlayNote(NoteManager.Instance.Get<NoteA>());

            if (TLoZInput.HasTriggeredKey(Keys.Left))
                PlayNote(NoteManager.Instance.Get<NoteLeft>());

            if (TLoZInput.HasTriggeredKey(Keys.Right))
                PlayNote(NoteManager.Instance.Get<NoteRight>());

            if (TLoZInput.HasTriggeredKey(Keys.Up))
                PlayNote(NoteManager.Instance.Get<NoteUp>());

            if (TLoZInput.HasTriggeredKey(Keys.Down))
                PlayNote(NoteManager.Instance.Get<NoteDown>());

            if (TLoZInput.HasTriggeredKey(Keys.X))
                _currentNotes.Clear();
        }


        //Currently played notes
        public IReadOnlyList<Note> CurrentNotes => _currentNotes.AsReadOnly();

        public bool IsPlayingInstrument { get; set; }
    }
}
