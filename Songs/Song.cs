using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using TLoZ.Notes;
using TLoZ.Players;
using TLoZ.Worlds;
using WebmilioCommons.Extensions;
using WebmilioCommons.Managers;
using SoundType = Terraria.ModLoader.SoundType;

namespace TLoZ.Songs
{
    public abstract class Song : IHasUnlocalizedName
    {
        internal const string SONG_PATH = "Sounds/Music/Songs/";


        protected Song(string unlocalizedName, TimeSpan normalDuration, TimeSpan fullDuration, params Note[] composedNotes)
        {
            UnlocalizedName = "song." + unlocalizedName;

            NormalDuration = (int) Math.Ceiling(normalDuration.TotalSeconds * Constants.TICKS_PER_SECOND);
            FullDuration = (int)Math.Ceiling(fullDuration.TotalSeconds * Constants.TICKS_PER_SECOND);

            Notes = new List<Note>(composedNotes).AsReadOnly();

            string songName = GetType().Name;
            NormalSongPath = SONG_PATH + songName;
            FullSongPath = SONG_PATH + songName + "Full";

            Mod = GetType().GetModFromType();
        }


        public bool Matches(IEnumerable<Note> notes) => Matches(new List<Note>(notes));

        public bool Matches(List<Note> notes) => Matches(notes.ToArray());

        public virtual bool Matches(params Note[] notes)
        {
            if (notes.Length != Notes.Count)
                return false;

            for (int i = 0; i < notes.Length; i++)
                if (notes[i] == null || !notes[i].Equals(Notes[i]))
                    return false;

            return true;
        }


        public SongVariant GetSongVariant(TLoZPlayer tlozPlayer) => SongVariant.Normal;


        public bool TryPlay(TLoZPlayer tlozPlayer, SongVariant variant = SongVariant.Normal)
        {
            if (TLoZMod.Instance.GetModWorld<TLoZWorld>().IsSongPlaying)
            {
                Main.NewText("There is already a song playing in the world!");
                return false;
            }

            if (!CanPlay(tlozPlayer))
                return false;

            Play(tlozPlayer, variant);
            return true;
        }

        public void Play(TLoZPlayer tlozPlayer, SongVariant variant)
        {
            //Main.PlaySound(Mod.GetSoundSlot(SoundType.Music, ))
            Mod.GetModWorld<TLoZWorld>().PlaySong(tlozPlayer, this, variant);
            OnPlay(tlozPlayer, variant);
        }

        public virtual void OnPlay(TLoZPlayer tlozPlayer, SongVariant variant) { }
        public virtual void PostPlay(TLoZPlayer tlozPlayer, SongVariant variant) { }

        public virtual bool CanPlay(TLoZPlayer tlozPlayer) => true;


        public string GetPath(SongVariant variant) => variant == SongVariant.Normal ? NormalSongPath : FullSongPath;
        public int GetDuration(SongVariant variant) => variant == SongVariant.Normal ? NormalDuration : FullDuration;


        public string UnlocalizedName { get; }

        public int NormalDuration { get; }
        public int FullDuration { get; }

        public IReadOnlyList<Note> Notes { get; }

        public virtual string NormalSongPath { get; }
        public virtual string FullSongPath { get; }

        public Mod Mod { get; }
    }
}