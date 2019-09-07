using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using TLoZ.Notes;

namespace TLoZ.Players
{
    public partial class TLoZPlayer : ModPlayer
    {
        public void InitializeOcarina()
        {
            CurrentNotes = new List<Note>(8);
        }

        public void SetOcarinaInputs()
        { 
            // Blocks movement, but you can still cancel out of it by using ocarina again
            if (IsPlayingInstrument)
                BlockInputs(true, true, false, true);
        }

        public void PostUpdateOcarina()
        {
            if (!IsPlayingInstrument)
                CurrentNotes.Clear();
        }

        public void ProcessOcarinaTriggers(TriggersSet triggersSet)
        {
            if (IsPlayingInstrument)
            {
                if (TLoZInput.HasTriggeredKey(Keys.A))
                    AddNote(new NoteA());

                if (TLoZInput.HasTriggeredKey(Keys.Left))
                    AddNote(new NoteLeft());

                if (TLoZInput.HasTriggeredKey(Keys.Right))
                    AddNote(new NoteRight());

                if (TLoZInput.HasTriggeredKey(Keys.Up))
                    AddNote(new NoteUp());

                if (TLoZInput.HasTriggeredKey(Keys.Down))
                    AddNote(new NoteDown());

                if (TLoZInput.HasTriggeredKey(Keys.X))
                    CurrentNotes.Clear();
            }
        }

        private void AddNote(Note note)
        {
            if (note.SoundPath != "")
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, note.SoundPath));

            if (CurrentNotes.Count > CurrentNotes.Capacity - 1)
            {
                CurrentNotes.RemoveAt(7);
                CurrentNotes.Insert(0, note);
                return;
            }

            CurrentNotes.Add(note);
        }

        //Currently played notes
        public List<Note> CurrentNotes { get; private set; }

        public bool IsPlayingInstrument { get; set; }
    }
}
