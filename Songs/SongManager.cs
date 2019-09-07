using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using TLoZ.Notes;
using WebmilioCommons.Managers;

namespace TLoZ.Songs
{
    public class SongManager : SingletonManager<SongManager, Song>
    {
        public override void DefaultInitialize()
        {
            foreach (Mod mod in ModLoader.Mods)
            {
                if (mod.Code == null)
                    continue;

                foreach (TypeInfo typeInfo in mod.Code.DefinedTypes.Where(t => t.IsSubclassOf(typeof(Song)) && !t.IsAbstract))
                    Add(Activator.CreateInstance(typeInfo) as Song);
            }


            base.DefaultInitialize();
        }


        public Song GetSong(List<Note> notes)
        {
            for (int i = 0; i < byIndex.Count; i++)
                if (byIndex[i].Matches(notes))
                    return byIndex[i];

            return null;
        }
    }
}