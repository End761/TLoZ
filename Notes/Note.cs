using Terraria;
using Terraria.ModLoader;
using WebmilioCommons.Extensions;
using WebmilioCommons.Managers;

namespace TLoZ.Notes
{
    public abstract class Note : IHasUnlocalizedName
    {
        protected Note(string name, float offset, string soundLoopPath = "")
        {
            UnlocalizedName = "note." + name;
            TexturePath = GetType().GetTexturePath();

            SoundPath = GetType().GetPath();
            SoundLoopPath = soundLoopPath;

            HeightOffset = offset;
        }


        public virtual void Play()
        {
            Main.PlaySound(SoundLoader.customSoundType, -1, -1, TLoZMod.Instance.GetSoundSlot(SoundType.Custom, SoundPath));
        }


        ///<summary>Determines height at which the note will be displayed when played. The higher the value, the lower the note is displayed.</summary> 
        public float HeightOffset { get; }

        public string UnlocalizedName { get; }

        /// <summary>Path to Note's texture displayed in UI.</summary>
        public virtual string TexturePath { get; }

        /// <summary>Path to sound played when tapping the note.</summary>
        public virtual string SoundPath { get; }

        /// <summary>Path to sound played when holding the note</summary>
        public string SoundLoopPath { get; }
    }
}
