using TLoZ.Commons;

namespace TLoZ.Notes
{
    public abstract class Note : IHasUnlocalizedName
    {
        public Note(string name, string texturePath, float offset, string soundPath = "", string soundLoopPath = "")
        {
            UnlocalizedName = name;
            TexturePath = texturePath;

            SoundPath = soundPath;
            SoundLoopPath = soundLoopPath;

            HeightOffset = offset;
        }


        ///<summary>
        /// Determines height at which the note will be displayed when played.
        /// Bigger == lower
        ///</summary> 
        public float HeightOffset { get; }

        public string UnlocalizedName { get; }

        /// <summary>
        /// Path to Note's texture displayed in UI.
        /// </summary>
        public string TexturePath { get; }

        /// <summary>
        /// Path to sound thats for tapping the note
        /// </summary>
        public string SoundPath { get; }

        /// <summary>
        /// Path to sound thats for holding the note
        /// </summary>
        public string SoundLoopPath { get; }
    }
}
