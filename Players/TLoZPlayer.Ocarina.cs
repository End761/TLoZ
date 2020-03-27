using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using TLoZ.Notes;
using TLoZ.Songs;
using TLoZ.Worlds;

namespace TLoZ.Players
{
    public partial class TLoZPlayer : ModPlayer
    {
        public const float 
            SECONDS_BEFORE_CHECK_SONG = 1.2f,
            SECONDS_BEFORE_CANCEL_INSTRUMENT = 5;


        public void PlayNote(Note note)
        {
            if (CurrentNotes.Count > CurrentNotes.Capacity - 1)
                CurrentNotes.RemoveAt(0);

            CurrentNotes.Add(note);

            TicksSinceLastNotePlayed = 0;
            note.Play();

            if (Main.LocalPlayer == player)
                new NotePlayedPacket()
                {
                    Note = note.UnlocalizedName
                }.Send();
        }

        public void CancelInstrument()
        {
            IsPlayingInstrument = false;

            if (!Main.dedServ)
                UIManager.InstrumentPlayUIState.Visible = false;
        }


        public void InitializeOcarina()
        {
            CurrentNotes = new List<Note>(8);
        }

        public void SetOcarinaControls()
        {
            // Blocks movement, but you can still cancel out of it by using ocarina again
            if (IsPlayingInstrument)
                BlockInputs(true, true, false, true);
        }


        private void ResetEffectOcarina()
        {
            WorldSong worldSong = ModContent.GetInstance<TLoZWorld>().CurrentSong;

            //if (worldSong != null && worldSong.Player == this && worldSong.Variant == SongVariant.Normal)

        }

        private void PostUpdateOcarina()
        {
            if (IsPlayingInstrument)
            {
                if (TicksSinceLastNotePlayed >= SECONDS_BEFORE_CHECK_SONG * Constants.TICKS_PER_SECOND && CurrentNotes.Count > 0)
                {
                    Song song = SongManager.Instance.GetSong(CurrentNotes);

                    if (song != null && song.CanPlay(this) && song.TryPlay(this, song.GetSongVariant(this)))
                    {
                        new SongPlayedPacket().Send();
                        CancelInstrument();
                    }
                }

                if (TicksSinceLastNotePlayed < SECONDS_BEFORE_CANCEL_INSTRUMENT * Constants.TICKS_PER_SECOND)
                    TicksSinceLastNotePlayed++;
                else
                    CancelInstrument();
            }
            else
            {
                TicksSinceLastNotePlayed = 0;
                CurrentNotes.Clear();
            }
        }

        public void ProcessOcarinaTriggers(TriggersSet triggersSet)
        {
            if (!IsPlayingInstrument)
                return;

            if (TLoZInput.HasTriggeredKey(Keys.A))
                PlayNote(new NoteA());

            if (TLoZInput.HasTriggeredKey(Keys.Left))
                PlayNote(new NoteLeft());

            if (TLoZInput.HasTriggeredKey(Keys.Right))
                PlayNote(new NoteRight());

            if (TLoZInput.HasTriggeredKey(Keys.Up))
                PlayNote(new NoteUp());

            if (TLoZInput.HasTriggeredKey(Keys.Down))
                PlayNote(new NoteDown());

            if (TLoZInput.HasTriggeredKey(Keys.X))
                CurrentNotes.Clear();
        }


        //Currently played notes
        public List<Note> CurrentNotes { get; private set; }

        public bool IsPlayingInstrument { get; set; }

        public int TicksSinceLastNotePlayed { get; internal set; }


        public Vector2 InvertedSongOfSoaringPosition { get; set; }
    }
}
